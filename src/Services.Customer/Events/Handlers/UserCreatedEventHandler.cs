using System.Threading.Tasks;
using Services.Customer.Data;
using Services.Customer.Events;
using Shared.Kafka.Consumer;

namespace Services.Customer.Handlers
{
    public class UserCreatedEventHandler : IKafkaHandler<string, UserCreatedEvent>
    {
        private readonly CustomerDBContext _dbContext;

        public UserCreatedEventHandler(CustomerDBContext dbContext)
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