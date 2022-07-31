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

        public Task Accept(Guid id)
        {
            return Examine(id, CacheKeys.Accepted);
        }

        public async Task<IEnumerable<Candidate>> FilterByCriteria(IEnumerable<Criteria> criterias)
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();

            var filteredCandidates = new List<Candidate>();
            foreach (var candidate in candidatesInCache.Values) 
            {
                foreach (var experience in candidate.experience) 
                {
                    if (criterias.Any(x => x.Technology == experience.technologyId && x.YearsOfExperience <= experience.yearsOfExperience))
                    { 
                        filteredCandidates.Add(candidate);
                        break;
                    }
                }
            }

            return filteredCandidates;
        }

        public async Task<Candidate> Get(Guid id)
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            if(candidatesInCache.ContainsKey(id))
                return candidatesInCache[id];

            throw new Exception("no candidate found by specified id");
        }

        public async Task<IEnumerable<Candidate>> GetAccepted()
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();

            var acceptedIds = _cache.Get<IEnumerable<Guid>>(CacheKeys.Accepted);
            return acceptedIds?.Select(x=> candidatesInCache[x]); // may have been a response model with only full name and id, because that's what we'll use accepted candidates page
        }

        public async Task<IEnumerable<Candidate>> GetAll()
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            return candidatesInCache.Values;
        }

        public async Task<Candidate> Next(Guid id)
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            var accepted = _cache.Get<IEnumerable<Guid>>(CacheKeys.Accepted);
            var rejected = _cache.Get<IEnumerable<Guid>>(CacheKeys.Rejected);

            return candidatesInCache.FirstOrDefault(x => x.Key != id && !accepted.Contains(x.Key) && !rejected.Contains(x.Key)).Value;
        }

        public Task Reject(Guid id)
        {
            return Examine(id, CacheKeys.Rejected);
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

        private async Task Examine(Guid id, string cacheKey) 
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            if (!candidatesInCache.ContainsKey(id))
                throw new Exception("no candidate found by specified id");

            // remove from cache so no second chance is provided
            candidatesInCache.Remove(id);
            _cache.Set(CacheKeys.Candidates, candidatesInCache);

            // add to accepted list
            var examinedCandidates = _cache.Get<List<Guid>>(cacheKey);
            examinedCandidates.Add(id);
            _cache.Set(cacheKey, examinedCandidates);
        }
    }
}
