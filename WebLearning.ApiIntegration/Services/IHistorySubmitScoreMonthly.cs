using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IHistorySubmitScoreMonthly
    {
        public Task<HistorySubmitMonthlyDto> GetHistorySubmitMonthlyDtoById(Guid questionId, string accountName);
        Task<bool> CreateHistorySubmitMonthlyDto(Guid questionMonthlyId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto);
        Task<bool> DeleteHistorySubmitMonthlyDto(Guid questionId);
    }
    public class HistorySubmitScoreMonthly : IHistorySubmitScoreMonthly
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HistorySubmitScoreMonthly(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateHistorySubmitMonthlyDto(Guid questionMonthlyId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitMonthlies?questionMonthlyId={questionMonthlyId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteHistorySubmitMonthlyDto(Guid questionId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/HistorySubmitMonthlies/{questionId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<HistorySubmitMonthlyDto> GetHistorySubmitMonthlyDtoById(Guid questionId, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/HistorySubmitMonthlies?questionId={questionId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<HistorySubmitMonthlyDto>(body);

            return user;
        }

    }
}
