using Mossad_Recruitment.Common.Dtos;
using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure
{
    public static class _Extensions
    {
        public static CandidateDto ToDto(this Candidate candidate, IDictionary<Guid, Technology> technologyDict)
        {
            return new CandidateDto
            {
                Email = candidate.Email,
                Id = candidate.Id,
                ImageUrl = candidate.ProfilePicture,
                Name = candidate.FullName,
                Experience = candidate.Experience.Select(x => x.ToDto(technologyDict)).ToArray()
            };
        }

        public static ExperienceDto ToDto(this Experience experience, IDictionary<Guid, Technology> technologyDict) 
        {
            return new ExperienceDto
            {
                Technology = technologyDict[experience.Id].Name,
                YearsOfExperience = experience.YearsOfExperience
            };
        }
    }
}
