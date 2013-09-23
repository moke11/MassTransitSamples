using System;
using MassTransit;

namespace Consumer.Handlers
{
    public class ConsumerClass : Consumes<Common.TextMessage>.All
    {
        public void Consume(Common.TextMessage message)
        {
            Console.WriteLine("Received: " + message.Text);
        }
    }
}