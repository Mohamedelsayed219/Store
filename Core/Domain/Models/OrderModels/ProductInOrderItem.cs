namespace Domain.Models.OrderModels
{
    public class ProductInOrderItem
    {

        public ProductInOrderItem()
        {
            
        }

        public ProductInOrderItem(int productId, string productName, string productUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductUrl = productUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }


    }
}