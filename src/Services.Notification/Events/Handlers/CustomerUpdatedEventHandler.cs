using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Notification.Data;
using Services.Notification.Events;
using Shared.Kafka.Consumer;

namespace Services.Notification.Handlers
{
    public class CustomerUpdatedEventHandler : IKafkaHandler<string, CustomerUpdatedEvent>
    {
        private readonly NotificationDBContext _dbContext;

        public CustomerUpdatedEventHandler(NotificationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(string key, CustomerUpdatedEvent @event)
        {
            var user = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Email == @event.Email);
            if (user == null)
                throw new ApplicationException("Email is not found.");

            user.FirstName = @event.FirstName;
            user.LastName = @event.LastName;
            user.PhoneNumber = @event.PhoneNumber;

            await _dbContext.SaveChangesAsync();
        }
    }
}