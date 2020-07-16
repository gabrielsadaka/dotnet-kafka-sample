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

                    services.AddOptions<ProducerWorkerOptions>();

                    services.AddTransient(serviceProvider =>
                    {
                        var sampleProducerBuilder = serviceProvider.GetRequiredService<SampleProducerBuilder>();
                        return sampleProducerBuilder.Build();
                    });
                });
        }
    }
}