using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using PosApp.Repositories;
using PosApp.Repositories.PosApp.Repositories;

namespace PosApp.Services
{
    public class PromotionService
    {
        readonly IProductRepository m_productRepository;
        readonly IPromotionsRepository m_promotionRepository;

        public PromotionService(IProductRepository productRepository, IPromotionsRepository promotionsRepository)
        {
            m_productRepository = productRepository;
            m_promotionRepository = promotionsRepository;
        }

        public string CreatePromotion(string promotionType,IList<string> barcodes)
        {
            if (!ValidateBarcodes(barcodes))
            {
                throw new ArgumentException("Invalid barcode");
            }
            IList<string> promotionBarcodes =
                m_promotionRepository.GetByPromotionType(promotionType).ToArray();

            var promotions = barcodes.Select(b => 
                new Promotion
                {
                    Id = new Guid(),
                    Name = promotionType,
                    barcode = (promotionBarcodes.IsNotEmpty() && promotionBarcodes.Contains(b))? null:b
                })
                .Where(p => p.barcode != null)
                .ToArray();
            promotions.Each(p => m_promotionRepository.Save(p));
            return "create promotion successfully";
        }

        public bool ValidateBarcodes(IList<string> barcodes)
        {
            if (barcodes == null)
            {
                return false;
            }
            return barcodes.All(b => m_productRepository.GetByBarcodes(b).IsNotEmpty());
        }

        public IList<string> GetPromotion(string promotionType)
        {
            return m_promotionRepository.GetByPromotionType(promotionType);
        }

        public void DeletePromotion(string promotionType, IList<string> barcodes)
        {
            IList<Promotion> promotions = 
                m_promotionRepository.GetByBarcode(barcodes)
                    .Where(p => p.Name.Equals(promotionType))
                    .ToList();
            m_promotionRepository.Delete(promotions);
        }
    }
}