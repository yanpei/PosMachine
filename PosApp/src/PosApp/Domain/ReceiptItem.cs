namespace PosApp.Domain
{
    public class ReceiptItem
    {
        public ReceiptItem(Product product, int amount,decimal promoted)
        {
            Product = product;
            Amount = amount;
            Total = product.Price * amount;
            Promoted = promoted;
        }

        public Product Product { get; }
        public int Amount { get; }
        public decimal Total { get; }
        public decimal Promoted { get; set; }
    }
}