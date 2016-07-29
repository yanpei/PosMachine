using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PosApp.Domain
{
    public class Receipt
    {
        public Receipt(IList<ReceiptItem> receiptItems)
        {
            ReceiptItems = new ReadOnlyCollection<ReceiptItem>(receiptItems);
            Promoted = receiptItems.Sum(r => r.Promoted);
            Total = receiptItems.Sum(r => r.Total) - Promoted;

        }

        public IList<ReceiptItem> ReceiptItems { get; }
        public decimal Total { get; }
        public decimal Promoted { get; set; }
    }
}