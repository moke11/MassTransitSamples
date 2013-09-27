using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace RabbitConsumer
{
    public class RabbitConsumerService
    {
        private readonly IServiceBus _bus;

        public RabbitConsumerService(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
