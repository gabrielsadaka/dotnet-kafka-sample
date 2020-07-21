using System;
using Common.Kafka.Producer;
using Microsoft.Extensions.Options;
using Xunit;

namespace Common.Kafka.Tests.Producer
{
    public class KafkaProducerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleProducerBuilder()
        {
            var kafkaOptions = Options.Create(new KafkaOptions());

            var sut = new KafkaProducerBuilder(kafkaOptions);

            Assert.IsType<KafkaProducerBuilder>(sut);
        }

        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<KafkaOptions> kafkaOptions = null;

            Assert.Throws<ArgumentNullException>(() => new KafkaProducerBuilder(kafkaOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullProducer()
        {
            var kafkaOptions = Options.Create(new KafkaOptions());

            var sut = new KafkaProducerBuilder(kafkaOptions);

            var producer = sut.Build();

            Assert.NotNull(producer);
        }
    }
}