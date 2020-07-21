using System;
using Common.Kafka.Producer;
using Microsoft.Extensions.Options;
using Xunit;

namespace Common.Kafka.Tests
{
    public class KafkaProducerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleProducerBuilder()
        {
            var producerWorkerOptions = Options.Create(new KafkaOptions());

            var sut = new KafkaProducerBuilder(producerWorkerOptions);

            Assert.IsType<KafkaProducerBuilder>(sut);
        }

        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<KafkaOptions> producerWorkerOptions = null;

            Assert.Throws<ArgumentNullException>(() => new KafkaProducerBuilder(producerWorkerOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullProducer()
        {
            var producerWorkerOptions = Options.Create(new KafkaOptions());

            var sut = new KafkaProducerBuilder(producerWorkerOptions);

            var producer = sut.Build();

            Assert.NotNull(producer);
        }
    }
}