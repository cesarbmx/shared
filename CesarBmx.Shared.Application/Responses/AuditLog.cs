﻿using System;


namespace CesarBmx.Shared.Application.Responses
{
    public class AuditLog
    {
        public Guid LogId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public DateTime Time { get; set; }
        public string Json { get; set; }
    }
}
