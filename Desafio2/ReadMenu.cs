using System;
using System.Collections.Generic;
using System.Linq;

namespace Desafio2
{
    // Classe que implementa a leitura e validação das entradas do usuário
    public static class ReadMenu
    {
       /* 
       * As mensagens a serem impressas na interação com o usuário.
       * Cada mensagem representa uma propriedade da classe Conversão.
       */
        private static readonly string[] messages =
        {
            "Moeda origem:",
            "Moeda destino:",
            "Valor:"
        };

        // As ações a serem feitas para cada entrada.
        private static readonly Action<Conversao, string>[] actions =
        {
            (data, line) => {data.MoedaOrigem = line; },
            (data, line) => {data.MoedaDestino = line; },
            (data, line) => {data.SetValor(line); }
        };

        private static readonly IEnumerable<(string, Action<Conversao, string>, int)> zippedWithIndex =
            messages
            .Zip(actions)
            .Select((a, b) => (a.First, a.Second, b));

        // Retorna falso, se o usuário digitou uma entrada nula para o primeiro campo
        static public bool Read(Conversao data)
        {
            string line;

            // Para cada mensgem, ação e índice em Messages e Actions; índice vai de 0 até 2
            foreach (var (message, action, index) in zippedWithIndex)
            {
                while (true)
                {
                    // Escrever Mensagem
                    Console.WriteLine(message);

                    // Ler entrada
                    line = Console.ReadLine();

                    // Se o campo atual for o primeiro, i.e, "MoedasOrigem" e a entrada for nula, nenhuma outra entrada deve ser lida
                    if (index == 0 && string.IsNullOrEmpty(line))
                        return false;


                    try
                    {
                        // Executar Ação
                        action(data, line);
                        // Se ação for sucedida, ir para próximo campo
                        // Isto é, sair do loop While
                        break;

                    }
                    // Se ação falhar, escrever mensagem de Erro e ler Campo de novo (i.e. outra iteração do loop while)
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return true;
        }

    }
}
