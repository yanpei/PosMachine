using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentNHibernate.Conventions;
using PosApp.Domain;
using PosApp.Services;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test.Unit
{
    public class PromotionServiceFacts:FactBase
    {
        [Fact]
        public void should_throw_exception_when_createPromotion_given_barcodes_is_null()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" });
            string promotionType = "BUY_TWO_GET_ONE";
            var promotionService = CreatePromotionService();

            Assert.Throws<ArgumentException>(() => promotionService.CreatePromotion(promotionType, null));
        }

        [Fact]
        public void
            should_throw_exception_when_createPromotion_given_barcodes_contains_barcode_which_not_exist_in_products
            ()
        {
            Fixtures.Products.Create(
                   new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" });
            string[] barcodes = { "barcode002" };
            string promotionType = "BUY_TWO_GET_ONE";
            var alterPromotion = CreatePromotionService();

            Assert.Throws<ArgumentException>(() => alterPromotion.CreatePromotion(promotionType, barcodes));
        }

        [Fact]
        public void
            should_created_successfluly_when_createPromotion_given_barcodes_all_are_new()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" });
                
            string[] barcodes = { "barcode001","barcode002" };
            string promotionType = "BUY_TWO_GET_ONE";
            var alterPromotion = CreatePromotionService();
            string result = alterPromotion.CreatePromotion(promotionType, barcodes);
            IList<string> promotionsBarcodes = Fixtures.Promotion.SelectPromotion(promotionType);

            Assert.Equal(true,barcodes.All( b => promotionsBarcodes.Contains(b)));
            Assert.Equal("create promotion successfully",result);
        }

        [Fact]
        public void
            should_created_successfullly_when_createPromotion_given_barcodes_contains_exist_barcode()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" });
            Fixtures.Promotion.Create(
                new Promotion { Name = "BUY_TWO_GET_ONE", barcode = "barcode001" });
            string[] barcodes = { "barcode001", "barcode002" };
            string promotionType = "BUY_TWO_GET_ONE";
            var promotionService = CreatePromotionService();

            promotionService.CreatePromotion(promotionType, barcodes);

            IList<string> promotionsBarcodes = Fixtures.Promotion.SelectPromotion(promotionType);
            Assert.Equal(true, barcodes.All(b => promotionsBarcodes.Contains(b)));

        }
        
        [Fact]
        public void should_return_null_when_getPromotion_type_not_exist()
        {
            string promotionType = "BUY_TWO_GET_ONE";
            var promotionService = CreatePromotionService();
            
            Assert.Equal(true,promotionService.GetPromotion(promotionType).IsEmpty());
        }

        [Fact]
        public void should_return_barcodes_when_getPromotion_given_promotion_type_contain_product()
        {
            string promotionType = "BUY_TWO_GET_ONE";
            Fixtures.Promotion.Create(
                new Promotion { Name = promotionType, barcode = "barcode001" });
            var promotionService = CreatePromotionService();

            IList<string> promotionBarcodes = promotionService.GetPromotion(promotionType);

            Assert.Equal(1,promotionBarcodes.Count);
            Assert.Equal("barcode001",promotionBarcodes[0]);
        }

        [Fact]
        public void should_successful_when_deletePromotion_given_type_and_barcode()
        {
            string promotionType = "BUY_TWO_GET_ONE";
            Fixtures.Promotion.Create(
                new Promotion { Name = promotionType, barcode = "barcode001" },
                new Promotion { Name = promotionType, barcode = "barcode002" });
            IList<string> barcodes = new List<string> { "barcode001","barcode002"};
            var promotionService = CreatePromotionService();

            promotionService.DeletePromotion(promotionType, barcodes);

            IList<string> promotionBarcodes = promotionService.GetPromotion(promotionType);
            Assert.Equal(true,promotionBarcodes.IsEmpty());
        }

        public PromotionServiceFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        PromotionService CreatePromotionService()
        {
            var alterPromotion = GetScope().Resolve<PromotionService>();
            return alterPromotion;
        }
    }
}