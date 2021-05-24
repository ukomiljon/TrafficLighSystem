using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class JsonSerializer
    {
        public static JsonSerializerSettings _jsonSettings => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Newtonsoft.Json.Formatting.Indented,
            //ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        };

        public static string Serialize(dynamic jsonObject)
        {
            return JsonConvert.SerializeObject(jsonObject, _jsonSettings);
        }

        public static T Deserialize<T>(string serializedString)
        {
            return JsonConvert.DeserializeObject<T>(serializedString, _jsonSettings);
        }


    }
}
