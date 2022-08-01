using System;
using System.Text.Json.Serialization;

namespace Mossad_Recruitment.Common.Models
{
    public class Candidate
    {
        [JsonPropertyName("candidateId")] public Guid Id { get; set; }
        [JsonPropertyName("fullName")] public string FullName { get; set; }
        [JsonPropertyName("firstName")] public string FirstName { get; set; }
        [JsonPropertyName("lastName")] public string LastName { get; set; }
        [JsonPropertyName("gender")] public int Gender { get; set; }
        [JsonPropertyName("profilePicture")] public string ProfilePicture { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("favoriteMusicGenre")] public string FavoriteMusicGenre { get; set; }
        [JsonPropertyName("dad")] public string Dad { get; set; }
        [JsonPropertyName("mom")] public string Mom { get; set; }
        [JsonPropertyName("canSwim")] public bool CanSwim { get; set; }
        [JsonPropertyName("barcode")] public string Barcode { get; set; }
        [JsonPropertyName("experience")] public Experience[] Experience { get; set; }
    }
}
