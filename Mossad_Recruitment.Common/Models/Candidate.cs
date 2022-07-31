using System.Text.Json.Serialization;

namespace Mossad_Recruitment.Common.Models
{
    public class Candidate
    {
        [JsonPropertyName("candidateId")]
        public Guid Id { get; set; }
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int gender { get; set; }
        public string profilePicture { get; set; }
        public string email { get; set; }
        public string favoriteMusicGenre { get; set; }
        public string dad { get; set; }
        public string mom { get; set; }
        public bool canSwim { get; set; }
        public string barcode { get; set; }
        public Experience[] experience { get; set; }
    }

    public class Experience
    {
        public Guid technologyId { get; set; }
        public int yearsOfExperience { get; set; }
    }
}
