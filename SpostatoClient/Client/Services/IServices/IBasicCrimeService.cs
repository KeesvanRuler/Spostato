using CrimeGameBlazor_Models;

namespace SpostatoClient.Services.IServices
{
    public interface IBasicCrimeService
    {
        public Task<BasicCrimeResultDTO> ShootingRange();
        public Task<BasicCrimeResultDTO> RobJuwelryStore();
        public Task<BasicCrimeResultDTO> RobGrandma();
    }
}
