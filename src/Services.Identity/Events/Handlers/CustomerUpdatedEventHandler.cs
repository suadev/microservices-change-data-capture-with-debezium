using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Data;
using Services.Identity.Events;
using Shared.Kafka.Consumer;

namespace Services.Identity.Handlers
{
    public class CustomerUpdatedEventHandler : IKafkaHandler<string, CustomerUpdatedEvent>
    {
        private readonly IdentityDBContext _dbContext;

        public CustomerUpdatedEventHandler(IdentityDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(string key, CustomerUpdatedEvent @event)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Email == @event.Email);
            if (user == null)
                throw new ApplicationException("Email is not found.");

            user.FirstName = @event.FirstName;
            user.LastName = @event.LastName;

            await _dbContext.SaveChangesAsync();
        }
    }
}