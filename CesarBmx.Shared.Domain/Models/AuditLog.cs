﻿using System;
using CesarBmx.Shared.Serialization.Helpers;

namespace CesarBmx.Shared.Domain.Models
{
    public class AuditLog : IEntity
    {
        public string Id => LogId.ToString();
        public Guid LogId { get; private set; }
        public string Action { get; private set; }
        public string Entity { get; private set; }
        public string EntityId { get; private set; }
        public string Json { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public AuditLog() { }
        public AuditLog(string action, object entity, string entityId, DateTime createdAt)
        {
            var entityName = entity.GetType().Name;
            if (entityName == "List`1")
            {
                entityName = entity.GetType().GetGenericArguments()[0].Name + "List";
            }

            LogId = Guid.NewGuid();
            Action = action;
            Entity = entityName;
            EntityId = entityId;
            CreatedAt = createdAt;
            Json = JsonConvertHelper.SerializeObjectRaw(entity);
        }
        public T ModelJsonToObject<T>()
        {
            return JsonConvertHelper.DeserializeObjectRaw<T>(Json);
        }
    }
}