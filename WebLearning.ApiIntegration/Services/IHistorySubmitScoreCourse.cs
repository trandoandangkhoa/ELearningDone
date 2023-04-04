using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.HistorySubmit;

namespace WebLearning.ApiIntegration.Services
{
    public interface IHistorySubmitScoreCourse
    {
        public Task<HistorySubmitCourseDto> GetHistorySubmitCourseDtoById(Guid questionId, string accountName);
        Task<bool> CreateHistorySubmitCourseDto(Guid questionCourseId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto);
        Task<bool> DeleteHistorySubmitCourseDto(Guid questionId);
    }
    public class HistorySubmitScoreCourse : IHistorySubmitScoreCourse
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HistorySubmitScoreCourse(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateHistorySubmitCourseDto(Guid questionCourseId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createHistorySubmitCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/HistorySubmitCourses?questionCourseId={questionCourseId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteHistorySubmitCourseDto(Guid questionId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/HistorySubmitCourses/{questionId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<HistorySubmitCourseDto> GetHistorySubmitCourseDtoById(Guid questionId, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/HistorySubmitCourses?questionId={questionId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<HistorySubmitCourseDto>(body);

            return user;
        }

    }
}
