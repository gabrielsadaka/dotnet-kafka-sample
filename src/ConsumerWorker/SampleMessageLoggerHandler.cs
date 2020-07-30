using System.Threading;
using System.Threading.Tasks;
using Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ConsumerWorker
{
    public class SampleMessageLoggerHandler : INotificationHandler<MessageNotification<SampleMessage>>
    {
        private readonly ILogger<SampleMessageLoggerHandler> _logger;

        public SampleMessageLoggerHandler(ILogger<SampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MessageNotification<SampleMessage> notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;

            _logger.LogInformation(
                $"Sample message received with key: {message.Key} and value: {message.SomeProperty}");

            return Task.CompletedTask;
        }
    }
}