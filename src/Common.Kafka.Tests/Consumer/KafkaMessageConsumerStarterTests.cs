using System.Threading;
using Common.Kafka.Consumer;
using Common.Kafka.Tests.Fakes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Common.Kafka.Tests.Consumer
{
    public class KafkaMessageConsumerStarterTests
    {
        [Fact]
        public void StartConsumersShouldStartSingleConsumerPerMessage()
        {
            var mockKafkaMessageConsumer = new Mock<IKafkaTopicMessageConsumer>();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(mockKafkaMessageConsumer.Object);
            serviceCollection.AddTransient(s => Mock.Of<INotificationHandler<FakeMessage>>());
            serviceCollection.AddTransient(s => Mock.Of<INotificationHandler<OtherFakeMessage>>());
            serviceCollection.AddTransient(s => Mock.Of<INotificationHandler<OtherFakeMessage>>());
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = new KafkaMessageConsumerStarter(serviceProvider, serviceCollection);
            sut.StartConsumers(CancellationToken.None);

            mockKafkaMessageConsumer.Verify(x => x.StartConsuming("fake-messages", It.IsAny<CancellationToken>()),
                Times.Once);
            mockKafkaMessageConsumer.Verify(x => x.StartConsuming("other-fake-messages", It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}