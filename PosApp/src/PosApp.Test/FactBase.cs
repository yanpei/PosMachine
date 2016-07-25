using System;
using Autofac;

namespace PosApp.Test
{
    public class FactBase : IDisposable
    {
        readonly IContainer m_container;

        public FactBase(Action<ContainerBuilder> customRegistration)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new PosAppModule());
            if (customRegistration != null)
            {
                customRegistration(containerBuilder);
            }

            m_container = containerBuilder.Build();
        }

        protected IContainer GetContainer()
        {
            return m_container;
        }

        public void Dispose()
        {
            m_container.Dispose();
        }
    }
}