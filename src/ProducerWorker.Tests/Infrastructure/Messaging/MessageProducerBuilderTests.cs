using System;
using Microsoft.Extensions.Options;
using ProducerWorker.Infrastructure.Messaging;
using Xunit;

namespace ProducerWorker.Tests.Infrastructure.Messaging
{
    public class MessageProducerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleProducerBuilder()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());

            var sut = new MessageProducerBuilder(producerWorkerOptions);

            Assert.IsType<MessageProducerBuilder>(sut);
        }

        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<ProducerWorkerOptions> producerWorkerOptions = null;

            Assert.Throws<ArgumentNullException>(() => new MessageProducerBuilder(producerWorkerOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullProducer()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());

            var sut = new MessageProducerBuilder(producerWorkerOptions);

            var producer = sut.Build();

            Assert.NotNull(producer);
        }
    }
}