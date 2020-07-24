using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Kafka.Consumer
{
    public class KafkaMessageConsumerStarter : IKafkaMessageConsumerStarter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _services;

        public KafkaMessageConsumerStarter(IServiceProvider serviceProvider, IServiceCollection services)
        {
            _serviceProvider = serviceProvider;
            _services = services;
        }

        public void StartConsumers(CancellationToken cancellationToken)
        {
            var topicsWithNotificationHandlers = GetTopicsWithNotificationHandlers(_services);

            foreach (var topic in topicsWithNotificationHandlers)
            {
                var kafkaTopicMessageConsumer = _serviceProvider.GetRequiredService<IKafkaTopicMessageConsumer>();

                Task.Run(() => kafkaTopicMessageConsumer.StartConsuming(topic, cancellationToken));
            }
        }

        private static IEnumerable<string> GetTopicsWithNotificationHandlers(IServiceCollection services)
        {
            var messageTypesWithNotificationHandlers = services
                .Where(s => s.ServiceType.IsGenericType &&
                            s.ServiceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(s => s.ServiceType.GetGenericArguments()[0])
                .Where(s => typeof(IMessage).IsAssignableFrom(s))
                .Distinct();

            return messageTypesWithNotificationHandlers
                .SelectMany(t => Attribute.GetCustomAttributes(t))
                .OfType<MessageTopicAttribute>()
                .Select(t => t.Topic)
                .Distinct()
                .ToList();
        }
    }
}