namespace Mossad_Recruitment.Api.Infrastructure.Services.Interfaces
{
    public interface ICacheService
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
    }
}
