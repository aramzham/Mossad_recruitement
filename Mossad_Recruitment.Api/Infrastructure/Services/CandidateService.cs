using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Dtos;
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
            var candidatesInCache = await EnsureCandidatesSetInCache();
            if (!candidatesInCache.ContainsKey(id))
                throw new Exception("no candidate found by specified id");

            // add to accepted list
            var examinedCandidates = _cache.Get<List<Candidate>>(CacheKeys.Accepted) ?? new List<Candidate>();
            examinedCandidates.Add(candidatesInCache[id]);
            _cache.Set(CacheKeys.Accepted, examinedCandidates);

            // remove from cache so no second chance is provided
            candidatesInCache.Remove(id);
            _cache.Set(CacheKeys.Candidates, candidatesInCache);
        }

        public async Task<IEnumerable<CandidateDto>> GetAccepted()
        {
            var technologiesInCache = await EnsureTechnologiesSetInCache();

            // may have been a response model with only full name and id, because that's what we'll use accepted candidates page
            var acceptedInCache = _cache.Get<IEnumerable<Candidate>>(CacheKeys.Accepted) ?? new List<Candidate>();
            return acceptedInCache.Select(x => x.ToDto(technologiesInCache));
        }

        public async Task<CandidateDto> Next()
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            var technologiesInCache = await EnsureTechnologiesSetInCache();
            var criterias = _cache.Get<IEnumerable<Criteria>>(CacheKeys.Criterias) ?? new List<Criteria>();

            var candidate = default(Candidate);
            var random = new Random();

            // some sort of randomness
            while (candidate is null)
            {
                candidate = candidatesInCache.ElementAt(random.Next(0, candidatesInCache.Count)).Value;

                // look at experience
                foreach (var experience in candidate.Experience)
                {
                    if (!criterias.Where(x => x.YearsOfExperience > 0).Any(x => x.Technology == experience.Id && x.YearsOfExperience <= experience.YearsOfExperience))
                    {
                        break;
                    }
                }
            }

            return candidate.ToDto(technologiesInCache);
        }

        public async Task Reject(Guid id)
        {
            var candidatesInCache = await EnsureCandidatesSetInCache();
            if (!candidatesInCache.ContainsKey(id))
                throw new Exception("no candidate found by specified id");

            // remove from cache so no second chance is provided
            candidatesInCache.Remove(id);
            _cache.Set(CacheKeys.Candidates, candidatesInCache);
        }

        private async Task<IDictionary<Guid, Candidate>> EnsureCandidatesSetInCache()
        {
            var candidates = _cache.Get<IDictionary<Guid, Candidate>>(CacheKeys.Candidates);
            if (candidates is null)
            {
                var response = await _httpClient.GetStringAsync(CacheKeys.Candidates);
                var deserializedResponse = JsonSerializer.Deserialize<IEnumerable<Candidate>>(response);

                candidates = deserializedResponse.ToDictionary(k => k.Id);
                _cache.Set(CacheKeys.Candidates, candidates);
            }

            return candidates;
        }

        private async Task<IDictionary<Guid, Technology>> EnsureTechnologiesSetInCache()
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
