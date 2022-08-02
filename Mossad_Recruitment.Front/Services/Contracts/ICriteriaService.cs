using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Front.Services.Contracts
{
    public interface ICriteriaService
    {
        Task<IEnumerable<Criteria>> Get();
        Task Set(IEnumerable<Criteria> criterias);
    }
}
