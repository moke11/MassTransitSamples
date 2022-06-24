using RabbitCommon.Interfaces;
using StructureMap.Configuration.DSL;

namespace RabbitCommon.Ioc
{
    public class RabbitMessageRegistry : Registry
    {
        public RabbitMessageRegistry()
        {
            Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                    x.AddAllTypesOf<IMessageBase>();
                });
        }
    }
}
