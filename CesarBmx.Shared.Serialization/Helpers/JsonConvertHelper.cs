﻿using Newtonsoft.Json;
using CesarBmx.Shared.Serialization.Resolvers;

namespace CesarBmx.Shared.Serialization.Helpers
{
    public static class JsonConvertHelper
    {
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver =  new NonPublicPropertiesResolver()
            });
        }
    }
}