using System.Collections.Generic;

namespace PosApp
{
    public interface IProductRepository
    {
        IList<Product> GetByBarcodes(params string[] barcodes);
        int CountByBarcodes(IList<string> barcodes);
    }
}