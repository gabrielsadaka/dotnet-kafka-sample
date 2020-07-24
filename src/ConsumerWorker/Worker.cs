using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Consumer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsumerWorker
{
    public class Worker : BackgroundService
    {
        private readonly IKafkaMessageConsumerStarter _kafkaMessageConsumerStarter;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IKafkaMessageConsumerStarter kafkaMessageConsumerStarter)
        {
            _logger = logger;
            _kafkaMessageConsumerStarter = kafkaMessageConsumerStarter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _kafkaMessageConsumerStarter.StartConsumers(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}