﻿using System;
using Microsoft.AspNetCore.Http;

namespace CesarBmx.Api.Helpers
{
    public class DateTimeProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DateTimeProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DateTime GetDateFromHeader()
        {
            var date = DateTime.Today.AddDays(1);
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null) return date;

            var header = httpContext.Request.Headers["X-Audit-Date"];
            if (header.Count == 0 || !DateTime.TryParse(header, null, System.Globalization.DateTimeStyles.RoundtripKind, out date))
                return date;
            return date.AddDays(1);
        }
    }
}