using CesarBmx.Shared.Application.Settings;
using MassTransit;

namespace CesarBmx.Shared.Api.Helpers
{
    public class SimpleEntityNameFormatter :
    IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _original;
        private readonly string _prefix;

        public SimpleEntityNameFormatter(IEntityNameFormatter original, string prefix = null)
        {
            _original = original;
            _prefix = prefix;
        }
        public string FormatEntityName<T>()
        {
            var name = _original.FormatEntityName<T>();

            name = name.Replace("CesarBmx.Shared.Messaging.", string.Empty);
            name = name.Replace(".Commands", string.Empty);
            name = name.Replace(".Events", string.Empty);

            return _prefix + name;
        }
    }
}