using Autofac;

namespace PosApp
{
    public class PosAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PosService>();
        }
    }
}