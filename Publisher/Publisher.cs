using System;
using System.Text;
using System.Threading;
using Common;
using Common.Messages;
using MassTransit;

namespace Publisher
{
    class Publisher
    {
        static void Main(string[] args)
        {
            IServiceBus bus = ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("msmq://localhost/mt_my_publisher");
                sbc.SetPurgeOnStartup(true);

                sbc.UseMsmq(); 
                sbc.UseMulticastSubscriptionClient();
                sbc.UseSubscriptionService("msmq://localhost/mt_subscriptions");
                sbc.UseControlBus();
            });

            InspectorGadget.WriteDetails(bus);

            Thread.Sleep(5000); 

            //bus.Publish(new Common.TextMessage { Text = "Hello world @ " + DateTime.Now.ToString("HH:mm:ss") });

            bus.Publish(new AddCustomerMessage() { FirstName = "Mickey", LastName = "Mouse", PublishedDateTime = DateTime.Now.ToString("HH:mm:ss") });

            //IServiceBus bus = ServiceBusFactory.New(sbc =>
            //{
            //    sbc.UseMsmq(x => x.UseSubscriptionService("msmq://localhost/mt_subscriptions"));
            //    sbc.UseControlBus();
            //    sbc.ReceiveFrom("msmq://localhost/mt_gth_test");
            //});

            //var message = new Common.TextMessage { Text = "Hello world @ " + DateTime.Now.ToString("HH:mm:ss") };
            //var sendTo = "msmq://localhost/mt_gth_test2";
            //bus.GetEndpoint(new Uri(sendTo)).Send(message);

        }

    
    }
}
