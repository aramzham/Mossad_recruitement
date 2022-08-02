using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ITechnologyService
    {
        Task<IDictionary<Guid, Technology>> GetAll();
    }
}
