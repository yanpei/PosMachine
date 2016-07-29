using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PosApp.Domain;
using PosApp.Dtos.Responses;
using PosApp.Services;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test.Apis
{
    public class PromotionControllerFacts:ApiFactBase
    {
        [Fact]
        public async Task
            should_return_400_badRequest_when_createPromotion_given_barcodes_is_null
            ()
        {
            var httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsync(
                "promotions/BUY_TWO_GET_ONE",
                null);

            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
            var error = await response.Content.ReadAsAsync<ErrorDto>();
            Assert.Equal("promotion barcodes can not be null.",error.Message);
        }

        [Fact]
        public async Task
            should_return_400_badRequest_when_createPromotion_given_barcodes_contains_barcode_which_not_exist_in_products
            ()
        {
            var httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                    "promotions/BUY_TWO_GET_ONE",
                    new[] { "barcode-does-not-exist" });
        
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var error = await response.Content.ReadAsAsync<ErrorDto>();
            Assert.Equal("Invalid barcode", error.Message);
            
        }

        [Fact]
        public async Task
            should_return_201_created_when_createPromotion_given_barcodes_all_are_new_or_contains_exist_barcode
            ()
        {
            var httpClient = CreateHttpClient();
            Fixtures.Products.Create(
                new Product { Barcode = "barcode001", Price = 1, Name = "I do not care" },
                new Product { Barcode = "barcode002", Price = 1, Name = "I do not care" });
            string promotionType = "BUY_TWO_GET_ONE";
            Fixtures.Promotion.Create(
                new Promotion { Name = promotionType, barcode = "barcode002" });

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE",
                new[] {"barcode001", "barcode002"});

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            MessageDto messageDto = await response.Content.ReadAsAsync<MessageDto>();
            Assert.Equal("create promotion successful",messageDto.Message);
        }

        [Fact]
        public async Task
            should_return_200_ok_and_get_barcodes_when_GetPromotion_given_promotion_type_not_null()
        {
            var httpClient = CreateHttpClient();
            string promotionType = "BUY_TWO_GET_ONE";
            Fixtures.Promotion.Create(
                new Promotion { Name = promotionType, barcode = "barcode001" });

            HttpResponseMessage response = await httpClient.GetAsync($"promotions/{promotionType}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            IList<string> promotionBarcodes = await response.Content.ReadAsAsync<IList<string>>();
            Assert.Equal(1,promotionBarcodes.Count);
            Assert.Equal("barcode001",promotionBarcodes[0]);
        }

        [Fact]
        public async Task should_return_ok_when_delete_promotions_given_barcodes_and_promotionType()
        {
            var httpClient = CreateHttpClient();
            string promotionType = "BUY_TWO_GET_ONE";
            Fixtures.Promotion.Create(
                new Promotion { Name = promotionType, barcode = "barcode001" });
            IList<string> barcodes = new List<string> {"barcode001"};

            HttpResponseMessage response = await httpClient.DeleteAsync($"promotions/{promotionType}",barcodes);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            MessageDto messageDto = await response.Content.ReadAsAsync<MessageDto>();
            Assert.Equal("delete successful",messageDto.Message);
        }

        public PromotionControllerFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

    }
}