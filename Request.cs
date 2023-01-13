using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// GOOD
namespace Desafio2
{
    public class Request
    {
        private readonly static HttpClient Client;

        static Request()
        {
            Client = new();
            Client.DefaultRequestHeaders.Accept.Clear();
        }

        public static async Task<Result> MakeRequest(string origem, string destino, string valor)
        {
            await using Stream stream =
            await Client.GetStreamAsync($"https://api.exchangerate.host/convert?from={origem}&to={destino}&amount={valor}");

            var result = await JsonSerializer.DeserializeAsync<Result>(stream);

            return result;
        }

        public class Result
        {
            [property: JsonPropertyName("result")] public float? Resultado { get; set; }
            [property: JsonPropertyName("success")] public bool Sucesso { get; set; }
            [property: JsonPropertyName("info")] public Information Info { get; set; }

        }
        public class Information
        {
            [property: JsonPropertyName("rate")]
            public float? Rate { get; set; }
        }
    }
}
