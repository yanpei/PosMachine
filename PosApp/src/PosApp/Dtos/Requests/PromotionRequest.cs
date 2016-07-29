using System.Collections.Generic;

namespace PosApp.Dtos
{
    public class PromotionRequest
    {
        public string PromotionType { get; set; }
        public IList<string> Barcodes { get; set; }
    }
}