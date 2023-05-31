using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Assets.Category;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetCategoryService
    {
        public Task<PagedViewModel<AssetsCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<AssetsCategoryDto>> GetAllAssetsCategories();

        public Task<AssetsCategoryDto> GetAssetCategoryById(Guid id);
        public Task<bool> InsertAssetCategory(CreateAssetCategoryDto createAssetsCategoryDto);
        public Task<bool> DeleteAssetCategory(Guid id);
        public Task<bool> UpdateAssetCategory(UpdateAssetsCategoryDto updateAssetsCategoryDto, Guid Id);

    }
    public class AssetCategoryService : IAssetCategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetCategoryService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AssetsCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsCategories/paging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsCategoryDto>>(body);

            return users;

        }
        public async Task<bool> InsertAssetCategory(CreateAssetCategoryDto createAssetsCategoryDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsCategoryDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AssetsCategories/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAssetCategory(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AssetsCategories/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsCategoryDto> GetAssetCategoryById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsCategories/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsCategoryDto>(body);

            return user;
        }
        public async Task<bool> UpdateAssetCategory(UpdateAssetsCategoryDto updateAssetsCategoryDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsCategoryDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AssetsCategories/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsCategoryDto>> GetAllAssetsCategories()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsCategories");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsCategoryDto>>(body);
            return users;
        }
    }
}

