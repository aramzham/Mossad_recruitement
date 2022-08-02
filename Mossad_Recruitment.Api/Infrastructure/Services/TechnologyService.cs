using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Models;
using System.Text.Json;

namespace Mossad_Recruitment.Api.Infrastructure.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;

        public TechnologyService(HttpClient httpClient, ICacheService cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<IDictionary<Guid, Technology>> GetAll()
        {
            var technologies = _cache.Get<IDictionary<Guid, Technology>>(CacheKeys.Technologies);
            if (technologies is null)
            {
                var response = await _httpClient.GetStringAsync(CacheKeys.Technologies);
                var deserializedResponse = JsonSerializer.Deserialize<IEnumerable<Technology>>(response);

                technologies = deserializedResponse.ToDictionary(k => k.Id);
                _cache.Set(CacheKeys.Technologies, technologies);
            }

            return technologies;
        }
    }
}
