using System.Collections.Generic;
using Autofac;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;
using PosApp.Repositories.PosApp.Repositories;
using PosApp.Services;

namespace PosApp.Test.DomainFixtures
{
    public class PromotionFixtures
    {
        readonly ILifetimeScope m_scope;

        public PromotionFixtures(ILifetimeScope scope)
        {
            m_scope = scope;
        }

        public void Create(params Promotion[] promotion)
        {
            var promotionRepository = m_scope.Resolve<IPromotionsRepository>();
            promotion.ForEach(p => promotionRepository.Save(p));
        }

        public IList<string> SelectPromotion(string promotionType)
        {
            var promotionRepository = m_scope.Resolve<IPromotionsRepository>();
            return promotionRepository.GetByPromotionType(promotionType);
        }

    }
}