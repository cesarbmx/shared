using MassTransit;

namespace CesarBmx.Shared.Api.Helpers
{
    public class SimpleNameFormatter :
    IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _original;

        public SimpleNameFormatter(IEntityNameFormatter original)
        {
            _original = original;
        }
        public string FormatEntityName<T>()
        {
            var name = _original.FormatEntityName<T>();

            name = name.Replace("CesarBmx.Shared.Messaging.", string.Empty);
            name = name.Replace(".Commands", string.Empty);
            name = name.Replace(".Events", string.Empty);

            return name;
        }
    }
}