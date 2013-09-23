using System;
using System.Configuration;
using Common;
using Common.Interfaces;
using Common.Ioc;
using MassTransit;
using StructureMap;

namespace Consumer
{
    public class ConsumerClass : Consumes<Common.TextMessage>.All
    {
        public void Consume(Common.TextMessage message)
        {
            Console.WriteLine("Received: " + message.Text);
        }
    }

    public class ConsumeAddCustomerMessage : Consumes<IAddCustomerMessage>.All
    {
        public void Consume(IAddCustomerMessage message)
        {
            Console.WriteLine(string.Format("Received Add Customer: {0}, {1} at {2}", message.LastName, message.FirstName, message.PublishedDateTime));
        }
    }

    public class ConsumeDeleteCustomerMessage : Consumes<IDeleteCustomerMessage>.All
    {
        public void Consume(IDeleteCustomerMessage message)
        {
            Console.WriteLine(string.Format("Delete customer with id {0}.", message.CustomerId));
        }
    }

    class Consumer
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Subscriber, hit return to quit");

            _container = BootstrapContainer();

            var bus = _container.GetInstance<IServiceBus>();

            InspectorGadget.WriteDetails(bus);

            Console.ReadLine();
            Console.WriteLine("Stopping Subscriber");
        }

        private static IContainer BootstrapContainer()
        {
            // create a container with ONLY consumers.  
            // there might be a better way to do this, but I have not found one lately
            var consumerContainer = new Container();
            consumerContainer.Configure(x => x.Scan(s =>
            {
                s.TheCallingAssembly();
                s.WithDefaultConventions();
                s.AddAllTypesOf(typeof(IConsumer));
            }));

            var container = new Container();

            container.Configure(cfg =>
                cfg.For<IServiceBus>().Use(context => ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                sbc.UseMulticastSubscriptionClient();
                sbc.UseSubscriptionService(ConfigurationManager.AppSettings[Constants.AppSettingKeys.SubscriptionServiceQueue]);

                sbc.UseControlBus();
                sbc.ReceiveFrom(ConfigurationManager.AppSettings[Constants.AppSettingKeys.ConsumerSourceQueue]);

                sbc.Subscribe(x => x.LoadFrom(consumerContainer));
            })));

            return container;
        }
    }
}
