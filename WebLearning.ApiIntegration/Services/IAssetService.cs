using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetService
    {
        public Task<PagedViewModel<AssetsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest, Guid[] assetsCategoryId, Guid[] assetsDepartmentId, int[] statusId, string url);
        public Task<IEnumerable<AssetsDto>> GetAllAssets();
        public Task<IEnumerable<AssetsDto>> FilterAssets(Guid catId, Guid subId, Guid depId, int statusId);
        public Task<AssetsDto> GetAssetById(string id);
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
        public async Task<PagedViewModel<AssetsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest, Guid[] assetsCategoryId, Guid[] assetsDepartmentId, int[] statusId,string url)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            string address = $"/api/Assests/paging?pageIndex=" +
    $"{getListPagingRequest.PageIndex}&pageSize={getListPagingRequest.PageSize}&keyword={getListPagingRequest.Keyword}&active={getListPagingRequest.Active}&";
            //if(url != null)
            //{
            //    address += url;
            //}


            for (int i = 0; i < assetsCategoryId.Length; i++)
            {
                address += $"assetsCategoryId={assetsCategoryId[i]}&";

            }
            for (int i = 0; i < assetsDepartmentId.Length; i++)
            {
                address += $"assetsDepartmentId={assetsDepartmentId[i]}&";

            }
            for (int i = 0; i < statusId.Length; i++)
            {
                address += $"assetsStatusId={statusId[i]}&";

            }
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


    }
}

