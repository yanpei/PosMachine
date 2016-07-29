using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using PosApp.Repositories.PosApp.Repositories;
using PosApp.Services;

namespace PosApp.Repositories
{

    public class PromotionRepository : IPromotionsRepository
    {
        readonly ISession m_session;

        public PromotionRepository(ISession session)
        {
            m_session = session;
        }

        public IList<string> GetByPromotionType(string promotionName)
        {
            return
                m_session.Query<Promotion>()
                    .Where(p => p.Name == promotionName)
                    .Select(p => p.barcode)
                    .ToList();
        }

        public void Save(Promotion promotion)
        {
            m_session.Save(promotion);
            m_session.Flush();
        }

        public IList<Promotion> GetByBarcode(IList<string> barcodes)
        {
            return  m_session.Query<Promotion>()
                .Where(p => barcodes.Contains(p.barcode)).ToList();
        }

        public void Delete(IList<Promotion> promotions)
        {
            promotions.ForEach(p => m_session.Delete(p));
            m_session.Flush();
        }
    }
}