using System;
using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;
using PosApp.Repositories;
using PosApp.Repositories.PosApp.Repositories;

namespace PosApp.Services
{
    public class PosService
    {
        readonly IProductRepository m_productRepository;
        readonly IPromotionsRepository m_promotionRepository;

        public PosService(IProductRepository productRepository, IPromotionsRepository promotionsRepository)
        {
            m_productRepository = productRepository;
            m_promotionRepository = promotionsRepository;
        }

        public Receipt GetReceipt(IList<BoughtProduct> boughtProducts)
        {
            Validate(boughtProducts);
            IList<ReceiptItem> receiptItems = MergeReceiptItems(boughtProducts);
            return new Receipt(receiptItems);
        }

        IList<ReceiptItem> MergeReceiptItems(IList<BoughtProduct> boughtProducts)
        {
            string[] barcodes = boughtProducts.Select(bp => bp.Barcode).Distinct().ToArray();
            Dictionary<string, Product> boughtProductSet = m_productRepository
                .GetByBarcodes(barcodes)
                .ToDictionary(p => p.Barcode, p => p);
            Dictionary<string,int> boughtDictionary = boughtProducts.GroupBy(bp => bp.Barcode)
                .ToDictionary(g => g.Key, g => g.Sum(bp => bp.Amount));
            var buyTwoGetOne = new BuyTwoGetOne(m_promotionRepository, m_productRepository);
            Dictionary<string, decimal> boughtProductPromoted = buyTwoGetOne.GetPromoted(boughtDictionary);

            return boughtProducts
                .GroupBy(bp => bp.Barcode)
                .Select(g => new ReceiptItem(boughtProductSet[g.Key], g.Sum(bp => bp.Amount),boughtProductPromoted[g.Key]))
                .ToArray();
        }

        void Validate(IList<BoughtProduct> boughtProducts)
        {
            if (boughtProducts == null) { throw new ArgumentNullException(nameof(boughtProducts)); }
            if (boughtProducts.Any(bp => bp.Amount <= 0))
            {
                throw new ArgumentException(nameof(boughtProducts));
            }

            string[] uniqueBarcodes = boughtProducts.Select(bp => bp.Barcode).Distinct().ToArray();
            if (m_productRepository.CountByBarcodes(uniqueBarcodes) != uniqueBarcodes.Length)
            {
                throw new ArgumentException("Some of the products cannot be found.");
            }
        }
    }
}