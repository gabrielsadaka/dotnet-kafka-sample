using Microsoft.Extensions.DependencyInjection;

namespace Common.Kafka.Producer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
        {
            services.AddTransient<IKafkaProducerBuilder, KafkaProducerBuilder>();

            services.AddTransient<IMessageProducer, KafkaMessageProducer>();

            return services;
        }
    }
}