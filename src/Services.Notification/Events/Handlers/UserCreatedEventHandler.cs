using System.Threading.Tasks;
using Services.Notification.Data;
using Services.Notification.Events;
using Shared.Kafka.Consumer;

namespace Services.Notification.Handlers
{
    public class UserCreatedEventHandler : IKafkaHandler<string, UserCreatedEvent>
    {
        private readonly NotificationDBContext _dbContext;

        public UserCreatedEventHandler(NotificationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(string key, UserCreatedEvent @event)
        {
            _dbContext.Customers.Add(new Data.Customer
            {
                Id = @event.Id,
                Email = @event.Email,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}