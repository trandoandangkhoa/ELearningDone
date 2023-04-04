using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.OptionMonthly;

namespace WebLearning.ApiIntegration.Services
{
    public interface IOptionMonthlyervice
    {
        public Task<PagedViewModel<OptionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<OptionMonthlyDto>> GetAllOptionMonthly();

        public Task<OptionMonthlyDto> GetOptionMonthlyById(Guid id);
        public Task<bool> InsertOptionMonthly(CreateOptionMonthlyDto createOptionMonthlyDto);
        public Task<bool> DeleteOptionMonthly(Guid id, Guid questionMonthlyId);
        public Task<bool> UpdateOptionMonthly(UpdateOptionMonthlyDto updateOptionMonthlyDto, Guid Id);

    }
    public class OptionMonthlyervice : IOptionMonthlyervice
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OptionMonthlyervice(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<OptionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

                    var users = JsonConvert.DeserializeObject<PagedViewModel<OptionMonthlyDto>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/paging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<OptionMonthlyDto>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/paging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<OptionMonthlyDto>>(body);

                return users;
            }

            return default;

        }
        public async Task<bool> InsertOptionMonthly(CreateOptionMonthlyDto createOptionMonthlyDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(createOptionMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/OptionMonthlies/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteOptionMonthly(Guid id, Guid questionMonthlyId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/OptionMonthlies/{id}?questionMonthlyId={questionMonthlyId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<OptionMonthlyDto> GetOptionMonthlyById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/OptionMonthlies/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<OptionMonthlyDto>(body);

            return user;
        }
        public async Task<bool> UpdateOptionMonthly(UpdateOptionMonthlyDto updateOptionMonthlyDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateOptionMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/OptionMonthlies/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<OptionMonthlyDto>> GetAllOptionMonthly()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/OptionMonthlies");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<OptionMonthlyDto>>(body);
            return users;
        }
    }
}

