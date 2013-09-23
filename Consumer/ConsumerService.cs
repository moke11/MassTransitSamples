using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Consumer
{
    public class ConsumerService
    {
        private readonly IServiceBus _bus;

        public ConsumerService(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Start()
        {

        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
