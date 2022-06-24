using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using SimpleStateMachine.Business;
using SimpleStateMachine.Messages;

namespace SimpleStateMachine
{
    public class CommandHostedService : IHostedService
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IRequestClient<EngineStatusRequest> engineRequestClient;
        private HashSet<Guid> Ids = new HashSet<Guid>();
        private Guid currentId;

        public CommandHostedService(IPublishEndpoint publishEndpoint, IRequestClient<EngineStatusRequest> engineRequestClient)
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
                        currentId = NewId.NextGuid();
                        Ids.Add(currentId);
                        var startCommand = new StartEngine()
                        {
                            EngineId = currentId
                        };
                        await publishEndpoint.Publish(startCommand);
                        Console.WriteLine("sent start command");
                        break;
                    case "stop":
                        var stopCommand = new StopEngine()
                        {
                            EngineId = currentId
                        };
                        await publishEndpoint.Publish(stopCommand);
                        Console.WriteLine("sent stop command");
                        break;
                    case "status":
                        var statusRequest = new EngineStatusRequest()
                        {
                            EngineId = currentId
                        };
                        var response = await engineRequestClient.GetResponse<EngineStatusResponse>(statusRequest, cancellationToken);
                        Console.WriteLine($"response state {response.Message.State}");
                        break;
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{this.GetType().Name} stop async");
        }
    }
}