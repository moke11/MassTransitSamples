using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using SimpleStateMachine.Engine;

namespace SimpleStateMachine
{
    public class CommandHostedService : IHostedService
    {
        private readonly IRequestClient<EngineStatusRequest> engineRequestClient;
        private readonly HashSet<Guid> ids = new HashSet<Guid>();
        private readonly IPublishEndpoint publishEndpoint;
        private Guid engineId;

        public CommandHostedService(IPublishEndpoint publishEndpoint,
            IRequestClient<EngineStatusRequest> engineRequestClient)
        {
            this.publishEndpoint = publishEndpoint;
            this.engineRequestClient = engineRequestClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                var input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "start":
                        engineId = NewId.NextGuid();
                        ids.Add(engineId);
                        var startCommand = new StartEngine
                        {
                            EngineId = engineId
                        };
                        await publishEndpoint.Publish(startCommand);
                        Console.WriteLine("sent start command");
                        break;
                    case "stop":
                        var stopCommand = new StopEngine
                        {
                            EngineId = engineId
                        };
                        await publishEndpoint.Publish(stopCommand);
                        Console.WriteLine("sent stop command");
                        break;
                    case "status":
                        var statusRequest = new EngineStatusRequest
                        {
                            EngineId = engineId
                        };
                        var response = await engineRequestClient.GetResponse<EngineStatusResponse>(statusRequest, cancellationToken);
                        Console.WriteLine($"response state {response.Message.State}");
                        break;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} stop async");
            return Task.CompletedTask;
        }
    }
}