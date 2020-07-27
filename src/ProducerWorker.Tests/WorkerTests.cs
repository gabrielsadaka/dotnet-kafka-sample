using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Producer;
using Microsoft.Extensions.Logging;
using Moq;
using ProducerWorker.Messages;
using Xunit;

namespace ProducerWorker.Tests
{
    public class WorkerTests
    {
        [Fact]
        public async Task ExecuteAsyncShouldPublishSampleMessage()
        {
            var stubLogger = Mock.Of<ILogger<Worker>>();
            var mockMessageProducer = new Mock<IMessageProducer>();
            var cancellationTokenSource = new CancellationTokenSource();

            var sut = new Worker(stubLogger, mockMessageProducer.Object);

            await sut.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            mockMessageProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<SampleMessage>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ExecuteAsyncShouldPublishAnotherSampleMessage()
        {
            var stubLogger = Mock.Of<ILogger<Worker>>();
            var mockMessageProducer = new Mock<IMessageProducer>();
            var cancellationTokenSource = new CancellationTokenSource();

            var sut = new Worker(stubLogger, mockMessageProducer.Object);

            await sut.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            mockMessageProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<AnotherSampleMessage>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ExecuteAsyncShouldPublishOtherSampleMessage()
        {
            var stubLogger = Mock.Of<ILogger<Worker>>();
            var mockMessageProducer = new Mock<IMessageProducer>();
            var cancellationTokenSource = new CancellationTokenSource();

            var sut = new Worker(stubLogger, mockMessageProducer.Object);

            await sut.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            mockMessageProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<OtherSampleMessage>(),
                It.IsAny<CancellationToken>()));
        }
    }
}