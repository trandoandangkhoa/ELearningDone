using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAnswerLessionService
    {
        public Task<IEnumerable<AnswerLessionDto>> GetAllAnswer();

        public Task<List<AnswerLessionDto>> GetAnswerById(Guid id, string accountName);
        public Task<bool> InsertAnswer(CreateAnswerLessionDto createAnswerDto);
        public Task<bool> DeleteAnswerNonExist(Guid id);
        public Task<bool> ResetAllAnswer(Guid quizLessionId, string accountName);
        public Task<bool> UpdateAnswer(UpdateAnswerLessionDto updateAnswerDto, Guid id, string accountName);
        Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto);
    }
    public class AnswerLessionService : IAnswerLessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerLessionService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> DeleteAnswerNonExist(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AnswerLessions/DeleteNonExist/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AnswerLessionDto>> GetAllAnswer()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerLessions");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AnswerLessionDto>>(body);

            return users;
        }

        public async Task<List<AnswerLessionDto>> GetAnswerById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerLessions/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<List<AnswerLessionDto>>(body.ToString());

            return user;
        }

        public async Task<bool> InsertAnswer(CreateAnswerLessionDto createAnswerDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createAnswerDto.Id = Guid.NewGuid();

            var json = JsonConvert.SerializeObject(createAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AnswerLessions/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetAllAnswer(Guid quizLessionId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AnswerLessions/ResetAllAnswer/{quizLessionId}?accountName={accountName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAnswer(UpdateAnswerLessionDto updateAnswerDto, Guid id, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateAnswerDto.AccountName = accountName;

            var json = JsonConvert.SerializeObject(updateAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AnswerLessions/{id}?accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitLessions/{questionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
