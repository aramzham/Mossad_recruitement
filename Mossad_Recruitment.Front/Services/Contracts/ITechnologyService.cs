using Mossad_Recruitment.Common.Models;

namespace Mossad_Recruitment.Front.Services.Contracts
{
    public interface ITechnologyService
    {
        Task<IDictionary<Guid, Technology>> GetAll();
    }
}
