using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PosApp
{
    public class Receipt
    {
        public Receipt(IList<ReceiptItem> receiptItems)
        {
            ReceiptItems = new ReadOnlyCollection<ReceiptItem>(receiptItems);
            Total = receiptItems.Sum(r => r.Total);
        }

        public IList<ReceiptItem> ReceiptItems { get; }
        public decimal Total { get; }
    }
}