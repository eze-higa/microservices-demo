using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace  PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDTO platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}/api/c/Platforms", httpContent);

            if(response.IsSuccessStatusCode){
                Console.WriteLine("Succesful communication");
            }
            else 
            {
                Console.WriteLine("Failed communication");
            }
        }
    }
}