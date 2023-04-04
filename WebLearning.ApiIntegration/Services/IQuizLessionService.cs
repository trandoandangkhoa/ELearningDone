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
    public interface IQuizLessionService
    {
        public Task<PagedViewModel<QuizlessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<QuizlessionDto>> GetAllQuiz();

        public Task<QuizlessionDto> GetQuizById(Guid quizLessionId, string accountName);

        public Task<QuizlessionDto> AdminGetQuizById(Guid id);
        Task<QuizlessionDto> GetNameLession(Guid id);

        public Task<bool> InsertQuiz(CreateQuizLessionDto createQuizLessionDto);

        public Task<bool> DeleteQuiz(Guid id);

        public Task<bool> UpdateQuiz(UpdateQuizLessionDto updateQuizLessionDto, Guid id);

        public Task<bool> SubmitAll(Guid id, QuizlessionDto submitAnswerDto);

        public Task<int> FindIndexQuiz(Guid id, Guid lessionId);

        public Task<int> CheckPassedQuiz(Guid id, string accountName, Guid lessionId);
    }
    public class QuizLessionService : IQuizLessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuizLessionService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<QuizlessionDto> AdminGetQuizById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions/AdminGetQuiz/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var quizLession = JsonConvert.DeserializeObject<QuizlessionDto>(body);

            return quizLession;
        }

        public async Task<int> CheckPassedQuiz(Guid id, string accountName, Guid lessionId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions/CheckQuizPassed/{id}?accountName={accountName}&lessionId={lessionId}");

            var checkPassed = await response.Content.ReadAsStringAsync();


            return Convert.ToInt32(checkPassed);
        }

        public async Task<bool> DeleteQuiz(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/QuizLessions/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<int> FindIndexQuiz(Guid id, Guid lessionId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions/FindIndexQuiz/{id}?lessionId={lessionId}");

            var findIndex = await response.Content.ReadAsStringAsync();


            return Convert.ToInt32(findIndex);
        }

        public async Task<IEnumerable<QuizlessionDto>> GetAllQuiz()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions");

            var body = await response.Content.ReadAsStringAsync();

            var quizLessions = JsonConvert.DeserializeObject<IEnumerable<QuizlessionDto>>(body);

            return quizLessions;
        }


        public async Task<PagedViewModel<QuizlessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuizLessionPaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<QuizlessionDto>>(body);

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

            //        var response = await client.GetAsync($"/QuizLessionPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizlessionDto>>(body);

            //        return users;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuizLessionPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizlessionDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuizLessionPaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<QuizlessionDto>>(body);

            //    return users;
            //}

            //return default;
        }

        public async Task<QuizlessionDto> GetQuizById(Guid quizLessionId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions/UserGetQuiz/{quizLessionId}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var quizLession = JsonConvert.DeserializeObject<QuizlessionDto>(body);

            return quizLession;
        }

        public async Task<bool> InsertQuiz(CreateQuizLessionDto createQuizLessionDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createQuizLessionDto.ID = Guid.NewGuid();

            createQuizLessionDto.DateCreated = DateTime.Now;

            createQuizLessionDto.Alias = Utilities.SEOUrl(createQuizLessionDto.Name);

            createQuizLessionDto.DescNotify = "Bạn có một bài kiểm tra mới";

            var json = JsonConvert.SerializeObject(createQuizLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuizLessions/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public Task<bool> SubmitAll(Guid id, QuizlessionDto submitAnswerDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuiz(UpdateQuizLessionDto updateQuizLessionDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateQuizLessionDto.Alias = Utilities.SEOUrl(updateQuizLessionDto.Name);

            updateQuizLessionDto.DescNotify = "Bạn có một bài kiểm tra mới";

            var json = JsonConvert.SerializeObject(updateQuizLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuizLessions/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<QuizlessionDto> GetNameLession(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizLessions/GetName?id={id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<QuizlessionDto>(body);

            return user;
        }
    }
}
