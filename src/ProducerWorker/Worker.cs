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
                await _messageProducer.ProduceAsync(new SampleMessage($"sample-key-{count}", "sample-property"),
                    stoppingToken);
                await _messageProducer.ProduceAsync(
                    new AnotherSampleMessage($"another-sample-key-{count}", "another-property"), stoppingToken);
                await _messageProducer.ProduceAsync(
                    new OtherSampleMessage($"other-sample-key-{count}", "some-other-property"), stoppingToken);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                count++;
            }
        }
    }
}