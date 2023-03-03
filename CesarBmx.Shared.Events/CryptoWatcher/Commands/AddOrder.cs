using CesarBmx.Shared.Messaging.CryptoWatcher.Types;

namespace CesarBmx.Shared.Messaging.CryptoWatcher.Commands
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AddOrder
    {
        public Guid OrderId { get; set; }
        public int WatcherId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}