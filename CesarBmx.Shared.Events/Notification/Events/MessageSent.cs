using CesarBmx.Shared.Messaging.Notification.Types;


namespace CesarBmx.Shared.Messaging.Notification.Events
{
    public class MessageSent
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; } = null!;
        public DeliveryType DeliveryType { get; set; } = DeliveryType.DIRECT;
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}