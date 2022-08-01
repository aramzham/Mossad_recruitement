using Mossad_Recruitment.Common.Dtos;
using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Front.Services.Contracts
{
    public interface ICandidatesService
    {
        Task<CandidateDto> Next();
        Task Accept(Guid id);
        Task Reject(Guid id);
        Task<IEnumerable<CandidateDto>> GetAcceptedList();
    }
}
