using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using StructureMap.Configuration.DSL;

namespace Common.Ioc
{
    public class MessageRegistry : Registry
    {
        public MessageRegistry()
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
