using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio2
{
    public class UserInterface
    {
        // Input Validation object
        private readonly Data _data;

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

        public void Menu()
        {
            while(true)
            {
                if (Read())
                {
                    Process();
                    Write();
                }
                else
                    break;
            }
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

        private async void Process()
        {
            Request.Result res = await Request.MakeRequest(_data.MoedaOrigem, _data.MoedaDestino, _data.Valor);

            if (res.Sucesso)
            {
                _data.Resultado = res.Resultado;
                _data.Taxa = res.Info.Rate;
            }
            else
                throw new Exception("Erro ao converter as moedas");
              
        }

        private void Write()
        {
            Console.WriteLine($"{_data.MoedaOrigem} {_data.Valor} => {_data.MoedaDestino} {_data.Resultado}\n Taxa: {_data.Taxa}");
        }
    }
}
