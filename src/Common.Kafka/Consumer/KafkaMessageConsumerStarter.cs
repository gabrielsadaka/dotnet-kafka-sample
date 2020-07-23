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
            var messageTypesWithNotificationHandlers = GetMessageTypesWithNotificationHandlers(_services);

            foreach (var messageType in messageTypesWithNotificationHandlers)
            {
                var genericKafkaMessageConsumerType = typeof(IKafkaMessageConsumer<>).MakeGenericType(messageType);
                var kafkaMessageConsumer =
                    (IKafkaMessageConsumer) _serviceProvider.GetRequiredService(genericKafkaMessageConsumerType);

                Task.Run(() => kafkaMessageConsumer.StartConsuming(cancellationToken));
            }
        }

        private static IEnumerable<Type> GetMessageTypesWithNotificationHandlers(IServiceCollection services)
        {
            return services
                .Where(s => s.ServiceType.IsGenericType &&
                            s.ServiceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(s => s.ServiceType.GetGenericArguments()[0])
                .Where(s => typeof(IMessage).IsAssignableFrom(s))
                .Distinct()
                .ToList();
        }
    }
}