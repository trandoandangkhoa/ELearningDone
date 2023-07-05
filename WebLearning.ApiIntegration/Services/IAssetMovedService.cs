using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetMovedService
    {
        public Task<IEnumerable<AssetsMovedDto>> GetAllAssetMoveds();
        public Task<PagedViewModel<AssetsMovedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<AssetsMovedDto> GetAssetMovedById(Guid id);
        public Task<bool> InsertAssetMoved(CreateAssetsMovedDto createAssetsMovedDto);
        public Task<bool> DeleteAssetMoved(Guid id);
        public Task<bool> UpdateAssetMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid Id);

    }
    public class AssetMoveService : IAssetMovedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetMoveService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> InsertAssetMoved(CreateAssetsMovedDto createAssetsMovedDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsMovedDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AssetsMoved/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAssetMoved(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AssetsMoved/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsMovedDto> GetAssetMovedById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsMoved/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsMovedDto>(body);

            return user;
        }
        public async Task<bool> UpdateAssetMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsMovedDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AssetsMoved/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsMovedDto>> GetAllAssetMoveds()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsMoved");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsMovedDto>>(body);
            return users;
        }

        public async Task<PagedViewModel<AssetsMovedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            string address = $"/api/AssetsMoved/paging?pageIndex=" +
    $"{getListPagingRequest.PageIndex}&pageSize={getListPagingRequest.PageSize}&keyword={getListPagingRequest.Keyword}";

            
            var response = await client.GetAsync(address);

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsMovedDto>>(body);

            return users;
        }
    }
}

