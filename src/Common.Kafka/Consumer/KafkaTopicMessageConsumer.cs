using System;
using System.Text;
using System.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Kafka.Consumer
{
    public class KafkaTopicMessageConsumer : IKafkaTopicMessageConsumer
    {
        private readonly IKafkaConsumerBuilder _kafkaConsumerBuilder;
        private readonly ILogger<KafkaTopicMessageConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public KafkaTopicMessageConsumer(ILogger<KafkaTopicMessageConsumer> logger,
            IKafkaConsumerBuilder kafkaConsumerBuilder, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _kafkaConsumerBuilder = kafkaConsumerBuilder;
            _serviceProvider = serviceProvider;
        }

        public void StartConsuming(string topic, CancellationToken cancellationToken)
        {
            using (var consumer = _kafkaConsumerBuilder.Build())
            {
                _logger.LogInformation($"Starting consumer for {topic}");
                consumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        // TODO: log error if missing header
                        var messageTypeEncoded = consumeResult.Message.Headers.GetLastBytes("message-type");
                        var messageTypeHeader = Encoding.UTF8.GetString(messageTypeEncoded);
                        var messageType = Type.GetType(messageTypeHeader);

                        var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, messageType);
                        var messageNotificationType = typeof(MessageNotification<>).MakeGenericType(messageType);
                        var messageNotification = Activator.CreateInstance(messageNotificationType, message);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            mediator.Publish(messageNotification, cancellationToken).GetAwaiter().GetResult();
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