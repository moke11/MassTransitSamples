using System;
using System.Configuration;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Common.Messages;
using MassTransit;
using StructureMap;
using Common.Ioc;

namespace Publisher
{
    class Publisher
    {
        private static IContainer _container; 
        private static IServiceBus _bus; 

        static void Main(string[] args)
        {
            _container = BootstrapContainer();

            _bus = _container.GetInstance<IServiceBus>(); 

            InspectorGadget.WriteDetails(_bus);

            Thread.Sleep(5000); 

            _bus.Publish(new AddCustomerMessage() { FirstName = "Mickey", LastName = "Mouse", PublishedDateTime = DateTime.Now.ToString("HH:mm:ss") });
            _bus.Publish(new DeleteCustomerMessage(){ CustomerId = 10});
        }

        private static IContainer BootstrapContainer()
        {
            var container = new Container(); 

            // configure messages
            container.Configure(x => x.AddRegistry(new MessageRegistry()));

            container.Configure(cfg => cfg.For<IServiceBus>().Use(context => ServiceBusFactory.New(sbc =>
                {
                    sbc.ReceiveFrom(ConfigurationManager.AppSettings[Constants.AppSettingKeys.PublisherSourceQueue]);
                    sbc.SetPurgeOnStartup(true);

                    sbc.UseMsmq();
                    sbc.UseMulticastSubscriptionClient();
                    sbc.UseSubscriptionService(ConfigurationManager.AppSettings[Constants.AppSettingKeys.SubscriptionServiceQueue]);
                    sbc.UseControlBus();
                })));

            return container;
        }
    
    }
}
