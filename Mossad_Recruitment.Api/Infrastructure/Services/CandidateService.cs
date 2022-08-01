using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Models;
using System.Text.Json;

namespace Mossad_Recruitment.Api.Infrastructure.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;

        public CandidateService(HttpClient httpClient, ICacheService cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task Accept(Guid id)
        {
            await RemoveFromCache(id);
            
            // add to accepted list
            var examinedCandidates = _cache.Get<List<Guid>>(CacheKeys.Accepted);
            examinedCandidates.Add(id);
            _cache.Set(CacheKeys.Accepted, examinedCandidates);
        }

        public async Task<IEnumerable<Candidate>> GetAccepted()
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();

            var acceptedIds = _cache.Get<IEnumerable<Guid>>(CacheKeys.Accepted);
            return acceptedIds?.Select(x=> candidatesInCache[x]); // may have been a response model with only full name and id, because that's what we'll use accepted candidates page
        }

        public async Task<Candidate> Next()
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            var criterias = _cache.Get<IEnumerable<Criteria>>(CacheKeys.Criterias) ?? new List<Criteria>();
            
            var candidate = default(Candidate);
            var random = new Random();

            // some sort of randomness
            while (candidate is null)
            {
                candidate = candidatesInCache.ElementAt(random.Next(0, candidatesInCache.Count)).Value;

                // look at experience
                foreach (var experience in candidate.experience)
                {
                    if (!criterias.Any(x => x.Technology == experience.technologyId && x.YearsOfExperience <= experience.yearsOfExperience))
                    { 
                        break;
                    }
                }
            }
            
            return candidate;
        }

        public Task Reject(Guid id)
        {
            return RemoveFromCache(id);
        }

        private async Task<IDictionary<Guid, Candidate>> EnsureCandidatesSetInCache()
        {
            var candidates = _cache.Get<IDictionary<Guid, Candidate>>(CacheKeys.Candidates);
            if (candidates is null)
            {
                var response = await _httpClient.GetStringAsync(CacheKeys.Candidates);
                var deserializedResponse = JsonSerializer.Deserialize<IEnumerable<Candidate>>(response, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                candidates = deserializedResponse.ToDictionary(k => k.Id);
                _cache.Set(CacheKeys.Candidates, candidates);
            }

            return candidates;
        }

        private async Task RemoveFromCache(Guid id) 
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            if (!candidatesInCache.ContainsKey(id))
                throw new Exception("no candidate found by specified id");

            // remove from cache so no second chance is provided
            candidatesInCache.Remove(id);
            _cache.Set(CacheKeys.Candidates, candidatesInCache);
        }
    }
}
