using Microsoft.AspNetCore.Http;

using System.Text.Json;

namespace ChillScrabble.Extensions
{
    public static class SessionExtensions
    {
        // Метод для сохранения объекта в сессии
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Метод для получения объекта из сессии
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null) return default;

            // Настройка JsonSerializerOptions
            var options = new JsonSerializerOptions
            {
                IncludeFields = true, // Включаем поля (если они используются)
                PropertyNameCaseInsensitive = true // Игнорируем регистр
            };

            return JsonSerializer.Deserialize<T>(value, options);
        }
    }
}