﻿using System;
using System.Collections.Generic;
using CesarBmx.Shared.Application.Responses;

namespace CesarBmx.Shared.Application.FakeResponses
{
    public static class AuditLogFakeResponse
    {
        public static AuditLogResponse GetFake_Add_Indicator()
        {
            return new AuditLogResponse
            {              
                LogId = Guid.NewGuid(),
                Action = "Add",
                Entity = "Indicator",
                EntityId = "master_price",
                Time = DateTime.Now,
                Json = "{}"
            };
        }
        public static List<AuditLogResponse> GetFake_List()
        {
            return new List<AuditLogResponse>
            {
                GetFake_Add_Indicator()
            };
        }
    }
}
