using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;

namespace WebLearning.ApiIntegration.Services
{
    public interface IQuestionLessionService
    {
        public Task<PagedViewModel<QuestionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<QuestionLessionDto>> GetAllQuestion();

        public Task<QuestionLessionDto> GetQuestionById(Guid id);

        public Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto);

        public Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto);


        public Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto);


        public Task<bool> DeleteQuestion(Guid id);
    }
    public class QuestionLessionService : IQuestionLessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionLessionService(IHttpClientFactory httpClientFactory,
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

            var response = await client.DeleteAsync($"/api/QuestionLessions/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<QuestionLessionDto>> GetAllQuestion()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionLessions");

            var body = await response.Content.ReadAsStringAsync();

            var questionLession = JsonConvert.DeserializeObject<IEnumerable<QuestionLessionDto>>(body);

            return questionLession;
        }

        public async Task<PagedViewModel<QuestionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/QuestionLessionPaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var questionLession = JsonConvert.DeserializeObject<PagedViewModel<QuestionLessionDto>>(body);

            return questionLession;
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

            //        var response = await client.GetAsync($"/QuestionLessionPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionLession = JsonConvert.DeserializeObject<PagedViewModel<QuestionLessionDto>>(body);

            //        return questionLession;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/QuestionLessionPaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var questionLession = JsonConvert.DeserializeObject<PagedViewModel<QuestionLessionDto>>(body);

            //        return questionLession;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/QuestionLessionPaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var questionLession = JsonConvert.DeserializeObject<PagedViewModel<QuestionLessionDto>>(body);

            //    return questionLession;
            //}

            //return default;
        }

        public async Task<QuestionLessionDto> GetQuestionById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/QuestionLessions/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var questionLession = JsonConvert.DeserializeObject<QuestionLessionDto>(body);

            return questionLession;
        }

        public async Task<bool> InsertConcerningQuestion(CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(createAllConcerningQuestionLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionLessions/CreateConcerningQuestionLession", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateConcerningQuestion(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionLesstionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/QuestionLessions/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InsertNewOptionAndCorrectAnswer(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateAllConcerningQuestionLesstionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/QuestionLessions/{id}/CreateNewOptionAndCorrectAnswer", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
