using System;
using System.Collections;
using System.Text.Json.Serialization;

namespace Task_1
{
    class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("duration")]
        public string Duration { get; set; }
        [JsonPropertyName("primes")]
        public ArrayList Primes { get; set; }
    }
}
