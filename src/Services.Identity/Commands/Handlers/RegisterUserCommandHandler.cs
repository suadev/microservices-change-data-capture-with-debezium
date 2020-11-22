using System.Threading.Tasks;
using System;
using System.Threading;
using Services.Identity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Services.Identity.Events;

namespace Services.Identity.Commands.Handlers
{
    public class RegisterUserCommandHandler : AsyncRequestHandler<RegisterUserCommand>
    {
        private readonly IdentityDBContext _dbContext;
        public RegisterUserCommandHandler(IdentityDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AsNoTracking().AnyAsync(s => s.Email == command.Email))
                throw new ApplicationException("Email is already exist.");

            var user = new User
            {
                Id = command.Id,
                Password = command.Password,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
            };

            var outboxEvent = new Outbox
            {
                Id = Guid.NewGuid(),
                AggregateId = command.Id,
                AggregateType = "User",
                Type = "UserCreated",
                Payload = JsonSerializer.Serialize(new UserCreatedEvent
                {
                    Id = user.Id,
                    Email = user.Email,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                })
            };

            _dbContext.Users.Add(user);

            _dbContext.OutboxEvents.Add(outboxEvent);

            await _dbContext.SaveChangesAsync();
        }
    }
}
