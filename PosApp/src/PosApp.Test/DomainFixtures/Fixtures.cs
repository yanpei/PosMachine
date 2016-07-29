using Autofac;

namespace PosApp.Test.DomainFixtures
{
    public class Fixtures
    {
        public Fixtures(ILifetimeScope scope)
        {
            Products = new ProductFixtures(scope);
            Promotion = new PromotionFixtures(scope);
        }

        public ProductFixtures Products { get; }
        public PromotionFixtures Promotion { get; set; }
    }
}