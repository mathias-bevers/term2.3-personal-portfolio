using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace PersonalPortfolio
{
    public static class Utilities
    {
        public static void SetSessionObjectAsJson(this ISession session, string key, object? value) =>
            session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetSessionObjectFromJson<T>(this ISession session, string key)
        {
            string? value = session.GetString(key);
            return ReferenceEquals(value, null) ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static string GetProjectVersion()
        {
            string? appVersion = 
                Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            return string.IsNullOrEmpty(appVersion) ? "not found" : appVersion.Replace(',', ' ');
        }
    }
}