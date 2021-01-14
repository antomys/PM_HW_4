using System;
using System.Collections;
using System.Text.Json.Serialization;

namespace Task_1
{
    internal class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }
        [JsonPropertyName("error")]
        public string Error { get; init; }
        [JsonPropertyName("duration")]
        public string Duration { get; init; }
        [JsonPropertyName("primes")]
        public ArrayList Primes { get; init; }
    }
}
