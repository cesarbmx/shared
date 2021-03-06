﻿using System;
using CesarBmx.Shared.Common.Providers;
using Microsoft.AspNetCore.Http;

namespace CesarBmx.Shared.Api.Helpers
{
    public class DateTimeProvider: IDateTimeProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DateTimeProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DateTime? GetDateTime()
        {
            //var date = DateTime.Today.AddDays(1);
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null) return null;

            var header = httpContext.Request.Headers["X-Audit-Date"];
            if (header.Count == 0 || !DateTime.TryParse(header, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
                return null;
            return date.Date.AddDays(1);
        }
    }
}