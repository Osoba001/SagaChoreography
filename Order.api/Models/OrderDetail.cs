namespace Order.api.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
    }
}
