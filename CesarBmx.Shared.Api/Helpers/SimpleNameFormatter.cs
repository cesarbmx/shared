using CesarBmx.Shared.Application.Settings;
using MassTransit;

namespace CesarBmx.Shared.Api.Helpers
{
    public class SimpleNameFormatter :
    IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _original;
        private readonly AppSettings _appSettings;

        public SimpleNameFormatter(IEntityNameFormatter original, AppSettings appSettings)
        {
            _original = original;
            _appSettings = appSettings;
        }
        public string FormatEntityName<T>()
        {
            var originalName = _original.FormatEntityName<T>();
            var index = originalName.IndexOf(":");
            var eventName = _original.FormatEntityName<T>().Substring(index);
            var simpleName = _appSettings.ApplicationId + eventName;
            return simpleName;
        }
    }
}