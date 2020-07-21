using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using Moq;
using Xunit;

namespace Common.Kafka.Tests.Consumer
{
    public class KafkaMessageConsumerTests
    {
        [Fact]
        public void StartConsumingSubscribesToCorrectTopic()
        {
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            
            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object);
            Task.Run(() => sut.StartConsuming(cancellationTokenSource.Token));
            cancellationTokenSource.Cancel();
            
            mockConsumer.Verify(x => x.Subscribe("fake-messages"));
        }
        
        [Fact]
        public void StartConsumingConsumesMessageFromConsumer()
        {
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);
            // throw exception to avoid infinite loop
            mockConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();
            
            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object);
            sut.StartConsuming(CancellationToken.None);
            
            mockConsumer.Verify(x => x.Consume(It.IsAny<CancellationToken>()));
        }
        
        [Fact]
        public void StartConsumingClosesConsumerWhenCancelled()
        {
            var stubMessageConsumerBuilder = new Mock<IKafkaConsumerBuilder>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer
                .Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();
            stubMessageConsumerBuilder
                .Setup(x => x.Build())
                .Returns(mockConsumer.Object);
            
            var sut = new KafkaMessageConsumer<FakeMessage>(stubMessageConsumerBuilder.Object);
            sut.StartConsuming(CancellationToken.None);
            
            mockConsumer.Verify(x => x.Close());
        }
    }
}