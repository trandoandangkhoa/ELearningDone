using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.Assets.Chart;

namespace WebLearning.ApiIntegration.Services
{
    public interface IIChartService
    {
        Task<IEnumerable<TotalAsset>> TotalAssetsCategory();
        Task<IEnumerable<TotalAsset>> TotalAssetsStatus();
        Task<IEnumerable<TotalAsset>> TotalAssetsAvailable();

    }
    public class IChartService : IIChartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IChartService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsAvailable()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Charts/donutchart");

            var body = await response.Content.ReadAsStringAsync();

            var pc = JsonConvert.DeserializeObject<IEnumerable<TotalAsset>>(body);
            return pc;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsCategory()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Charts/barchart");

            var body = await response.Content.ReadAsStringAsync();

            var bc = JsonConvert.DeserializeObject<IEnumerable<TotalAsset>>(body);
            return bc;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsStatus()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Charts/piechart");

            var body = await response.Content.ReadAsStringAsync();

            var pc = JsonConvert.DeserializeObject<IEnumerable<TotalAsset>>(body);
            return pc;
        }
    }
}
