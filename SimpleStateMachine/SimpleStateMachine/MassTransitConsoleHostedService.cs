using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleStateMachine
{
    public class MassTransitConsoleHostedService :
        IHostedService, IHostApplicationLifetime
    {
        private readonly IBusControl bus;
        private readonly ILogger logger; 
        public MassTransitConsoleHostedService(IBusControl bus, ILoggerFactory loggerFactory, IHostApplicationLifetime hostApplicationLifetime)
        {
            this.bus = bus;
            logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("start async");
            await bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("stop async");
            return bus.StopAsync(cancellationToken);
        }

        public void StopApplication()
        {
            logger.LogDebug("stop application");
            StopAsync(CancellationToken.None);
        }

        public CancellationToken ApplicationStarted { get; }
        public CancellationToken ApplicationStopping { get; }
        public CancellationToken ApplicationStopped { get; }
    }
}