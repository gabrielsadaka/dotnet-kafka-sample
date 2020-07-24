using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class OtherSampleMessageLoggerHandler : INotificationHandler<OtherSampleMessage>
    {
        private readonly ILogger<OtherSampleMessageLoggerHandler> _logger;

        public OtherSampleMessageLoggerHandler(ILogger<OtherSampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OtherSampleMessage notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Other sample message received with key: {notification.Key} and value: {notification.SomeOtherProperty}");

            return Task.CompletedTask;
        }
    }
}