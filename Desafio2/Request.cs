using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace Desafio2
{
    // Classe responsável por fazer as requisições a API
    public class Request
    {
        private readonly static HttpClient Client;
        private static readonly string Uri = "https://api.exchangerate.host/convert?";

        private readonly static string[] Params = { "from", "to", "amount", "places" };

        // Construtor de Classe
        static Request()
        {
            Client = new();
            Client.DefaultRequestHeaders.Accept.Clear();
        }

        public static async Task<Result> MakeRequest(string origem, string destino, float valor, uint casasDecimais)
        {
            await using Stream stream =
            await Client.GetStreamAsync($"{Uri}{Params[0]}={origem}&{Params[1]}={destino}&{Params[2]}={valor}&{Params[3]}={casasDecimais}");

            var result = await JsonSerializer.DeserializeAsync<Result>(stream);

            return result;
        }

        // Somente os campos relevantes
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
