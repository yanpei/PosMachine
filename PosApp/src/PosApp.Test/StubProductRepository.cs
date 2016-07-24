using System.Collections.Generic;
using System.Linq;

namespace PosApp.Test
{
    class StubProductRepository : IProductRepository
    {
        readonly IList<Product> m_products;

        public StubProductRepository(IList<Product> products)
        {
            m_products = products;
        }

        public IList<Product> GetByBarcodes(params string[] barcodes)
        {
            return m_products.Where(p => barcodes.Contains(p.Barcode)).ToArray();
        }

        public int CountByBarcodes(IList<string> barcodes)
        {
            return m_products.Count(p => barcodes.Contains(p.Barcode));
        }
    }
}