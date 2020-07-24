using System;
using System.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                        var messageHeaderJson = JObject.Parse(consumeResult.Message.Value)["Header"];
                        var messageHeader = messageHeaderJson?.ToObject<MessageHeader>();
                        // TODO: log error if missing header
                        var messageType = Type.GetType(messageHeader.Type);

                        var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, messageType);

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