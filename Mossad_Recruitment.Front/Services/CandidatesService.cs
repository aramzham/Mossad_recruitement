using Mossad_Recruitment.Common.Dtos;
using Mossad_Recruitment.Common.Models;
using Mossad_Recruitment.Front.Services.Contracts;
using System.Text.Json;

namespace Mossad_Recruitment.Front.Services
{
    public class CandidatesService : ICandidatesService
    {
        private readonly HttpClient _httpClient;

        public CandidatesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task Accept(Guid id)
        {
            return _httpClient.PostAsync($"/candidate/accept/{id}", null);
        }

        public async Task<IEnumerable<CandidateDto>> GetAcceptedList()
        {
            var response = await _httpClient.GetStringAsync("candidate/accepted");
            var accepted = JsonSerializer.Deserialize<IEnumerable<CandidateDto>>(response, new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            });

            return accepted;
        }

        public async Task<CandidateDto> Next()
        {
            var response = await _httpClient.GetStringAsync("candidate/next");
            var candidate = JsonSerializer.Deserialize<CandidateDto>(response, new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            });

            return candidate;
        }

        public Task Reject(Guid id)
        {
            return _httpClient.PostAsync($"/candidate/reject/{id}", null);
        }
    }
}
