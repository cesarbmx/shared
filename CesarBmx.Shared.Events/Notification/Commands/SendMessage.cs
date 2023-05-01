

namespace CesarBmx.Shared.Messaging.Notification.Commands
{
    public class SendMessage
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; } = null!;
        public string Text { get; set; } = null!;

    }
}