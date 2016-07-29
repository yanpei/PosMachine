using System.Collections.Generic;
using PosApp.Services;

namespace PosApp.Repositories
{
    namespace PosApp.Repositories
    {
        public interface IPromotionsRepository
        {
            IList<string> GetByPromotionType(string promotionName);
            void Save(Promotion promotion);
            IList<Promotion> GetByBarcode(IList<string> barcodes);
            void Delete(IList<Promotion> promotions);
        }
    }
}