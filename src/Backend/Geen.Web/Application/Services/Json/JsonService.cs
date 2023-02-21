using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Geen.Web.Application.Services.Json;

public static class JsonService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T FromJson<T>(this string data)
    {
        return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object FromJson(this string data, Type type)
    {
        return JsonSerializer.Deserialize(data, type, JsonSerializerOptions);
    }
}