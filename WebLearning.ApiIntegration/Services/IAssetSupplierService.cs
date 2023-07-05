using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Supplier;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetSupplierService
    {
        public Task<PagedViewModel<AssetsSupplierDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<AssetsSupplierDto>> GetAllAssetsSupplier();

        public Task<AssetsSupplierDto> GetAssetSupplierById(string id);
        public Task<bool> InsertAssetSupplier(CreateAssetsSupplierDto createAssetsSupplierDto);
        public Task<bool> DeleteAssetSupplier(string id);
        public Task<bool> UpdateAssetSupplier(UpdateAssetsSupplierDto updateAssetsSupplierDto, string Id);

    }
    public class AssetSupplierService : IAssetSupplierService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetSupplierService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AssetsSupplierDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsSuppliers/paging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsSupplierDto>>(body);

            return users;

        }
        public async Task<bool> InsertAssetSupplier(CreateAssetsSupplierDto createAssetsSupplierDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsSupplierDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AssetsSuppliers/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAssetSupplier(string id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"api/AssetsSuppliers/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsSupplierDto> GetAssetSupplierById(string id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsSuppliers/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsSupplierDto>(body);

            return user;
        }
        public async Task<bool> UpdateAssetSupplier(UpdateAssetsSupplierDto updateAssetsSupplierDto, string id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsSupplierDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AssetsSuppliers/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsSupplierDto>> GetAllAssetsSupplier()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsSuppliers");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsSupplierDto>>(body);
            return users;
        }
    }
}

