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
            while (!stoppingToken.IsCancellationRequested)
            {
                await _messageProducer.ProduceAsync(new SampleMessage("some-key-id-1", "some-property"), stoppingToken);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}