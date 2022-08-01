using System;
using System.Text.Json.Serialization;

namespace Mossad_Recruitment.Common.Models
{
    public class Technology
    {
        [JsonPropertyName("guid")] public Guid Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}
