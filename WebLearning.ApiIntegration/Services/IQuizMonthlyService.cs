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
    public interface IQuizMonthlyService
    {
        public Task<PagedViewModel<QuizMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<QuizMonthlyDto>> GetAllQuiz();
        public Task<IEnumerable<QuizMonthlyDto>> GetOwnQuiz(Guid roleId);
        Task<QuizMonthlyDto> GetName(Guid id);


        public Task<QuizMonthlyDto> GetQuizById(Guid id, string accountName);

        public Task<QuizMonthlyDto> AdminGetQuizById(Guid id);

        public Task<bool> InsertQuiz(CreateQuizMonthlyDto createQuizMonthlyDto);

        public Task<bool> DeleteQuiz(Guid id);

        public Task<bool> UpdateQuiz(UpdateQuizMonthlyDto updateQuizMonthlyDto, Guid id);

        public Task<bool> SubmitAll(Guid id, QuizMonthlyDto submitAnswerDto);
    }
    public class QuizMonthlyService : IQuizMonthlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuizMonthlyService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<QuizMonthlyDto> AdminGetQuizById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizMonthlies/AdminGetQuiz/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var quizMonthly = JsonConvert.DeserializeObject<QuizMonthlyDto>(body);

            return quizMonthly;
        }

        public async Task<bool> DeleteQuiz(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/QuizMonthlies/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<QuizMonthlyDto>> GetAllQuiz()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizMonthlies");

            var body = await response.Content.ReadAsStringAsync();

            var quizMonthlys = JsonConvert.DeserializeObject<IEnumerable<QuizMonthlyDto>>(body);

            return quizMonthlys;
        }

        public async Task<QuizMonthlyDto> GetName(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizMonthlies/GetName?id={id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<QuizMonthlyDto>(body);

            return user;
        }

        public async Task<IEnumerable<QuizMonthlyDto>> GetOwnQuiz(Guid roleId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizMonthlies/OwnQuizMonthly/{roleId}");

            var body = await response.Content.ReadAsStringAsync();

            var quizMonthlys = JsonConvert.DeserializeObject<IEnumerable<QuizMonthlyDto>>(body);

            return quizMonthlys;
        }

        public async Task<PagedViewModel<QuizMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuizMonthlyPaging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<QuizMonthlyDto>>(body);

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

            //        var response = await client.GetAsync($"/QuizMonthlyPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizMonthlyDto>>(body);

            //        return users;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuizMonthlyPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<QuizMonthlyDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuizMonthlyPaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<QuizMonthlyDto>>(body);

            //    return users;
            //}

            //return default;
        }

        public async Task<QuizMonthlyDto> GetQuizById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuizMonthlies/UserGetQuiz/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var quizMonthly = JsonConvert.DeserializeObject<QuizMonthlyDto>(body);

            return quizMonthly;
        }

        public async Task<bool> InsertQuiz(CreateQuizMonthlyDto createQuizMonthlyDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createQuizMonthlyDto.ID = Guid.NewGuid();

            createQuizMonthlyDto.DateCreated = DateTime.Now;

            createQuizMonthlyDto.Alias = Utilities.SEOUrl(createQuizMonthlyDto.Name);

            createQuizMonthlyDto.DescNotify = "Bạn có một bài kiểm tra mới";


            var json = JsonConvert.SerializeObject(createQuizMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuizMonthlies/", httpContent);

            return response.IsSuccessStatusCode;
        }

        public Task<bool> SubmitAll(Guid id, QuizMonthlyDto submitAnswerDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuiz(UpdateQuizMonthlyDto updateQuizMonthlyDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateQuizMonthlyDto.Alias = Utilities.SEOUrl(updateQuizMonthlyDto.Name);

            updateQuizMonthlyDto.DescNotify = "Bạn có một bài kiểm tra mới";

            var json = JsonConvert.SerializeObject(updateQuizMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuizMonthlies/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
