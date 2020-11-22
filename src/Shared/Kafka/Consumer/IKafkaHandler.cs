using System.Threading.Tasks;

namespace Shared.Kafka.Consumer
{
    public interface IKafkaHandler<Tk, Tv>
    {
        Task HandleAsync(Tk key, Tv value);
    }
}