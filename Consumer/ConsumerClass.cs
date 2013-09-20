using MassTransit;
using System;

namespace Consumer
{
    public class ConsumerClass : Consumes<Common.TextMessage>.All
    {
        public void Consume(Common.TextMessage message)
        {
            Console.WriteLine("Received: " + message.Text);
        }
    }
}
