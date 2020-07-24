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

            // TODO: is there a way to avoid this? Better way to discover handlers?
            services.AddTransient<IKafkaMessageConsumerStarter>(serviceProvider =>
                new KafkaMessageConsumerStarter(serviceProvider, services));

            services.AddTransient<IKafkaConsumerBuilder, KafkaConsumerBuilder>();

            services.AddTransient<IKafkaTopicMessageConsumer, KafkaTopicMessageConsumer>();

            return services;
        }
    }
}