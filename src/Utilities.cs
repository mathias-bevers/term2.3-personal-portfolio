using System.Text.Json;

namespace PersonalPortfolio
{
    public static class Utilities
    {
        public static void SetSessionObjectAsJson(this ISession session, string key, object value) => 
            session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetSessionObjectFromJson<T>(this ISession session, string key)
        {
            string? value = session.GetString(key);
            return ReferenceEquals(value, null) ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}