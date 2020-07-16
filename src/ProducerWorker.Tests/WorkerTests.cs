using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ProducerWorker.Infrastructure.Messaging;
using Xunit;

namespace ProducerWorker.Tests
{
    public class WorkerTests
    {
        [Fact]
        public async Task ExecuteAsyncShouldPublishMessage()
        {
            var stubLogger = Mock.Of<ILogger<Worker>>();
            var mockMessageProducer = new Mock<IMessageProducer>();
            var cancellationTokenSource = new CancellationTokenSource();

            var sut = new Worker(stubLogger, mockMessageProducer.Object);

            await sut.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            mockMessageProducer.Verify(x => x.ProduceAsync(It.IsAny<IMessage>(),
                It.IsAny<CancellationToken>()));
        }
    }
}