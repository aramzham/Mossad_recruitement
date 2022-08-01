using Mossad_Recruitment.Common.Dtos;

namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<CandidateDto> Next();
        Task Accept(Guid id);
        Task Reject(Guid id);
        Task<IEnumerable<CandidateDto>> GetAccepted();
    }
}
