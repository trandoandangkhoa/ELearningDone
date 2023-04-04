using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;

namespace WebLearning.ApiIntegration.Services
{
    public interface ICorrectAnswerMonthlyService
    {
        public Task<PagedViewModel<CorrectAnswerMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<CorrectAnswerMonthlyDto>> GetAllCorrectAnswerMonthlys();

        public Task<CorrectAnswerMonthlyDto> GetCorrectAnswerMonthlyById(Guid id);
        public Task<bool> InsertCorrectAnswerMonthly(CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto);
        public Task<bool> DeleteCorrectAnswerMonthly(Guid id);
        public Task<bool> UpdateCorrectAnswerMonthly(UpdateCorrectAnswerMonthlyDto updateCorrectAnswerMonthlyDto, Guid Id);

    }
    public class CorrectAnswerMonthlyService : ICorrectAnswerMonthlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrectAnswerMonthlyService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<CorrectAnswerMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var a = getListPagingRequest.Keyword;

            if (a != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("Keyword", a);
            }
            var ksession = _httpContextAccessor.HttpContext.Session.GetString("Keyword");

            if (string.IsNullOrEmpty(ksession))
            {
                if (a == null)
                {

                    var response = await client.GetAsync($"/paging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerMonthlyDto>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/paging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerMonthlyDto>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/paging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerMonthlyDto>>(body);

                return users;
            }

            return default;

        }
        public async Task<bool> InsertCorrectAnswerMonthly(CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createCorrectAnswerMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/CorrectAnswerMonthlies/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCorrectAnswerMonthly(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/CorrectAnswerMonthlies/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<CorrectAnswerMonthlyDto> GetCorrectAnswerMonthlyById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CorrectAnswerMonthlies/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CorrectAnswerMonthlyDto>(body);

            return user;
        }
        public async Task<bool> UpdateCorrectAnswerMonthly(UpdateCorrectAnswerMonthlyDto updateCorrectAnswerMonthlyDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateCorrectAnswerMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/CorrectAnswerMonthlies/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<CorrectAnswerMonthlyDto>> GetAllCorrectAnswerMonthlys()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CorrectAnswerMonthlies");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<CorrectAnswerMonthlyDto>>(body);
            return users;
        }
    }
}

