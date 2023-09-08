

using CesarBmx.Shared.Messaging.Notification.Types;

namespace CesarBmx.Shared.Messaging.Notification.Commands
{
    public class SendNotification
    {
        public Guid NotificationId { get; set; } = Guid.Empty;
        public NotificationType NotificationType { get; set; } = NotificationType.TELEGRAM;
        public string UserId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Text { get; set; } = null!;

    }
}