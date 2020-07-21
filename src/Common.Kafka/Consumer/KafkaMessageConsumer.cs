using System;
using System.Linq;
using System.Threading;

namespace Common.Kafka.Consumer
{
    public class KafkaMessageConsumer<T>
        where T : IMessage
    {
        private readonly IKafkaConsumerBuilder _kafkaConsumerBuilder;

        public KafkaMessageConsumer(IKafkaConsumerBuilder kafkaConsumerBuilder)
        {
            _kafkaConsumerBuilder = kafkaConsumerBuilder;
        }

        public void StartConsuming(CancellationToken cancellationToken)
        {
            using (var consumer = _kafkaConsumerBuilder.Build())
            {
                var topic = Attribute.GetCustomAttributes(typeof(T))
                    .OfType<MessageTopicAttribute>()
                    .Single()
                    .Topic;
                
                consumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        consumer.Consume(cancellationToken);
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