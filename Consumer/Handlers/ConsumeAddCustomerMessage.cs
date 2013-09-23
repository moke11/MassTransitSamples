using System;
using Common.Interfaces;
using MassTransit;

namespace Consumer.Handlers
{
    public class ConsumeAddCustomerMessage : Consumes<IAddCustomerMessage>.All
    {
        public void Consume(IAddCustomerMessage message)
        {
            Console.WriteLine(string.Format("Received Add Customer: {0}, {1} at {2}", message.LastName, message.FirstName, message.PublishedDateTime));
        }
    }
}