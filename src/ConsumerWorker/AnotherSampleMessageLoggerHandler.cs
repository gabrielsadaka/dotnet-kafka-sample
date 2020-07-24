using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class AnotherSampleMessageLoggerHandler : INotificationHandler<AnotherSampleMessage>
    {
        private readonly ILogger<AnotherSampleMessageLoggerHandler> _logger;

        public AnotherSampleMessageLoggerHandler(ILogger<AnotherSampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AnotherSampleMessage notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Another sample message received with key: {notification.Key} and value: {notification.AnotherProperty}");

            return Task.CompletedTask;
        }
    }
}