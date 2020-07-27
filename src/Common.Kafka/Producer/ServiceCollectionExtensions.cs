using Microsoft.Extensions.DependencyInjection;

namespace Common.Kafka.Producer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaProducerBuilder, KafkaProducerBuilder>();

            services.AddSingleton<IMessageProducer, KafkaMessageProducer>();

            return services;
        }
    }
}