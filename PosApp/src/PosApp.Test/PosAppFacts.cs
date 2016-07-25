using System;
using System.Linq;
using Autofac;
using Xunit;

namespace PosApp.Test
{
    public class PosAppFacts : FactBase
    {
        public PosAppFacts()
            : base(RegisterCustomComponent)
        {
        }

        static void RegisterCustomComponent(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<StubProductRepository>()
                .As<IProductRepository>().InstancePerLifetimeScope();
        }

        [Fact]
        public void should_fail_if_bought_products_are_not_provided()
        {
            var posService = GetContainer().Resolve<PosService>();

            Assert.Throws<ArgumentNullException>(() => posService.GetReceipt(null));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void should_fail_if_one_of_bought_product_amount_is_less_than_or_equal_to_zero(int invalidAmount)
        {
            var posService = GetContainer().Resolve<PosService>();
            var invalidProduct = new BoughtProduct("barcode001", invalidAmount);
            var validProduct = new BoughtProduct("barcode002", 1);

            BoughtProduct[] boughtProducts = {invalidProduct, validProduct};

            Assert.Throws<ArgumentException>(() => posService.GetReceipt(boughtProducts));
        }

        [Fact]
        public void should_fail_if_bought_product_does_not_exist()
        {
            var posService = GetContainer().Resolve<PosService>();
            var notExistedProduct = new BoughtProduct("barcode", 1);

            Assert.Throws<ArgumentException>(() => posService.GetReceipt(new[] {notExistedProduct}));
        }

        [Fact]
        public void should_merge_receipt_items()
        {
            CreateProductFixture(
                new Product {Barcode = "barcodesame"},
                new Product {Barcode = "barcodediff"});
            var posService = GetContainer().Resolve<PosService>();
            var boughtProduct = new BoughtProduct("barcodesame", 1);
            var sameBoughtProduct = new BoughtProduct("barcodesame", 2);
            var differentBoughtProduct = new BoughtProduct("barcodediff", 1);

            Receipt receipt = posService.GetReceipt(
                new[] {boughtProduct, differentBoughtProduct, sameBoughtProduct});

            Assert.Equal(receipt.ReceiptItems.Single(i => i.Product.Barcode == "barcodesame").Amount, 3);
            Assert.Equal(receipt.ReceiptItems.Single(i => i.Product.Barcode == "barcodediff").Amount, 1);
        }

        [Fact]
        public void should_calculate_subtotal()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode", Price = 10M });
            var posService = GetContainer().Resolve<PosService>();

            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode", 2) });

            Assert.Equal(20M, receipt.ReceiptItems.Single().Total);
        }

        [Fact]
        public void should_calculate_total()
        {
            // given
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 10M },
                new Product { Barcode = "barcode002", Price = 20M });

            var posService = GetContainer().Resolve<PosService>();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 2), new BoughtProduct("barcode002", 3) });

            Assert.Equal(80M, receipt.Total);
        }

        void CreateProductFixture(params Product[] products)
        {
            var repository = GetContainer().Resolve<IProductRepository>();
            Array.ForEach(products, p => repository.Save(p));
        }
    }
}