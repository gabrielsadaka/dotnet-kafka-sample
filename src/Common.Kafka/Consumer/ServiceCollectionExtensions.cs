using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Kafka.Consumer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services,
            params Type[] handlerAssemblyMarkerTypes)
        {
            services.AddMediatR(handlerAssemblyMarkerTypes);

            services.AddTransient<IKafkaConsumerBuilder, KafkaConsumerBuilder>();

            services.AddTransient(typeof(IKafkaMessageConsumer<>), typeof(KafkaMessageConsumer<>));

            return services;
        }
    }
}