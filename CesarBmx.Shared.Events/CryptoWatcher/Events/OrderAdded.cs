using CesarBmx.Shared.Messaging.CryptoWatcher.Types;

namespace CesarBmx.Shared.Messaging.CryptoWatcher.Events
{
    public class OrderAdded
    {
        public int OrderId { get; set; }
        public int WatcherId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}