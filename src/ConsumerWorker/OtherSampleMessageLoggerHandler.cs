using System.Threading;
using System.Threading.Tasks;
using Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class OtherSampleMessageLoggerHandler : INotificationHandler<MessageNotification<OtherSampleMessage>>
    {
        private readonly ILogger<OtherSampleMessageLoggerHandler> _logger;

        public OtherSampleMessageLoggerHandler(ILogger<OtherSampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MessageNotification<OtherSampleMessage> notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;

            _logger.LogInformation(
                $"Other sample message received with key: {message.Key} and value: {message.SomeOtherProperty}");

            return Task.CompletedTask;
        }
    }
}