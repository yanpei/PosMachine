using System;
using System.Collections.Generic;
using System.Linq;

namespace PosApp.Test
{
    class StubProductRepository : IProductRepository
    {
        readonly IList<Product> m_products = new List<Product>();
        
        public IList<Product> GetByBarcodes(params string[] barcodes)
        {
            return m_products.Where(p => barcodes.Contains(p.Barcode)).ToArray();
        }

        public int CountByBarcodes(IList<string> barcodes)
        {
            return m_products.Count(p => barcodes.Contains(p.Barcode));
        }

        public void Save(Product product)
        {
            if (m_products.Any(p => p.Barcode == product.Barcode))
            {
                throw new InvalidOperationException($"Barcode already exist: {product.Barcode}");
            }

            m_products.Add(product);
        }
    }
}