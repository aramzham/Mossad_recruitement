using System;

namespace Mossad_Recruitment.Common.Dtos
{
    public class CandidateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public ExperienceDto[] Experience { get; set; }
    }
}
