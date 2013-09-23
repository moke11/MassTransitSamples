using System;
using Common.Interfaces;
using MassTransit;

namespace Consumer.Handlers
{
    public class ConsumeDeleteCustomerMessage : Consumes<IDeleteCustomerMessage>.All
    {
        public void Consume(IDeleteCustomerMessage message)
        {
            Console.WriteLine(string.Format("Delete customer with id {0}.", message.CustomerId));
        }
    }
}