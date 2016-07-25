using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PosApp.Domain;
using PosApp.Services;

namespace PosApp.Controllers
{
    public class ReceiptController : ApiController
    {
        readonly PosService m_posService;

        public ReceiptController(PosService posService)
        {
            m_posService = posService;
        }

        [HttpPost]
        public HttpResponseMessage BuildReceipt(string[] barcodes)
        {
            try
            {
                Receipt receipt = m_posService.GetReceipt(
                    barcodes.Select(bc => new BoughtProduct(bc, 1)).ToArray());
                return Request.CreateResponse(HttpStatusCode.OK, receipt);
            }
            catch (ArgumentException error)
            {
                throw new HttpException(400, error.Message);
            }
        }
    }
}