using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetService
    {
        public Task<PagedViewModel<AssetsDto>> GetPaging(GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<AssetsDto>> GetAllAssets();
        public Task<IEnumerable<AssetsDto>> GetAllAssetsSelected([FromQuery] string[] id);

        public Task<IEnumerable<AssetsDto>> FilterAssets(Guid catId, Guid subId, Guid depId, int statusId);
        public Task<IEnumerable<AssetsDto>> Export(string url);

        public Task<AssetsDto> GetAssetById(string id);
        public Task<AssetsDto> GetAssetByIdForMoving(string id);

        public Task<bool> InsertAsset(CreateAssetsDto createAssetsDto);
        public Task<bool> DeleteAsset(string id);
        public Task<bool> UpdateAsset(UpdateAssetsDto updateAssetsDto, string Id);
    }
    public class AssetService : IAssetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AssetsDto>> GetPaging(GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.Keyword != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            }

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            string address = $"/api/Assests/paging?keyword={getListPagingRequest.Keyword}&active={getListPagingRequest.Active}&";


            for (int i = 0; i < getListPagingRequest.AssetsCategoryId.Length; i++)
            {
                address += $"assetsCategoryId={getListPagingRequest.AssetsCategoryId[i]}&";

            }
            for (int i = 0; i < getListPagingRequest.AssetsDepartmentLocationId.Length; i++)
            {
                address += $"assetsDepartmentLocationId={getListPagingRequest.AssetsDepartmentLocationId[i]}&";

            }
            for (int i = 0; i < getListPagingRequest.AssetsDepartmentId.Length; i++)
            {
                address += $"assetsDepartmentId={getListPagingRequest.AssetsDepartmentId[i]}&";

            }

            for (int i = 0; i < getListPagingRequest.AssetsStatusId.Length; i++)
            {
                address += $"assetsStatusId={getListPagingRequest.AssetsCategoryId[i]}&";

            }
            address += "pageIndex=" + $"{getListPagingRequest.PageIndex}&pageSize={getListPagingRequest.PageSize}";

            var response = await client.GetAsync(address);

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsDto>>(body);

            return users;

        }
        public async Task<bool> InsertAsset(CreateAssetsDto createAssetsDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Assests/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsset(string id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Assests/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsDto> GetAssetById(string id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Assests/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsDto>(body);

            return user;
        }
        public async Task<bool> UpdateAsset(UpdateAssetsDto updateAssetsDto, string id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Assests/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsDto>> GetAllAssets()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Assests");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsDto>>(body);
            return users;
        }

        public async Task<IEnumerable<AssetsDto>> FilterAssets(Guid catId, Guid subId, Guid depId, int statusId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Assests/filter?catId={catId}&subId={subId}&depId={depId}&statusId={statusId}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsDto>>(body);
            return users;
        }

        public async Task<IEnumerable<AssetsDto>> Export(string url)
        {

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            string address = $"/api/Assests/export" + url;


            var response = await client.GetAsync(address);

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsDto>>(body);

            return users;
        }

        public async Task<IEnumerable<AssetsDto>> GetAllAssetsSelected([FromQuery] string[] id)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            string address = $"/api/Assests/assetsselected?id=" +
    $"{id}&";

            for (int i = 0; i < id.Length; i++)
            {
                address += $"id={id[i]}&";

            }

            var response = await client.GetAsync(address);

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<List<AssetsDto>>(body);

            return users;
        }

        public async Task<AssetsDto> GetAssetByIdForMoving(string id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Assests/moving/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsDto>(body);

            return user;
        }

        
    }
}

