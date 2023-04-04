using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;

namespace WebLearning.ApiIntegration.Services
{
    public interface IQuestionCourseService
    {
        public Task<PagedViewModel<QuestionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<QuestionCourseDto>> GetAllQuestion();

        public Task<QuestionCourseDto> GetQuestionById(Guid id);

        public Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto);

        public Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto);


        public Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto);

        public Task<bool> DeleteQuestion(Guid id);
    }
    public class QuestionCourseService : IQuestionCourseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionCourseService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> DeleteQuestion(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/QuestionCourses/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<QuestionCourseDto>> GetAllQuestion()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionCourses");

            var body = await response.Content.ReadAsStringAsync();

            var questionCourse = JsonConvert.DeserializeObject<IEnumerable<QuestionCourseDto>>(body);

            return questionCourse;
        }

        public async Task<PagedViewModel<QuestionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuestionCoursePaging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var questionCourse = JsonConvert.DeserializeObject<PagedViewModel<QuestionCourseDto>>(body);

            return questionCourse;
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

            //        var response = await client.GetAsync($"/QuestionCoursePaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionCourse = JsonConvert.DeserializeObject<PagedViewModel<QuestionCourseDto>>(body);

            //        return questionCourse;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuestionCoursePaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionCourse = JsonConvert.DeserializeObject<PagedViewModel<QuestionCourseDto>>(body);

            //        return questionCourse;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuestionCoursePaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var questionCourse = JsonConvert.DeserializeObject<PagedViewModel<QuestionCourseDto>>(body);

            //    return questionCourse;
            //}

            //return default;
        }

        public async Task<QuestionCourseDto> GetQuestionById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionCourses/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var questionCourse = JsonConvert.DeserializeObject<QuestionCourseDto>(body);

            return questionCourse;
        }
        public async Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(createAllConcerningQuestionCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionCourses/CreateConcerningQuestionCourse", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuestionCourses/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionCourses/{id}/CreateNewOptionAndCorrectAnswer", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
