using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio2
{
    // Classe que implementa a interação com o usuário
    public class UserInterface
    {
        // mantém os dados relativos à conversão a ser feita
        private Conversao conversao = new();

        // Método Principal
        public async Task Start()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            while (true)
            {
                // Ler Entradas
                if (Read())
                {
                    // Processar dados lidos
                    var processTask = Process(tokenSource);

                    // Imprime no Console enquanto o método "Process()" não acaba
                    var loadTask = Loading(token);

                    try
                    {
                        await processTask;

                        // Esperar método "Load()" terminar
                        loadTask?.Wait();

                        // Se o método "Process()" não gerar erro, chama o método "Write()"
                        Write();

                    }
                    // Verifica se exceção gerada é "esperada" (ArgumentException) ou não.
                    catch (ArgumentException) { }

                    // Se não for "esperada", cancelar a execução das tarefas
                    catch (Exception e)
                    {
                        // Esperar método "Load()" terminar
                        loadTask?.Wait();

                        Console.WriteLine($"Erro Interno. \nAbortando...");
                        Console.Error.WriteLine(e);
                        break;
                    }
                    finally
                    {
                        conversao = new();
                    }
                }
                else
                    break;
            }
        }


        // Lê e valida as entradas do usuário
        // Retorna falso quando o usuário deseja terminar a interação
        private bool Read() => ReadMenu.Read(conversao);



        // Imprime no Console enquanto a tarefa "until" não acaba
        private static async Task Loading(CancellationToken ct)
        {
            Console.Write("Executando");
            while (true)
            {
                try
                {
                    // Imprime no Console a cada 100 milisegundos
                    await Task.Delay(100, ct).ContinueWith(_ => Console.Write('.'), ct);
                }
                catch(Exception)
                {
                    Console.WriteLine();
                    break;
                }
            }
        }
        // Obtem Resultado e Taxa por meio da classe Request
        private async Task Process(CancellationTokenSource cts)
        {
            try
            {
                try
                {
                    Request.Result res = await Request.MakeRequest(conversao.MoedaOrigem, conversao.MoedaDestino, conversao.Valor);
                    conversao.SetResultado(res.Resultado);
                    conversao.SetTaxa(res.Info.Rate);
                }
                finally
                {
                    cts.Cancel();
                }
            }
            // Erro Esperado, i.e, o Resultado e/ou a Taxa passadas não são válidos
            catch (ArgumentException e)
            {
                Console.WriteLine($"Falha ao converter {Conversao.FormatValor(conversao.Valor)} de {conversao.MoedaOrigem} para {conversao.MoedaDestino}\n" + e.Message);
                throw;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Console.WriteLine("Falha ao se comunicar com a API\nCódigo de Resposta HTTP: " + e.StatusCode);
                throw;
            }
            catch (JsonException)
            {
                Console.WriteLine("Falha ao ler a resposta da API\nJson não pode ser desserializado");
                throw;
            }
        }

        // Imprime os dados da Conversão
        private void Write()
        {
            string border = new('-', 50);
            Console.WriteLine($"{border}\n{conversao.MoedaOrigem} {Conversao.FormatValor(conversao.Valor)} => {conversao.MoedaDestino} {Conversao.FormatValor(conversao.Resultado)}\nTaxa: {Conversao.FormatTaxa(conversao.Taxa)}\n{border}");
        }
    }
}
