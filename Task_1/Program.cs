using System;
using System.IO;
using System.Text.Json;
using System.Collections;
using System.Diagnostics;

namespace Task_1
{
    class Program
    {
        static void Main()
        {
            var time = DateTime.Now;
            Settings settings = LoadDataFromJson(time);
            var primes = PrimeAlgorithm(settings, time);
        }

        public static Settings LoadDataFromJson(DateTime time)
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
                if (deserializer.PrimesFrom == 0 || deserializer.PrimesTo == 0)
                {
                    throw new Exception();
                }
                return deserializer;
            }
            catch
            {
                var endTime = DateTime.Now;
                TimeSpan duration = (endTime - time);
                var error = "settings.json are missing or corrupted";
                SaveResultInJson(false, error, duration, null);
                return null;
            }
        }

        public static void SaveResultInJson(bool success, string error, 
            TimeSpan duration, ArrayList primes)
        {
            Result result = new Result();
            result.Success = success;
            result.Error = error;
            result.Duration = duration.ToString();
            result.Primes = primes;
            var serialized = JsonSerializer.Serialize(result);
            Console.WriteLine(serialized);
            File.WriteAllText(@"result.json", serialized);
        }
        public static ArrayList PrimeAlgorithm(Settings settings, DateTime time)
        {
            if(settings == null)
            {
                return null;
            }
            var primes = new ArrayList();
            int counter;
            
            for(var number = settings.PrimesFrom; number <= settings.PrimesTo; number++)
            {
                counter = 0;
                for(var i = 2; i <= number / 2; i++)
                {
                    if (number % i == 0)
                    {
                        counter++;
                        break;
                    }
                }
                if (counter == 0 && number != 1)
                    primes.Add(number);
            }
            TimeSpan duration = DateTime.Now.Subtract(time);
            SaveResultInJson(true, null, duration, primes);
            return primes;
        }
    }
}
