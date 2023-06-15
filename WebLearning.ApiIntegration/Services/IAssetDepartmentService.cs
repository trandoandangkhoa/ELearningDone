using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Assets.Department;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetDepartmentService
    {
        public Task<PagedViewModel<AssetsDepartmentDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<AssetsDepartmentDto>> GetAllAssetsDepartment();

        public Task<AssetsDepartmentDto> GetAssetDepartmentById(Guid id);
        public Task<bool> InsertAssetDepartment(CreateAssetsDepartmentDto createAssetsDepartmentDto);
        public Task<bool> DeleteAssetDepartment(Guid id);
        public Task<bool> UpdateAssetDepartment(UpdateAssetsDepartmentDto updateAssetsDepartmentDto, Guid Id);

    }
    public class AssetDepartmentService : IAssetDepartmentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetDepartmentService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AssetsDepartmentDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsDepartments/paging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsDepartmentDto>>(body);

            return users;

        }
        public async Task<bool> InsertAssetDepartment(CreateAssetsDepartmentDto createAssetsDepartmentDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsDepartmentDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AssetsDepartments/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAssetDepartment(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AssetsDepartments/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsDepartmentDto> GetAssetDepartmentById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsDepartments/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsDepartmentDto>(body);

            return user;
        }
        public async Task<bool> UpdateAssetDepartment(UpdateAssetsDepartmentDto updateAssetsDepartmentDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsDepartmentDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AssetsDepartments/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsDepartmentDto>> GetAllAssetsDepartment()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsDepartments");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsDepartmentDto>>(body);
            return users;
        }
    }
}

