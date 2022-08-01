using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Front.Services.Contracts
{
    public interface ICandidatesService
    {
        Task<Candidate> Next();
        Task Accept(Guid id);
        Task Reject(Guid id);
        Task<IEnumerable<Candidate>> GetAcceptedList();
    }
}
