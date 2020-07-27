using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Producer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProducerWorker.Messages;

namespace ProducerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageProducer _messageProducer;

        public Worker(ILogger<Worker> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var sampleMessage = new SampleMessage($"sample-key-{count}", "sample-property");
                await _messageProducer.ProduceAsync(sampleMessage.Key, sampleMessage, stoppingToken);

                var anotherSampleMessage = new AnotherSampleMessage($"another-sample-key-{count}", "another-property");
                await _messageProducer.ProduceAsync(anotherSampleMessage.Key, anotherSampleMessage, stoppingToken);

                var otherSampleMessage = new OtherSampleMessage($"other-sample-key-{count}", "some-other-property");
                await _messageProducer.ProduceAsync(otherSampleMessage.Key, otherSampleMessage, stoppingToken);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                count++;
            }
        }
    }
}