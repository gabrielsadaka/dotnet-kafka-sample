using System;
using Microsoft.Extensions.Options;
using ProducerWorker.Infrastructure.Messaging;
using Xunit;

namespace ProducerWorker.Tests.Infrastructure.Messaging
{
    public class KafkaProducerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleProducerBuilder()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());

            var sut = new KafkaProducerBuilder(producerWorkerOptions);

            Assert.IsType<KafkaProducerBuilder>(sut);
        }

        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<ProducerWorkerOptions> producerWorkerOptions = null;

            Assert.Throws<ArgumentNullException>(() => new KafkaProducerBuilder(producerWorkerOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullProducer()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());

            var sut = new KafkaProducerBuilder(producerWorkerOptions);

            var producer = sut.Build();

            Assert.NotNull(producer);
        }
    }
}