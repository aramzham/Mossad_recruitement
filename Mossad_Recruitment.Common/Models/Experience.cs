using System;
using System.Text.Json.Serialization;

namespace Mossad_Recruitment.Common.Models
{
    public class Experience
    {
        [JsonPropertyName("technologyId")] public Guid Id { get; set; }
        [JsonPropertyName("yearsOfExperience")] public int YearsOfExperience { get; set; }
    }
}
