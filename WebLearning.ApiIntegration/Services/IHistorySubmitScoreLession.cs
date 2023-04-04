using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IHistorySubmitScoreLession
    {
        public Task<HistorySubmitLessionDto> GetHistorySubmitLessionDtoById(Guid questionId, string accountName);
        Task<bool> CreateHistorySubmitLessionDto(Guid questionLessionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto);
        Task<bool> UpdateHistorySubmitLessionDto(Guid questionLessionId, Guid quizLessionId, string accountName, UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto);
        Task<bool> DeleteHistorySubmitLessionDto(Guid questionId);
        Task<bool> ResetHistorySubmitLessionDto(Guid quizLessionId, string accountName);

    }
    public class HistorySubmitScoreLession : IHistorySubmitScoreLession
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HistorySubmitScoreLession(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateHistorySubmitLessionDto(Guid questionLessionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitLessions?questionLessionId={questionLessionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteHistorySubmitLessionDto(Guid questionId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/HistorySubmitLessions/{questionId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<HistorySubmitLessionDto> GetHistorySubmitLessionDtoById(Guid questionId, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/HistorySubmitLessions?questionId={questionId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<HistorySubmitLessionDto>(body);

            return user;
        }

        public async Task<bool> ResetHistorySubmitLessionDto(Guid quizLessionId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/HistorySubmitLessions/ResetAllHistory/{quizLessionId}?accountName={accountName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateHistorySubmitLessionDto(Guid questionLessionId, Guid quizLessionId, string accountName, UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateHistorySubmitLessionDto.AccountName = accountName;

            updateHistorySubmitLessionDto.DateCompoleted = DateTime.Now;

            updateHistorySubmitLessionDto.Submit = true;

            var json = JsonConvert.SerializeObject(updateHistorySubmitLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"/api/HistorySubmitLessions?questionLessionId={questionLessionId}&quizLessionId={quizLessionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
