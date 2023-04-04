using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using WebLearning.Contract.Dtos.Login;

namespace WebLearning.ApiIntegration.Services
{
    public interface ILoginService
    {

        Task<string> Authenicate(LoginDto loginDto);

    }
    public class LoginServie : ILoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LoginServie(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> Authenicate(LoginDto loginDto)
        {
            var json = JsonConvert.SerializeObject(loginDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");


            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.PostAsync("api/Logins/Login", httpContent);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }
}
