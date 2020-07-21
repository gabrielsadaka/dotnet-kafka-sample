using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Moq;
using Newtonsoft.Json;
using ProducerWorker.Infrastructure.Messaging;
using ProducerWorker.Messages;
using Xunit;

namespace ProducerWorker.Tests.Infrastructure.Messaging
{
    public class KafkaMessageProducerTests
    {
        [Fact]
        public async Task ProduceShouldProduceMessageWithCorrectTopic()
        {
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var sampleMessage = new SampleMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(sampleMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(sampleMessage.Header.GetTopic(),
                It.IsAny<Message<string, string>>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ProduceShouldProduceMessageWithCorrectKey()
        {
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var sampleMessage = new SampleMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(sampleMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(),
                It.Is<Message<string, string>>(i => i.Key == sampleMessage.Key),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ProduceShouldProduceMessageWithSerialisedMessage()
        {
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var sampleMessage = new SampleMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(sampleMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(),
                It.Is<Message<string, string>>(i =>
                    i.Value == JsonConvert.SerializeObject(sampleMessage)),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ProduceShouldFlushProducer()
        {
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var sampleMessage = new SampleMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(sampleMessage, CancellationToken.None);

            mockProducer.Verify(x => x.Flush(It.IsAny<CancellationToken>()));
        }
    }
}