using System;
using Shared.Kafka.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Kafka
{
    public static class RegisterServiceExtensions
    {
        public static IServiceCollection AddKafkaConsumer<Tk, Tv, THandler>(this IServiceCollection services,
            Action<KafkaConsumerConfig<Tk, Tv>> configAction) where THandler : class, IKafkaHandler<Tk, Tv>
        {
            services.AddScoped<IKafkaHandler<Tk, Tv>, THandler>();

            services.AddHostedService<BackGroundKafkaConsumer<Tk, Tv>>();

            services.Configure(configAction);

            return services;
        }
    }
}