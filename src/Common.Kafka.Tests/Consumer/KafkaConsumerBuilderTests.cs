using System;
using Common.Kafka.Consumer;
using Microsoft.Extensions.Options;
using Xunit;

namespace Common.Kafka.Tests.Consumer
{
    public class KafkaConsumerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleConsumerBuilder()
        {
            var kafkaOptions = Options.Create(new KafkaOptions());

            var sut = new KafkaConsumerBuilder(kafkaOptions);

            Assert.IsType<KafkaConsumerBuilder>(sut);
        }

        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<KafkaOptions> kafkaOptions = null;

            Assert.Throws<ArgumentNullException>(() => new KafkaConsumerBuilder(kafkaOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullConsumer()
        {
            var kafkaOptions = Options.Create(new KafkaOptions
            {
                KafkaBootstrapServers = "kafka-bootstrap",
                ConsumerGroupId = "test-group-id"
            });

            var sut = new KafkaConsumerBuilder(kafkaOptions);

            var consumer = sut.Build();

            Assert.NotNull(consumer);
        }
    }
}