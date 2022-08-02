using Mossad_Recruitment.Common.Models;
using Mossad_Recruitment.Front.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace Mossad_Recruitment.Front.Services
{
    public class CriteriaService : ICriteriaService
    {
        private readonly HttpClient _httpClient;

        public CriteriaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Criteria>> Get()
        {
            var response = await _httpClient.GetStringAsync("/criterias");
            var criterias = JsonSerializer.Deserialize<IEnumerable<Criteria>>(response);

            return criterias;
        }

        public Task Set(IEnumerable<Criteria> criterias)
        {
            var json = JsonSerializer.Serialize(criterias);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            return _httpClient.PostAsync("/criterias", body);
        }
    }
}
