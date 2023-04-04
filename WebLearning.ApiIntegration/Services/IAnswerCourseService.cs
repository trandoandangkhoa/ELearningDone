using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAnswerCourseService
    {
        public Task<IEnumerable<AnswerCourseDto>> GetAllAnswer();

        public Task<List<AnswerCourseDto>> GetAnswerById(Guid id, string accountName);
        public Task<bool> InsertAnswer(CreateAnswerCourseDto createAnswerDto);
        public Task<bool> DeleteAnswer(Guid id);

        public Task<bool> DeleteAnswerNonExist(Guid id);

        public Task<bool> UpdateAnswer(UpdateAnswerCourseDto updateAnswerDto, Guid id, string accountName);
        Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto);
    }
    public class AnswerCourseService : IAnswerCourseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerCourseService(IHttpClientFactory httpClientFactory,
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

            var response = await client.DeleteAsync($"/api/AnswerCourses/DeleteNonExist/{id}");

            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<AnswerCourseDto>> GetAllAnswer()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerCourses");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AnswerCourseDto>>(body);

            return users;
        }

        public async Task<List<AnswerCourseDto>> GetAnswerById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AnswerCourses/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<List<AnswerCourseDto>>(body.ToString());

            return user;
        }

        public async Task<bool> InsertAnswer(CreateAnswerCourseDto createAnswerDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createAnswerDto.Id = Guid.NewGuid();

            var json = JsonConvert.SerializeObject(createAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AnswerCourses/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAnswer(UpdateAnswerCourseDto updateAnswerDto, Guid id, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateAnswerDto.AccountName = accountName;

            var json = JsonConvert.SerializeObject(updateAnswerDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AnswerCourses/{id}?accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePoint(Guid questionId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitCourses/{questionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
