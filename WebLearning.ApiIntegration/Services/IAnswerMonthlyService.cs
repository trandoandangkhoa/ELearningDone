using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAnswerMonthlyService
    {
        public Task<IEnumerable<AnswerMonthlyDto>> GetAllAnswer();

        public Task<List<AnswerMonthlyDto>> GetAnswerById(Guid id, string accountName);
        public Task<bool> InsertAnswer(CreateAnswerMonthlyDto createAnswerDto);
        public Task<bool> DeleteAnswer(Guid id);

        public Task<bool> DeleteAnswerNonExist(Guid id);

        public Task<bool> UpdateAnswer(UpdateAnswerMonthlyDto updateAnswerDto, Guid id, string accountName);
        Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto);
    }
    public class AnswerMonthlyService : IAnswerMonthlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerMonthlyService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public Task<bool> DeleteAnswer(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAnswerNonExist(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AnswerMonthlies/DeleteNonExist/{id}");

            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<AnswerMonthlyDto>> GetAllAnswer()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerMonthlies");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AnswerMonthlyDto>>(body);

            return users;
        }

        public async Task<List<AnswerMonthlyDto>> GetAnswerById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerMonthlies/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<List<AnswerMonthlyDto>>(body.ToString());

            return user;
        }

        public async Task<bool> InsertAnswer(CreateAnswerMonthlyDto createAnswerDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createAnswerDto.Id = Guid.NewGuid();

            var json = JsonConvert.SerializeObject(createAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AnswerMonthlies/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAnswer(UpdateAnswerMonthlyDto updateAnswerDto, Guid id, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateAnswerDto.AccountName = accountName;

            var json = JsonConvert.SerializeObject(updateAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AnswerMonthlies/{id}?accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitMonthlies/{questionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
