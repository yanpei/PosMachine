using System.Collections.Generic;
using System.Linq;
using PosApp.Repositories;
using PosApp.Repositories.PosApp.Repositories;

namespace PosApp.Services
{
    public class BuyTwoGetOne
    {
        readonly IList<string> promotionBarcodes;
        readonly IPromotionsRepository promotionRepository;
        readonly IProductRepository productRepository;

        public BuyTwoGetOne(IPromotionsRepository promotionRepository,IProductRepository productRepository)
        {
            this.promotionRepository = promotionRepository;
            promotionBarcodes = promotionRepository.GetByPromotionType("BUY_TWO_GET_ONE");
            this.productRepository = productRepository;
        }

        public Dictionary<string, decimal> GetPromoted(
            Dictionary<string, int> boughtDictionary )
        {
            return
                boughtDictionary.ToDictionary(bd => bd.Key,
                    bd => promotionBarcodes.Contains(bd.Key)
                        ? bd.Value/3*
                          productRepository.GetByBarcodes(bd.Key).First().Price
                        : 0);
        }
    }
}