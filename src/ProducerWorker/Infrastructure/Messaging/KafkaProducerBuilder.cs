using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace ProducerWorker.Infrastructure.Messaging
{
    public class KafkaProducerBuilder : IKafkaProducerBuilder
    {
        private readonly ProducerWorkerOptions _producerWorkerOptions;

        public KafkaProducerBuilder(IOptions<ProducerWorkerOptions> producerWorkerOptions)
        {
            _producerWorkerOptions = producerWorkerOptions?.Value ??
                                     throw new ArgumentNullException(nameof(producerWorkerOptions));
        }

        public IProducer<string, string> Build()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _producerWorkerOptions.KafkaBootstrapServers
            };

            var producerBuilder = new ProducerBuilder<string, string>(config);

            return producerBuilder.Build();
        }
    }
}