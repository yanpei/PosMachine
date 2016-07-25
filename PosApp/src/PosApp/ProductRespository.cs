using System.Collections.Generic;
using NHibernate;

namespace PosApp
{
    class ProductRespository : IProductRepository
    {
        readonly ISession m_session;

        public ProductRespository(ISession session)
        {
            m_session = session;
        }

        public IList<Product> GetByBarcodes(params string[] barcodes)
        {
            throw new System.NotImplementedException();
        }

        public int CountByBarcodes(IList<string> barcodes)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}