using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerLession;

namespace WebLearning.ApiIntegration.Services
{
    public interface ICorrectAnswerLessionService
    {
        public Task<PagedViewModel<CorrectAnswerLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<CorrectAnswerLessionDto>> GetAllCorrectAnswerLessions();

        public Task<CorrectAnswerLessionDto> GetCorrectAnswerLessionById(Guid id);
        public Task<bool> InsertCorrectAnswerLession(CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto);
        public Task<bool> DeleteCorrectAnswerLession(Guid id);
        public Task<bool> UpdateCorrectAnswerLession(UpdateCorrectAnswerLessionDto updateCorrectAnswerLessionDto, Guid Id);

    }
    public class CorrectAnswerLessionService : ICorrectAnswerLessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrectAnswerLessionService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<CorrectAnswerLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerLessionDto>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/paging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerLessionDto>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/paging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<CorrectAnswerLessionDto>>(body);

                return users;
            }

            return default;

        }
        public async Task<bool> InsertCorrectAnswerLession(CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createCorrectAnswerLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/CorrectAnswerLessions/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCorrectAnswerLession(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/CorrectAnswerLessions/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<CorrectAnswerLessionDto> GetCorrectAnswerLessionById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CorrectAnswerLessions/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CorrectAnswerLessionDto>(body);

            return user;
        }
        public async Task<bool> UpdateCorrectAnswerLession(UpdateCorrectAnswerLessionDto updateCorrectAnswerLessionDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateCorrectAnswerLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/CorrectAnswerLessions/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<CorrectAnswerLessionDto>> GetAllCorrectAnswerLessions()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CorrectAnswerLessions");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<CorrectAnswerLessionDto>>(body);
            return users;
        }
    }
}

