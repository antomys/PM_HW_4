using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Task_2
{
    public static class ActualizingData
    {
        public static async Task UpdateCache(HttpClient client)
        {
            try
            {
                var response = await client.SendAsync(
                    new HttpRequestMessage(HttpMethod.Get, client.BaseAddress));
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Is File cache.json created? {CheckForFileCreated()}");
                var responseBody = await response.Content.ReadAsStringAsync();
                //responseBody = responseBody.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                await File.WriteAllTextAsync("cache.json", responseBody);
                Console.WriteLine("Success of updating Cache");
                //Console.WriteLine(responseBody);

            }
            catch
            {
                Console.WriteLine("Unable to update cache. Using local copy");
            }
        }

        private static bool CheckForFileCreated()
        {
            return File.Exists("cache.json");
        }

        public static List<Cache> InputData()
        {
            return JsonConvert.DeserializeObject<List<Cache>>(File.ReadAllText("cache.json"));
        }
    }
}