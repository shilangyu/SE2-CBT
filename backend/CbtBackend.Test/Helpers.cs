using System.Text.Json;
using System.Text;

namespace CbtBackend.Test;


public static class Helpers {
    private static readonly JsonSerializerOptions JsonSerdeOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static HttpContent JsonBody<T>(T body) where T : notnull {
        return new StringContent(JsonSerializer.Serialize(body, options: JsonSerdeOptions), Encoding.UTF8, "application/json");
    }

    public static async Task<T> ReadAsJson<T>(this HttpResponseMessage self) where T : notnull {
        var body = await self.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(body, options: JsonSerdeOptions)!;
    }

    public static string ReplaceParam(this string self, string name, object value) {
        return self.Replace($"{{{name}}}", value.ToString());
    }

    public static string TestEmail() => $"user_{Guid.NewGuid()}@email.com";
}
