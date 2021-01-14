using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task_2
{
    internal static class Program
    {
        private static List<Cache> _cachedValues;
        private static async Task Main()
        {
            Console.WriteLine("Currency Converter, Volokhovych");
            var requestHttpClient = new HttpClient
            {
                BaseAddress = new Uri(@"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json")
            };
            await ActualizingData.UpdateCache(requestHttpClient);
            _cachedValues = ActualizingData.InputData();
            Menu();
        }
        private static void Menu()
        {
            Console.Write("Enter input Currency (3 symbols): ");
            var inputCurrency = InputCurrency();
            Console.Write("Enter output Currency (3 symbols): ");
            var outputCurrency = InputCurrency();
            var amount= InputAmount();
            Calculate(inputCurrency,outputCurrency,amount);

        }
        private static bool CheckInput(string compare)
        {
            return string.IsNullOrEmpty(compare) || compare.Length != 3;
        }
        private static decimal InputAmount()
        {
            string input;
            decimal amount;
            do
            {
                Console.Write("Enter amount: ");
                input = Console.ReadLine();
            } while (!decimal.TryParse(input, out amount) || amount <= 0);
            return amount;
        }
        
        private static string InputCurrency()
        {
            string inputString;
            do
            {
                inputString = Console.ReadLine()?.ToUpper();
                if(CheckInput(inputString))
                    Console.Write("Please try again.\nInput: ");
            } while (CheckInput(inputString));
            return inputString;
        }

        private static void Calculate(string inputCurrency, string outputCurrency, decimal amount)
        {
            if (!_cachedValues.Any(currencies => currencies.Currency.Equals(inputCurrency)) ||
                !_cachedValues.Any(currencies => currencies.Currency.Equals(outputCurrency)))
            {
                Console.WriteLine($"Error! Pair {inputCurrency} and {outputCurrency} is not found. Exiting.");
                Environment.Exit(1);
            }
            var inputCurrencyCache = (from t in _cachedValues
                where t.Currency.Equals(inputCurrency)
                select t).ToArray();
            var outputCurrencyCache = (from t in _cachedValues
                where t.Currency.Equals(outputCurrency)
                select t).ToArray();
            var hryvniaTemporary = amount * inputCurrencyCache[0].CurrencyRate;
            var conversionResult = hryvniaTemporary / outputCurrencyCache[0].CurrencyRate;
            Console.WriteLine($"{amount} {inputCurrency} x {Math.Abs(inputCurrencyCache[0].CurrencyRate/outputCurrencyCache[0].CurrencyRate)}" +
                              $" {outputCurrency}" +
                              $" = {conversionResult} {outputCurrency} (by {inputCurrencyCache[0].ExchangeDate})");
        }
    }
}