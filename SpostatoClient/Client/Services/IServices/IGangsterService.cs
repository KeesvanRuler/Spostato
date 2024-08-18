using CrimeGameBlazor_DataAccess;

namespace SpostatoClient.Services.IServices
{
    public interface IGangsterService
    {
        public Task<bool> Create(string Name);
        public Task<Gangster?> Get();
    }
}
