using CesarBmx.Shared.Messaging.CryptoWatcher.Types;

namespace CesarBmx.Shared.Messaging.Ordering.Commands
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class CancelOrder
    {
        public Guid OrderId { get; set; }

    }
}