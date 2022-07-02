using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleStateMachine.Engine;

namespace SimpleStateMachine
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

                    var endpointNameFormatter = new KebabCaseEndpointNameFormatter(null, false);

                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddSagaStateMachine<EngineStateMachine, EngineState>()
                            .InMemoryRepository();

                        cfg.AddBus(context =>
                        {
                            return Bus.Factory.CreateUsingInMemory(mt =>
                            {
                                mt.ConfigureEndpoints(context, endpointNameFormatter);
                            });
                        });
                        
                        cfg.AddRequestClient<EngineStatusRequest>();
                    });

                    services.AddHostedService<MassTransitConsoleHostedService>();
                    services.AddHostedService<CommandHostedService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }

        private static IBusControl ConfigureBus(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingInMemory(cfg =>
            {
                var serviceInstanceOptions = new ServiceInstanceOptions()
                    .SetEndpointNameFormatter(KebabCaseEndpointNameFormatter.Instance);

                // cfg.ConfigureEndpoints(cfg.,  serviceInstanceOptions);
                // cfg.ConfigureServiceEndpoints(provider, serviceInstanceOptions);
            });
        }
    }
}