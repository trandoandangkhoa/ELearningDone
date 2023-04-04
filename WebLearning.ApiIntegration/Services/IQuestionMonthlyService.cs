using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;

namespace WebLearning.ApiIntegration.Services
{
    public interface IQuestionMonthlyService
    {
        public Task<PagedViewModel<QuestionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<QuestionMonthlyDto>> GetAllQuestion();

        public Task<QuestionMonthlyDto> GetQuestionById(Guid id);

        public Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto);

        public Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto);


        public Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto);

        public Task<bool> DeleteQuestion(Guid id);
    }
    public class QuestionMonthlyService : IQuestionMonthlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionMonthlyService(IHttpClientFactory httpClientFactory,
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

            var response = await client.DeleteAsync($"/api/QuestionMonthlies/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<QuestionMonthlyDto>> GetAllQuestion()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionMonthlies");

            var body = await response.Content.ReadAsStringAsync();

            var questionMonthly = JsonConvert.DeserializeObject<IEnumerable<QuestionMonthlyDto>>(body);

            return questionMonthly;
        }

        public async Task<PagedViewModel<QuestionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuestionMonthlyPaging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var questionMonthly = JsonConvert.DeserializeObject<PagedViewModel<QuestionMonthlyDto>>(body);

            return questionMonthly;
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

            //        var response = await client.GetAsync($"/QuestionMonthlyPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionMonthly = JsonConvert.DeserializeObject<PagedViewModel<QuestionMonthlyDto>>(body);

            //        return questionMonthly;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuestionMonthlyPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionMonthly = JsonConvert.DeserializeObject<PagedViewModel<QuestionMonthlyDto>>(body);

            //        return questionMonthly;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuestionMonthlyPaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var questionMonthly = JsonConvert.DeserializeObject<PagedViewModel<QuestionMonthlyDto>>(body);

            //    return questionMonthly;
            //}

            //return default;
        }

        public async Task<QuestionMonthlyDto> GetQuestionById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionMonthlies/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var questionMonthly = JsonConvert.DeserializeObject<QuestionMonthlyDto>(body);

            return questionMonthly;
        }
        public async Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(createAllConcerningQuestionMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionMonthlies/CreateConcerningQuestionMonthly", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuestionMonthlies/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionMonthlies/{id}/CreateNewOptionAndCorrectAnswer", httpContent);

            return response.IsSuccessStatusCode;
        }

    }
}
