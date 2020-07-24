using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Consumer;
using Common.Kafka.Tests.Fakes;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Common.Kafka.Tests.Consumer
{
    public class KafkaTopicMessageConsumerTests
    {
        [Fact]
        public void StartConsumingSubscribesToCorrectTopic()
        {
            const string expectedTopic = "fake-messages";
            var stubLogger = Mock.Of<ILogger<KafkaTopicMessageConsumer>>();
            var stubMediator = Mock.Of<IMediator>();
            var serviceProvider = BuildServiceProvider(stubMediator);
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            // throw exception to avoid infinite loop
            mockConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);

            var sut = new KafkaTopicMessageConsumer(stubLogger, stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming(expectedTopic, CancellationToken.None);

            mockConsumer.Verify(x => x.Subscribe(expectedTopic));
        }

        [Fact]
        public void StartConsumingConsumesMessageFromConsumer()
        {
            var stubLogger = Mock.Of<ILogger<KafkaTopicMessageConsumer>>();
            var stubMediator = Mock.Of<IMediator>();
            var serviceProvider = BuildServiceProvider(stubMediator);
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);
            // throw exception to avoid infinite loop
            mockConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();

            var sut = new KafkaTopicMessageConsumer(stubLogger, stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming("fake-messages", CancellationToken.None);

            mockConsumer.Verify(x => x.Consume(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void StartConsumingClosesConsumerWhenCancelled()
        {
            var stubLogger = Mock.Of<ILogger<KafkaTopicMessageConsumer>>();
            var stubMediator = Mock.Of<IMediator>();
            var serviceProvider = BuildServiceProvider(stubMediator);
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);

            var sut = new KafkaTopicMessageConsumer(stubLogger, stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming("fake-messages", CancellationToken.None);

            mockConsumer.Verify(x => x.Close());
        }

        [Fact]
        public async Task StartConsumingPublishesConsumedMessageToMediator()
        {
            var fakeMessage = new FakeMessage("some-key-id", "some-property-value")
            {
                Header =
                {
                    Type = typeof(FakeMessage).AssemblyQualifiedName
                }
            };
            var cancellationTokenSource = new CancellationTokenSource();
            var mockMediator = new Mock<IMediator>();
            var serviceProvider = BuildServiceProvider(mockMediator.Object);
            var stubLogger = Mock.Of<ILogger<KafkaTopicMessageConsumer>>();
            var stubConsumer = new Mock<IConsumer<string, string>>();
            stubConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Returns(BuildFakeConsumeResult(fakeMessage));
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(stubConsumer.Object);

            // TODO: find better way to test than relying on async timing
            var sut = new KafkaTopicMessageConsumer(stubLogger, stubMessageConsumerBuilder.Object, serviceProvider);
            Task.Run(() => sut.StartConsuming("fake-messages", cancellationTokenSource.Token));
            await Task.Delay(500);
            cancellationTokenSource.Cancel();

            mockMediator.Verify(x =>
                x.Publish(
                    It.Is<object>(i => i.GetType() == typeof(FakeMessage)),
                    It.IsAny<CancellationToken>()));
        }

        private static ServiceProvider BuildServiceProvider(IMediator mediator)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped(s => mediator);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static ConsumeResult<string, string> BuildFakeConsumeResult(FakeMessage fakeMessage)
        {
            return new ConsumeResult<string, string>
            {
                Message = new Message<string, string>
                {
                    Value = JsonConvert.SerializeObject(fakeMessage)
                }
            };
        }
    }
}