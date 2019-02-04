using PaymentApp.Core.Enums;

namespace PaymentApp.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double Total { get; set; }
        public OrderStatus Status { get; set; }
    }
}
