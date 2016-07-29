﻿using System;
using System.Linq;
using Autofac;
using PosApp.Domain;
using PosApp.Services;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test.Unit
{
    public class PosAppFacts : FactBase
    {
        public PosAppFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void should_fail_if_bought_products_are_not_provided()
        {
            PosService posService = CreatePosService();

            Assert.Throws<ArgumentNullException>(() => posService.GetReceipt(null));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void should_fail_if_one_of_bought_product_amount_is_less_than_or_equal_to_zero(int invalidAmount)
        {
            PosService posService = CreatePosService();
            var invalidProduct = new BoughtProduct("barcode001", invalidAmount);
            var validProduct = new BoughtProduct("barcode002", 1);

            BoughtProduct[] boughtProducts = {invalidProduct, validProduct};

            Assert.Throws<ArgumentException>(() => posService.GetReceipt(boughtProducts));
        }

        [Fact]
        public void should_fail_if_bought_product_does_not_exist()
        {
            PosService posService = CreatePosService();
            var notExistedProduct = new BoughtProduct("barcode", 1);

            Assert.Throws<ArgumentException>(() => posService.GetReceipt(new[] {notExistedProduct}));
        }

        [Fact]
        public void should_merge_receipt_items()
        {
            CreateProductFixture(
                new Product {Barcode = "barcodesame", Name = "I do not care" },
                new Product {Barcode = "barcodediff", Name = "I do not care" });
            PosService posService = CreatePosService();
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
                new Product { Barcode = "barcode", Price = 10M, Name = "I do not care"});
            PosService posService = CreatePosService();

            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode", 2) });

            Assert.Equal(20M, receipt.ReceiptItems.Single().Total);
        }

        [Fact]
        public void should_calculate_total_not_promoted()
        {
            // given
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 10M, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 20M, Name = "I do not care" });

            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 2), new BoughtProduct("barcode002", 1) });

            Assert.Equal(40M, receipt.Total);
        }

        [Fact]
        public void should_calculate_total_promoted()
        {
            // given
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 10M, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 20M, Name = "I do not care" });
            CreatePromotionFixture(
                new Promotion { Name = "BUY_TWO_GET_ONE", barcode = "barcode002" });

            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 2), new BoughtProduct("barcode002", 3) });

            Assert.Equal(60M, receipt.Total);
        }

        [Fact]
        public void should_get_subpromoted()
        {
            // given
            CreateProductFixture(
                new Product {Barcode = "barcode001", Price = 1, Name = "I do not care"});
            CreatePromotionFixture(
                new Promotion {Name = "BUY_TWO_GET_ONE", barcode = "barcode001"});
            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 3)});

            Assert.Equal(1M, receipt.ReceiptItems.Single().Promoted);
        }

        [Fact]
        public void should_get_subpromoted_is_0()
        {
            // given
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" }
                );
            CreatePromotionFixture(
                new Promotion { Name = "BUY_TWO_GET_ONE", barcode = "barcode001" });
            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode002", 3) });

            Assert.Equal(0M, receipt.ReceiptItems.Single().Promoted);

        }

        [Fact]
        public void should_get_promoted_total()
        {
            CreateProductFixture(
            new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
            new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" });
            CreatePromotionFixture(
                new Promotion { Name = "BUY_TWO_GET_ONE", barcode = "barcode001" });
            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 3) });

            Assert.Equal(1M, receipt.Promoted);
        }

        [Fact]
        public void should_get_promoted_total_0()
        {
            CreateProductFixture(
            new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
            new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" });
            CreatePromotionFixture(
                new Promotion { Name = "BUY_TWO_GET_ONE", barcode = "barcode001" });
            PosService posService = CreatePosService();

            // when
            Receipt receipt = posService.GetReceipt(
                new[] { new BoughtProduct("barcode001", 2) });

            Assert.Equal(0M, receipt.Promoted);
        }




        PosService CreatePosService()
        {
            var posService = GetScope().Resolve<PosService>();
            return posService;
        }


        void CreateProductFixture(params Product[] products)
        {
            Array.ForEach(products, p => Fixtures.Products.Create(p));
        }

        void CreatePromotionFixture(params Promotion[] promotions)
        {
            Array.ForEach(promotions, p => Fixtures.Promotion.Create(p));

        }
    }
}