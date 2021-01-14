using Newtonsoft.Json;
namespace Task_2
{
    public class Cache
    {
        [JsonProperty ("rate")]
        public decimal CurrencyRate { get; set; }
        [JsonProperty ("cc")]
        public string Currency { get; set; }
        [JsonProperty("exchangedate")]
        public string ExchangeDate { get; set; }
        
    }
}