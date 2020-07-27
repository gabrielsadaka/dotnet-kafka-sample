using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Producer;
using Common.Kafka.Tests.Fakes;
using Confluent.Kafka;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Common.Kafka.Tests.Producer
{
    public class KafkaMessageProducerTests
    {
        [Fact]
        public async Task ProduceShouldProduceMessageWithCorrectTopic()
        {
            const string expectedTopic = "fake-messages";
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var fakeMessage = new FakeMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(fakeMessage.Key, fakeMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(expectedTopic,
                It.IsAny<Message<string, string>>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ProduceShouldProduceMessageWithCorrectKey()
        {
            const string expectedMessageKey = "some-key-id";
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var fakeMessage = new FakeMessage(expectedMessageKey, "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(expectedMessageKey, fakeMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(),
                It.Is<Message<string, string>>(i => i.Key == expectedMessageKey),
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
            var fakeMessage = new FakeMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(fakeMessage.Key, fakeMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(),
                It.Is<Message<string, string>>(i =>
                    i.Value == JsonConvert.SerializeObject(fakeMessage)),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ProduceShouldProduceMessageTypeAsHeader()
        {
            var stubMessageProducerBuilder = new Mock<IKafkaProducerBuilder>();
            var mockProducer = new Mock<IProducer<string, string>>();
            stubMessageProducerBuilder
                .Setup(x => x.Build())
                .Returns(mockProducer.Object);
            var expectedMessageType = typeof(FakeMessage).AssemblyQualifiedName;
            var fakeMessage = new FakeMessage("some-key-id", "some-property-value");

            var sut = new KafkaMessageProducer(stubMessageProducerBuilder.Object);
            await sut.ProduceAsync(fakeMessage.Key, fakeMessage, CancellationToken.None);

            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(),
                It.Is<Message<string, string>>(i =>
                    Encoding.UTF8.GetString(i.Headers.GetLastBytes("message-type")) == expectedMessageType),
                It.IsAny<CancellationToken>()));
        }
    }
}