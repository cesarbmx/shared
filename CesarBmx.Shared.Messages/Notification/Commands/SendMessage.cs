﻿using CesarBmx.Shared.Messaging.Notification.Types;


namespace CesarBmx.Shared.Messaging.Notification.Commands
{
    public class SendMessage
    {
        public Guid NotificationId { get; set; } = Guid.Empty;
        public string UserId { get; set; } = null!;
        public DeliveryType DeliveryType { get; set; } = DeliveryType.DIRECT;
        public string Text { get; set; } = null!;
        public DateTime? ScheduledFor { get; set; } = null!;

    }
}