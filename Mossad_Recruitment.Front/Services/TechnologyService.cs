using Mossad_Recruitment.Common.Models;
using Mossad_Recruitment.Front.Services.Contracts;
using System.Text.Json;

namespace Mossad_Recruitment.Front.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly HttpClient _httpClient;

        public TechnologyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDictionary<Guid, Technology>> GetAll()
        {
            var response = await _httpClient.GetStringAsync("/technologies");
            var technologies = JsonSerializer.Deserialize<IDictionary<Guid, Technology>>(response);

            return technologies;
        }
    }
}
