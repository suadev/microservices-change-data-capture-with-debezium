using System.Threading.Tasks;
using System;
using System.Threading;
using Services.Customer.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Services.Customer.Events;

namespace Services.Customer.Commands.Handlers
{
    public class UpdateCustomerCommandHandler : AsyncRequestHandler<UpdateCustomerCommand>
    {
        private readonly CustomerDBContext _dbContext;

        public UpdateCustomerCommandHandler(CustomerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Email == command.Email);
            if (customer == null)
                throw new ApplicationException("Email is not found.");

            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.Address = command.Address;
            customer.PhoneNumber = command.PhoneNumber;
            customer.Gender = command.Gender;
            customer.BirthDate = command.BirthDate;

            var outboxEvent = new Outbox
            {
                Id = Guid.NewGuid(),
                AggregateId = customer.Id,
                AggregateType = "Customer",
                Type = "CustomerUpdated",
                Payload = JsonSerializer.Serialize(new CustomerUpdatedEvent
                {
                    Email = customer.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Address = customer.Address,
                    BirthDate = customer.BirthDate,
                    PhoneNumber = customer.PhoneNumber,
                    Gender = customer.Gender
                })
            };

            _dbContext.Customers.Update(customer);

            _dbContext.OutboxEvents.Add(outboxEvent);

            await _dbContext.SaveChangesAsync();
        }
    }
}
