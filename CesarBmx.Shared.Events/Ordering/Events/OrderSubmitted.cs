using CesarBmx.Shared.Messaging.Ordering.Types;

namespace CesarBmx.Shared.Messaging.Ordering.Events
{
    public class OrderSubmitted
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public string CurrencyId { get; set; } = null!;
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime SubmittedAt { get; set; }

    }
}