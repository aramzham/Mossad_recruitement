using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Dtos;
using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure.Services
{
    public class CriteriaService : ICriteriaService
    {
        private readonly ICacheService _cache;

        public CriteriaService(ICacheService cache)
        {
            _cache = cache;
        }

        public IEnumerable<Criteria> Get()
        {
            return _cache.Get<IEnumerable<Criteria>>(CacheKeys.Criterias) ?? new List<Criteria>();
        }

        public void Set(IEnumerable<Criteria> criterias)
        {
            _cache.Set(CacheKeys.Criterias, criterias);
        }
    }
}
