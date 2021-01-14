using System;
using Newtonsoft.Json;

namespace Task_3
{
    public interface INote
    {

        [JsonProperty("Id")]
        int Id { get; }
        [JsonProperty("Title")]
        string Title { get; }
        [JsonProperty("Text")]
        string Text { get; }
        [JsonProperty("CreatedOn")]
        DateTime? CreatedOn { get; }
    }
}