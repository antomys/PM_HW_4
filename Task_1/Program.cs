using System;
using System.IO;
using System.Text.Json;
using System.Collections;
using System.Diagnostics;

namespace Task_1
{
    internal static class Program
    {
        private static void Main()
        {
            var time = DateTime.Now;
            var settings = LoadDataFromJson(time);
            PrimeAlgorithm(settings, time);
        }

        private static Settings LoadDataFromJson(DateTime time)
        {
            try
            {
                var jsonString = File.ReadAllText(@"settings.json");
                var option = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var deserializer = JsonSerializer.Deserialize<Settings>(jsonString, option);
                if (deserializer != null && (deserializer.PrimesFrom == 0 || deserializer.PrimesTo == 0))
                {
                    throw new Exception();
                }
                return deserializer;
            }
            catch
            {
                var endTime = DateTime.Now;
                var duration = (endTime - time);
                const string error = "settings.json are missing or corrupted";
                SaveResultInJson(false, error, duration, null);
                return null;
            }
        }

        private static void SaveResultInJson(bool success, string error, 
            TimeSpan duration, ArrayList primes)
        {
            var result = new Result
            {
                Success = success, Error = error, Duration = duration.ToString(), Primes = primes
            };
            var serialized = JsonSerializer.Serialize(result);
            //Console.WriteLine(serialized);
            File.WriteAllText(@"result.json", serialized);
        }

        private static void PrimeAlgorithm(Settings settings, DateTime time)
        {
            var duration = DateTime.Now.Subtract(time);
            if(settings == null)
            {
                return;
            }
            var primes = new ArrayList();

            if (settings != null)
                for (var number = settings.PrimesFrom; number <= settings.PrimesTo; number++)
                {
                    var counter = 0;
                    for (var i = 2; i <= number / 2; i++)
                    {
                        if (number % i != 0) continue;
                        counter++;
                        break;
                    }

                    if (counter == 0 && number != 1)
                        primes.Add(number);
                }

            duration = DateTime.Now.Subtract(time);
            SaveResultInJson(true, null, duration, primes);
            //return primes;
        }
    }
}
