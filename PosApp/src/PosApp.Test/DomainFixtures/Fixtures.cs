using Autofac;

namespace PosApp.Test.DomainFixtures
{
    public class Fixtures
    {
        public Fixtures(ILifetimeScope scope)
        {
            Products = new ProductFixtures(scope);
        }

        public ProductFixtures Products { get; }
    }
}