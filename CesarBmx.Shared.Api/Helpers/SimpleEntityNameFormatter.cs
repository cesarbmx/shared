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
            var newName = string.Empty;

            name = name.Replace("CesarBmx.Shared.Messaging.", string.Empty);

            if (name.Contains("Events")) newName = _appSettings.ApplicationId + "_" + "Event_";
            if (name.Contains("Commands")) newName = _appSettings.ApplicationId + "_" + "Command_";

            newName += name.Substring(name.IndexOf(":") + 1);

            return newName;
        }
    }
}