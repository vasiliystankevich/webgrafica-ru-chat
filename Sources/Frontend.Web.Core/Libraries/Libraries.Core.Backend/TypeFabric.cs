using System;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi.Repositories;
using Microsoft.Practices.Unity;
using Project.Kernel;

namespace Libraries.Core.Backend
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IWrapper<Random>, Wrapper<Random>>(new InjectionFactory(factory => new Wrapper<Random>(new Random(DateTime.UtcNow.Millisecond))));
            container.RegisterType<ISystemMessagesRepository, SystemMessagesRepository>();
        }
    }
}
