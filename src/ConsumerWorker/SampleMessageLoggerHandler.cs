using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class SampleMessageLoggerHandler : INotificationHandler<SampleMessage>
    {
        private readonly ILogger<SampleMessageLoggerHandler> _logger;

        public SampleMessageLoggerHandler(ILogger<SampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SampleMessage notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Sample message received with key: {notification.Key} and value: {notification.SomeProperty}");

            return Task.CompletedTask;
        }
    }
}