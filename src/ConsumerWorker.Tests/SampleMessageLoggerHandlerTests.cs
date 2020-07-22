using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using ProducerWorker.Messages;
using Xunit;

namespace ConsumerWorker.Tests
{
    public class SampleMessageLoggerHandlerTests
    {
        [Fact]
        public void HandleLogsSampleMessageProperties()
        {
            var mockLogger = new Mock<ILogger<SampleMessageLoggerHandler>>();

            var sut = new SampleMessageLoggerHandler(mockLogger.Object);
            const string expectedKey = "some-key";
            const string expectedSomePropertyValue = "some-property-value";
            var sampleMessage = new SampleMessage(expectedKey, expectedSomePropertyValue);
            sut.Handle(sampleMessage, CancellationToken.None);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals(
                            $"Message received with key: {expectedKey} and value: {expectedSomePropertyValue}",
                            o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once);
        }
    }
}