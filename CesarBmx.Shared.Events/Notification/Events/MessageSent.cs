

namespace CesarBmx.Shared.Messaging.Notification.Events
{
    public class MessageSent
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}