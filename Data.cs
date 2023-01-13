using System;
using System.Globalization;

namespace Desafio2
{
    public class Data
    {
        // Backing Fields
        private string _moedaOrigem;
        private string _moedaDestino;
        private float _valor;
        private float _resultado;
        private float _taxa;

        
        public string MoedaOrigem 
        { 
            get => _moedaOrigem;  
         
            set
            {
                if (value == MoedaDestino)
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
                if (value == MoedaOrigem)
                    throw new ArgumentException("Moeda destino e moeda origem devem ser diferentes");

                if (IsValidMoeda(value))
                    _moedaDestino = value.ToUpper();
                else
                    throw new ArgumentException("Moeda informada não possui 3 caracteres");
            }
        }
        // FLOAT?
        public string Valor { 
            get => FormatCurrency(_valor);

            set
            {
                _valor = GetCurrency(value);
            } 
        
        }
        // FLOAT?
        public string Resultado 
        {
            get => FormatCurrency(_resultado);

            set
            {
                _resultado = GetCurrency(value);
            }
        }
        // FLOAT?
        public string Taxa
        {
            get => _taxa.ToString("n6");

            set
            {
                if (float.TryParse(value, out float result))
                {
                    _taxa = result;
                }
                else
                    throw new ArgumentException("Valor informado não possui formato válido.");
            }
        }

        private static bool IsValidMoeda(string val)
        {
            return val.Length == 3;
        }

        private static float GetCurrency(string value)
        {
            if (float.TryParse(value, NumberStyles.Currency, new CultureInfo("pt-BR"), out float result))
            {
                if (result > 0)
                {
                    return result;
                }
                else
                    throw new ArgumentException("Valor deve ser maior que zero");
            }
            throw new ArgumentException("Valor informado não possui formato válido.");
        }

        private static string FormatCurrency(float value) => value.ToString("0.00");
    }
}
