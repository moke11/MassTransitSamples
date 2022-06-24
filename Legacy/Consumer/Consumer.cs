using System;
using System.Configuration;
using Common;
using Common.Ioc;
using MassTransit;
using StructureMap;
using Topshelf;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Subscriber, hit return to quit");

            var container = BootstrapContainer();

            var bus = container.GetInstance<IServiceBus>();

            HostFactory.Run(c =>
                {
                    c.SetServiceName("Consumer");
                    c.SetDisplayName("Consumer Display Name");
                    c.SetDescription("a Masstransit sample service for hosting consumers.");

                    // list dependencies 
                    c.DependsOnMsmq();
                    c.DependsOnMsSql();

                    c.RunAsLocalService();

                    c.Service<ConsumerService>(s =>
                        {
                            s.ConstructUsing(builder => container.GetInstance<ConsumerService>());
                            s.WhenStarted(o => o.Start());
                            s.WhenStopped(o =>
                                {
                                    o.Stop();
                                    container.Dispose();
                                });
                        });

                    c.StartAutomatically(); 
                }); 

            InspectorGadget.WriteDetails(bus);

            Console.ReadLine();
            Console.WriteLine("Stopping Subscriber");
        }

        private static IContainer BootstrapContainer()
        {
            var container = new Container();

            container.Configure(cfg =>
                {
                    // configure the consumers
                    cfg.Scan(s =>
                        {
                            s.TheCallingAssembly();
                            s.WithDefaultConventions();
                            s.AddAllTypesOf(typeof(IConsumer));
                        });

                    // configure the service bus
                    cfg.For<IServiceBus>().Use(context => ServiceBusFactory.New(sbc =>
                        {
                            sbc.UseMsmq();
                            sbc.UseMulticastSubscriptionClient();
                            sbc.UseSubscriptionService(
                                ConfigurationManager.AppSettings[Constants.AppSettingKeys.SubscriptionServiceQueue]);

                            sbc.UseControlBus();
                            sbc.ReceiveFrom(ConfigurationManager.AppSettings[Constants.AppSettingKeys.ConsumerSourceQueue]);

                            sbc.Subscribe(x => x.LoadFrom(container));
                        }));
                });

            return container;
        }
    }
}
