using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ICriteriaService
    {
        void Set(IEnumerable<Criteria> criterias);
        IEnumerable<Criteria> Get();
    }
}
