using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Common.Kafka.Consumer
{
    public class KafkaConsumerBuilder : IKafkaConsumerBuilder
    {
        private readonly KafkaOptions _kafkaOptions;

        public KafkaConsumerBuilder(IOptions<KafkaOptions> kafkaOptions)
        {
            _kafkaOptions = kafkaOptions?.Value
                            ?? throw new ArgumentNullException(nameof(kafkaOptions));
        }

        public IConsumer<string, string> Build()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _kafkaOptions.KafkaBootstrapServers
            };

            var consumerConfig = new ConsumerConfig(config)
            {
                GroupId = _kafkaOptions.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig);

            return consumerBuilder.Build();
        }
    }
}