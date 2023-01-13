using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio2
{
    public class UserInterface
    {
        private Data _data = new();

        private static readonly string[] messages =
        {
            "Moeda origem:",
            "Moeda destino:",
            "Valor:"
        };

        private static readonly Action<Data, string>[] actions =
        {
            (data, line) => {data.MoedaOrigem = line; },
            (data, line) => {data.MoedaDestino = line; },
            (data, line) => {data.Valor =line; }
        };

        private static readonly IEnumerable<(string, Action<Data, string>, int)> zippedWithIndex = 
            messages
            .Zip(actions)
            .Select((a, b) => (a.First, a.Second, b));

        // EXCEPTION
        public void Start()
        {
            Task writeTask = null;
            while (true)
            {
                writeTask?.Wait();
                if (Read())
                {
                    var processTask = Process();

                    writeTask = Loading(processTask)
                        .ContinueWith(_ => 
                        {
                            if (!processTask.IsFaulted)
                            {
                                Write();
                            }
                            _data = new(); 
                        });
                }
                else
                    break;
            }
            //while (true)
            //{
            //    if (Read())
            //    {
            //        Process();
            //        Write(); 
            //        _data = new();
            //    }
            //    else
            //        break;
            //}
        }

        private bool Read()
        {
            string line;

            foreach (var (message, action, index) in zippedWithIndex)
            {
                while (true)
                {
                    try
                    {

                        Console.WriteLine(message);
                        line = Console.ReadLine();
                        if (index == 0 && string.IsNullOrEmpty(line))
                            return false;
                           
                        action(_data, line);

                        break;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return true;
        }

        // TODO: Better Animation
        private static async Task Loading(Task until)
        {
            Console.Write("Executando");
            while (true)
            { 
                await Task.Delay(100).ContinueWith(_ => Console.Write('.'));
                if (until.IsCompleted)
                {
                    Console.WriteLine();
                    break;
                }
            }
        }
        private async Task Process()
        {
            try
            {
                Request.Result res = await Request.MakeRequest(_data.MoedaOrigem, _data.MoedaDestino, _data.Valor);
                // REDO
                _data.Resultado = res.Resultado?.ToString();
                _data.Taxa = res.Info.Rate?.ToString();
            }
            catch(ArgumentException e)
            {
                // TODO
                Console.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
            return;

        }

        //private void Process()
        //{
        //    try
        //    {
        //        Request.Result res = Request.MakeRequest(_data.MoedaOrigem, _data.MoedaDestino, _data.Valor).Result;
        //        _data.Resultado = res.Resultado?.ToString();
        //        _data.Taxa = res.Info.Rate.ToString();
        //    }
        //    catch(ArgumentException e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Error.WriteLine(e);
        //        throw;
        //    }
        //    return;

        //}

        private void Write()
        {
            string border = new('-', 50);
            Console.WriteLine($"{border}\n{_data.MoedaOrigem} {_data.Valor} => {_data.MoedaDestino} {_data.Resultado}\nTaxa: {_data.Taxa}\n{border}");
        }
    }
}
