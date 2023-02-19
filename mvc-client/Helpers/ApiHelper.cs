
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace mvc_client.Helpers
{
    public static class ApiHelper
    {
        public static async Task<T> GetApi<T>(this HttpClient client, string api)
        {
            HttpResponseMessage res = await client.GetAsync(api);
            var data = await res.Content.ReadAsStringAsync();

            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var list = JsonSerializer.Deserialize<T>(data, opt);
            return list == null ? default : list;
        }
    }
}