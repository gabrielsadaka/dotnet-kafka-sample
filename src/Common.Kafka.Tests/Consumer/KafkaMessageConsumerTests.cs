using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Consumer;
using Common.Kafka.Tests.Fakes;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Common.Kafka.Tests.Consumer
{
    public class KafkaMessageConsumerTests
    {
        [Fact]
        public void StartConsumingSubscribesToCorrectTopic()
        {
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

            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming(CancellationToken.None);

            mockConsumer.Verify(x => x.Subscribe("fake-messages"));
        }

        [Fact]
        public void StartConsumingConsumesMessageFromConsumer()
        {
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

            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming(CancellationToken.None);

            mockConsumer.Verify(x => x.Consume(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void StartConsumingClosesConsumerWhenCancelled()
        {
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

            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object, serviceProvider);
            sut.StartConsuming(CancellationToken.None);

            mockConsumer.Verify(x => x.Close());
        }

        [Fact]
        public async Task StartConsumingPublishesConsumedMessageToMediator()
        {
            var mockMediator = new Mock<IMediator>();
            var serviceProvider = BuildServiceProvider(mockMediator.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var stubConsumer = new Mock<IConsumer<string, string>>();
            var fakeMessage = new FakeMessage("some-key-id", "some-property-value");
            stubConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Returns(BuildFakeConsumeResult(fakeMessage));
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(stubConsumer.Object);

            // TODO: find better way to test than relying on async timing
            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object, serviceProvider);
            Task.Run(() => sut.StartConsuming(cancellationTokenSource.Token));
            await Task.Delay(500);
            cancellationTokenSource.Cancel();

            mockMediator.Verify(x =>
                x.Publish(
                    It.Is<FakeMessage>(i => i.Key == fakeMessage.Key && i.SomeProperty == fakeMessage.SomeProperty),
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