using System;
using System.Linq;
using System.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Common.Kafka.Consumer
{
    public class KafkaMessageConsumer<TMessage> : IKafkaMessageConsumer<TMessage>
        where TMessage : IMessage
    {
        private readonly IKafkaConsumerBuilder _kafkaConsumerBuilder;
        private readonly IServiceProvider _serviceProvider;

        public KafkaMessageConsumer(IKafkaConsumerBuilder kafkaConsumerBuilder, IServiceProvider serviceProvider)
        {
            _kafkaConsumerBuilder = kafkaConsumerBuilder;
            _serviceProvider = serviceProvider;
        }

        public void StartConsuming(CancellationToken cancellationToken)
        {
            using (var consumer = _kafkaConsumerBuilder.Build())
            {
                var topic = Attribute.GetCustomAttributes(typeof(TMessage))
                    .OfType<MessageTopicAttribute>()
                    .Single()
                    .Topic;

                consumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var message = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            mediator.Publish(message, cancellationToken).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // do nothing on cancellation
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}