using System;
using Common;
using MassTransit;

namespace Consumer
{
    public class ConsumerClass : Consumes<Common.TextMessage>.All
    {
        public void Consume(Common.TextMessage message)
        {
            Console.WriteLine("Received: " + message.Text);
        }
    }

    public class ConsumeAddCustomerMessage  : Consumes<AddCustomerMessage>.All
    {
        public void Consume(AddCustomerMessage message)
        {
            Console.WriteLine(string.Format("Received Add Customer: {0}, {1} at {2}", message.LastName, message.FirstName, message.PublishedDateTime));
        }
    }

    class Consumer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Subscriber, hit return to quit");

            var c = new ConsumerClass();

            IServiceBus bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                sbc.UseMulticastSubscriptionClient();
                sbc.UseSubscriptionService("msmq://localhost/mt_subscriptions");

                sbc.UseControlBus();
                //sbc.VerifyMsmqConfiguration();
                //sbc.UseMulticastSubscriptionClient();
                sbc.ReceiveFrom("msmq://localhost/mt_my_consumer");

                sbc.Subscribe(x => x.Handler<TextMessage>(msg => Console.WriteLine(msg.Text)));
                //sbc.Subscribe(cfg => cfg.Instance(c));

                sbc.Subscribe(x => x.Consumer<ConsumeAddCustomerMessage>());
            });

            InspectorGadget.WriteDetails(bus);

            Console.ReadLine();
            Console.WriteLine("Stopping Subscriber");
        }
    }
}
