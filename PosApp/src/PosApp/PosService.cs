using System;
using System.Collections.Generic;

namespace PosApp
{
    public class PosService
    {
        public PosService(IProductRepository repository)
        {
        }

        public Receipt GetReceipt(IList<BoughtProduct> boughtProducts)
        {
            throw new NotImplementedException();
        }
    }
}