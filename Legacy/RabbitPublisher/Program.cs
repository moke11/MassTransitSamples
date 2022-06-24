using System;
using System.Configuration;
using System.Threading;
using MassTransit;
using RabbitCommon;
using RabbitCommon.Ioc;
using RabbitCommon.Messages;
using StructureMap;

namespace RabbitPublisher
{
    class Program
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            _container = BootstrapContainer();

            using (var bus = _container.GetInstance<IServiceBus>())
            {
                InspectorGadget.WriteDetails(bus);

                Thread.Sleep(2000);

                bus.Publish(new AddCustomerMessage() { FirstName = "Mickey", LastName = "Mouse", PublishedDateTime = DateTime.Now.ToString("HH:mm:ss") });
                bus.Publish(new DeleteCustomerMessage() { CustomerId = 10 });
            }
        }

        private static IContainer BootstrapContainer()
        {
            var container = new Container();

            // configure messages
            container.Configure(x => x.AddRegistry(new RabbitMessageRegistry()));

            container.Configure(cfg => cfg.For<IServiceBus>().Use(context => ServiceBusFactory.New(sbc =>
                {
                    sbc.ReceiveFrom(ConfigurationManager.AppSettings[Constants.AppSettingKeys.PublisherSourceQueue]);
                    sbc.SetPurgeOnStartup(true);

                    sbc.UseRabbitMq();
                    sbc.UseRabbitMqRouting();

                    //not needed with rabbitmq
                    //sbc.UseSubscriptionService(ConfigurationManager.AppSettings[Constants.AppSettingKeys.SubscriptionServiceQueue]);
                    //sbc.UseControlBus();
                })));

            return container;
        }

    }
}
