using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.ApiIntegration.Services
{
    public interface IQuizCourseService
    {
        public Task<PagedViewModel<QuizCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<QuizCourseDto>> GetAllQuiz();

        Task<QuizCourseDto> GetName(Guid id);

        public Task<QuizCourseDto> GetQuizById(Guid id, string accountName);

        public Task<QuizCourseDto> AdminGetQuizById(Guid id);

        public Task<bool> InsertQuiz(CreateQuizCourseDto createQuizCourseDto);

        public Task<bool> DeleteQuiz(Guid id);

        public Task<bool> ResetQuizCourse(Guid quizCourseId, string accoutName);

        public Task<bool> UpdateQuiz(UpdateQuizCourseDto updateQuizCourseDto, Guid id);

        public Task<bool> SubmitAll(Guid id, QuizCourseDto submitAnswerDto);
    }
    public class QuizCourseService : IQuizCourseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuizCourseService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<QuizCourseDto> AdminGetQuizById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizCourses/AdminGetQuiz/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var quizCourse = JsonConvert.DeserializeObject<QuizCourseDto>(body);

            return quizCourse;
        }

        public async Task<bool> DeleteQuiz(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/QuizCourses/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<QuizCourseDto>> GetAllQuiz()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizCourses");

            var body = await response.Content.ReadAsStringAsync();

            var quizCourses = JsonConvert.DeserializeObject<IEnumerable<QuizCourseDto>>(body);

            return quizCourses;
        }

        public async Task<QuizCourseDto> GetName(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizCourses/GetName?id={id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<QuizCourseDto>(body);

            return user;
        }

        public async Task<PagedViewModel<QuizCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuizCoursePaging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<QuizCourseDto>>(body);

            return users;
            //var a = getListPagingRequest.Keyword;

            //if (!string.IsNullOrEmpty(a))
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", a);
            //}
            //var ksession = _httpContextAccessor.HttpContext.Session.GetString("Keyword");

            //if (string.IsNullOrEmpty(ksession))
            //{
            //    if (string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuizCoursePaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizCourseDto>>(body);

            //        return users;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuizCoursePaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizCourseDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuizCoursePaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<QuizCourseDto>>(body);

            //    return users;
            //}

            //return default;
        }

        public async Task<QuizCourseDto> GetQuizById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizCourses/UserGetQuiz/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var quizCourse = JsonConvert.DeserializeObject<QuizCourseDto>(body);

            return quizCourse;
        }

        public async Task<bool> InsertQuiz(CreateQuizCourseDto createQuizCourseDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createQuizCourseDto.ID = Guid.NewGuid();

            createQuizCourseDto.DateCreated = DateTime.Now;

            createQuizCourseDto.Alias = Utilities.SEOUrl(createQuizCourseDto.Name);

            createQuizCourseDto.DescNotify = "Bạn có một bài kiểm tra mới";

            var json = JsonConvert.SerializeObject(createQuizCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuizCourses/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetQuizCourse(Guid quizCourseId, string accoutName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/QuizCourses/ResetAnswer/{quizCourseId}?accountName={accoutName}");

            return response.IsSuccessStatusCode;
        }

        public Task<bool> SubmitAll(Guid id, QuizCourseDto submitAnswerDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuiz(UpdateQuizCourseDto updateQuizCourseDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateQuizCourseDto.Alias = Utilities.SEOUrl(updateQuizCourseDto.Name);

            updateQuizCourseDto.DescNotify = "Bạn có một bài kiểm tra mới";

            var json = JsonConvert.SerializeObject(updateQuizCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuizCourses/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
