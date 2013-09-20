using System;
using Common;
using MassTransit;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Subscriber, hit return to quit");

            var c = new ConsumerClass();

            IServiceBus bus = ServiceBusFactory.New(sbc =>
            {
                //sbc.UseMsmq();
                sbc.UseMsmq(x =>
                {
                    x.UseSubscriptionService("msmq://localhost/mt_subscriptions");
                    x.UseMulticastSubscriptionClient();
                });

                sbc.UseControlBus();
                //sbc.VerifyMsmqConfiguration();
                //sbc.UseMulticastSubscriptionClient();
                sbc.ReceiveFrom("msmq://localhost/mt_gth_test2");

                sbc.Subscribe(x => x.Handler<TextMessage>(msg => Console.WriteLine(msg.Text)));
                //sbc.Subscribe(cfg => cfg.Instance(c));
            });

            InspectorGadget.WriteDetails(bus);

            Console.ReadLine();
            Console.WriteLine("Stopping Subscriber");
        }
    }
}
