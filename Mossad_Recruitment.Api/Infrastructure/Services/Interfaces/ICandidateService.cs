using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<Candidate> Next();
        Task Accept(Guid id);
        Task Reject(Guid id);
        IEnumerable<Candidate> GetAccepted();
    }
}
