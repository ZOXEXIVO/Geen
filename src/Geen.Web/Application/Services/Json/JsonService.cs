using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Geen.Web.Application.Services.Json
{
    public static class JsonService
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToJson(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FromJson<T>(this string data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object FromJson(this string data, Type type)
        {
            return System.Text.Json.JsonSerializer.Deserialize(data, type, JsonSerializerOptions);
        }
    }
}
