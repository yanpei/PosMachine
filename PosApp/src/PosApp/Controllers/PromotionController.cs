using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PosApp.Dtos.Responses;
using PosApp.Services;

namespace PosApp.Controllers
{
    public class PromotionController:ApiController
    {
        readonly PromotionService m_promotionService;

        public PromotionController(PromotionService promotionService)
        {
            m_promotionService = promotionService;
        }

        [HttpPost]
        public HttpResponseMessage CreatePromotion(string promotionType,string[] barcodes)
        {
            if (barcodes == null)
            {
                throw new HttpException(400, "promotion barcodes can not be null.");
            }

            try
            {
                m_promotionService.CreatePromotion(promotionType,barcodes);
                return Request.CreateResponse(HttpStatusCode.OK, new MessageDto {Message = "create promotion successful"});
            }
            catch (ArgumentException error)
            {
                throw new HttpException(400, error.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPromotion(string promotionType)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                m_promotionService.GetPromotion(promotionType));
        }

        [HttpDelete]
        public HttpResponseMessage DeletePromotion(string promotionType, IList<string> barcodes)
        {
            m_promotionService.DeletePromotion(promotionType, barcodes);
            return Request.CreateResponse(HttpStatusCode.OK,new MessageDto {Message = "delete successful"});
        }

    }
}
