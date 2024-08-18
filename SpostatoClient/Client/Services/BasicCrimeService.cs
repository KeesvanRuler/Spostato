using CrimeGameBlazor_Client.Services.IServices;
using CrimeGameBlazor_DataAccess;
using CrimeGameBlazor_Models;
using Newtonsoft.Json;
using System.Text;

namespace SpostatoClient.Services
{
    public class BasicCrimeService : IBasicCrimeService
    {
        private readonly HttpClient _httpClient;

        public BasicCrimeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BasicCrimeResultDTO> ShootingRange()
        {
            var response = await _httpClient.GetAsync("api/BasicCrime/ShootingRange");
            string responseResult = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<BasicCrimeResultDTO>(responseResult);
        }
        public async Task<BasicCrimeResultDTO> RobJuwelryStore()
        {
            var response = await _httpClient.GetAsync("api/BasicCrime/RobJuwelryStore");
            string responseResult = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<BasicCrimeResultDTO>(responseResult);
        }
        public async Task<BasicCrimeResultDTO> RobGrandma()
        {
            var response = await _httpClient.GetAsync("api/BasicCrime/RobGrandma");
            string responseResult = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<BasicCrimeResultDTO>(responseResult);
        }
    }
}
