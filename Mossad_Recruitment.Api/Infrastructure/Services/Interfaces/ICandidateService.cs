using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<IEnumerable<Candidate>> GetAll();
        Task<Candidate> Get(Guid id);
        Task<Candidate> Next(Guid id);
        Task Accept(Guid id);
        Task Reject(Guid id);
        Task<IEnumerable<Candidate>> GetAccepted();
        Task<IEnumerable<Candidate>> FilterByCriteria(IEnumerable<Criteria> criterias);
    }
}
