using System;
using System.Globalization;

namespace Desafio2
{
    public class Conversao
    {
        public const uint CasasDecimais = 2;

        // Backing Fields
        private string _moedaOrigem;
        private string _moedaDestino;

        public string MoedaOrigem 
        { 
            get => _moedaOrigem;  
         
            set
            {
                if (value.ToUpper() == MoedaDestino)
                    throw new ArgumentException("Moeda destino e moeda origem devem ser diferentes");

                if (IsValidMoeda(value))
                    _moedaOrigem = value.ToUpper();
                else
                    throw new ArgumentException("Moeda informada não possui 3 caracteres");
            }
        }

        public string MoedaDestino
        {
            get => _moedaDestino;

            set
            {
                if (value.ToUpper() == MoedaOrigem)
                    throw new ArgumentException("Moeda destino e moeda origem devem ser diferentes");

                if (IsValidMoeda(value))
                    _moedaDestino = value.ToUpper();
                else
                    throw new ArgumentException("Moeda informada não possui 3 caracteres");
            }
        }
        
        public float Valor { get; private set; }

        public float Resultado { get; private set; }

        public float Taxa { get; private set; }
        private static bool IsValidMoeda(string val)
        {
            return val.Length == 3;
        }

        // Método a ser chamado para definir o campo valor
        public void SetValor(string value)
        {
            if (value.Contains('.'))
                throw new ArgumentException("Valor não deve conter ponto. Use vírgula!");

            if (float.TryParse(value, NumberStyles.Currency, new CultureInfo("pt-BR"), out float result))
            {
                // API não responde valores desse tamanho
                if(result >= 1000000000)
                    throw new ArgumentException("Valor informado é muito grande");

                if (result > 0)
                {
                    Valor = result;
                }
                else
                    throw new ArgumentException("Valor deve ser maior que zero");
            }
            else
                throw new ArgumentException("Valor informado não possui formato válido.");
        }

        public void SetResultado(float? value)
        {
            Resultado = value ?? throw new ArgumentException("Resultado da operação é nulo");
        }

        public void SetTaxa(float? value)
        {
            Taxa = value ?? throw new ArgumentException("Taxa da operação é nula");
        }

        // Método chamado para formatar o campo valor e o campo Resultado
        public static string FormatValor(float value) => value.ToString("0.00");

        public static string FormatTaxa(float value) => value.ToString("n6");
    }
}
