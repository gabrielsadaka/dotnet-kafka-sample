using System;
using Microsoft.Extensions.Options;
using Xunit;

namespace ProducerWorker.Tests
{
    public class SampleProducerBuilderTests
    {
        [Fact]
        public void ConstructorShouldCreateSampleProducerBuilder()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());
            
            var sut = new SampleProducerBuilder(producerWorkerOptions);

            Assert.IsType<SampleProducerBuilder>(sut);
        }
        
        [Fact]
        public void ConstructorShouldThrowIfOptionsIsNull()
        {
            IOptions<ProducerWorkerOptions> producerWorkerOptions = null;
            
            Assert.Throws<ArgumentNullException>(() => new SampleProducerBuilder(producerWorkerOptions));
        }

        [Fact]
        public void BuildShouldReturnNonNullProducer()
        {
            var producerWorkerOptions = Options.Create(new ProducerWorkerOptions());
            
            var sut = new SampleProducerBuilder(producerWorkerOptions);

            var producer = sut.Build();

            Assert.NotNull(producer);
        }
    }
}