using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace BattleshipGame.Services.Extensions
{
    public static class SessionExtensions
    {
        public static void SetJsonObject<T>(this ISession session, string key, T value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var bytes = Encoding.UTF8.GetBytes(jsonString);
            session.Set(key, bytes);
        }

        public static T? GetJsonObject<T>(this ISession session, string key)
        {
            session.TryGetValue(key, out var bytes);
            if (bytes == null) return default;
            var jsonString = Encoding.UTF8.GetString(bytes);
            var ds = JsonConvert.DeserializeObject<T>(jsonString);
            return ds;
        }
    }
}
