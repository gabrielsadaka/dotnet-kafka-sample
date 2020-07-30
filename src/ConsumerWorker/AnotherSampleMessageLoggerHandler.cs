using System.Threading;
using System.Threading.Tasks;
using Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class AnotherSampleMessageLoggerHandler : INotificationHandler<MessageNotification<AnotherSampleMessage>>
    {
        private readonly ILogger<AnotherSampleMessageLoggerHandler> _logger;

        public AnotherSampleMessageLoggerHandler(ILogger<AnotherSampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MessageNotification<AnotherSampleMessage> notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;

            _logger.LogInformation(
                $"Another sample message received with key: {message.Key} and value: {message.AnotherProperty}");

            return Task.CompletedTask;
        }
    }
}