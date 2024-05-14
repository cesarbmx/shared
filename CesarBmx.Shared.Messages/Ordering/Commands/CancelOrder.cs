

namespace CesarBmx.Shared.Messaging.Ordering.Commands
{
    public class CancelOrder
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = null!;
    }
}