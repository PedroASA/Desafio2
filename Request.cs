using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Desafio2
{
    public class Request
    {
        private readonly static HttpClient Client;
        // TODO HEADERS
        static Request()
        {
            Client = new();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<Result> MakeRequest(string origem, string destino, string valor)
        {
            await using Stream stream =
        await Client.GetStreamAsync($"https://api.exchangerate.host/convert?from={origem}&to={destino}&amount={valor}");
            var result =
                await JsonSerializer.DeserializeAsync<Result>(stream);
            return result;
        }

        public struct Result
        {
            [field: JsonPropertyName("result")] public string Resultado;
            [field: JsonPropertyName("success")] public bool Sucesso;
            [field: JsonPropertyName("info")] public Information Info;

            public struct Information { [field: JsonPropertyName("rate")] public string Rate; };
        }
    }
}
