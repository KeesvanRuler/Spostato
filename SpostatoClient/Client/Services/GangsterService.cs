using CrimeGameBlazor_Client.Services.IServices;
using CrimeGameBlazor_DataAccess;
using Newtonsoft.Json;
using System.Text;

namespace SpostatoClient.Services
{
    public class GangsterService : IGangsterService
    {
        private readonly HttpClient _httpClient;

        public GangsterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Create(string Name)
        {
            var content = JsonConvert.SerializeObject(Name);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/gangster", bodyContent);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<bool>(responseResult);
                return result;
            }
            return false;

        }
        public async Task<Gangster?> Get()
        {
            var response = await _httpClient.GetAsync("api/gangster");
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Gangster>(responseResult);
                return result;
            }
            return null;
        }
    }
}
