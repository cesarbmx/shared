using CesarBmx.Shared.Application.Settings;
using MassTransit;

namespace CesarBmx.Shared.Api.Helpers
{
    public class SimpleNameFormatter : IEntityNameFormatter
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
            var name = _original.FormatEntityName<T>();
            name = name.Replace("CesarBmx.Shared.Messaging.", string.Empty);

            return name;
        }
    }
}