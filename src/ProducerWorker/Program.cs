using Common.Kafka;
using Common.Kafka.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProducerWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddOptions<KafkaOptions>()
                        .Bind(hostContext.Configuration.GetSection("Kafka"));

                    services.AddKafkaProducer();
                });
        }
    }
}