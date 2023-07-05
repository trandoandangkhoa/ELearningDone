using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Repair;

namespace WebLearning.ApiIntegration.Services
{
    public interface IAssetRepairedService
    {
        public Task<PagedViewModel<AssetsRepairedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<AssetsRepairedDto>> GetAllAssetsRepaired();

        public Task<AssetsRepairedDto> GetAssetRepairedById(Guid id);
        public Task<bool> InsertAssetRepaired(CreateAssetsRepairedDto createAssetsRepairedDto);
        public Task<bool> DeleteAssetRepaired(Guid id);
        public Task<bool> UpdateAssetRepaired(UpdateAssetsRepairedDto updateAssetsRepairedDto, Guid Id);

    }
    public class AssetRepairedService : IAssetRepairedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetRepairedService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AssetsRepairedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //if(getListPagingRequest.Keyword != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", getListPagingRequest.Keyword);

            //}

            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsRepaired/paging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AssetsRepairedDto>>(body);

            return users;

        }
        public async Task<bool> InsertAssetRepaired(CreateAssetsRepairedDto createAssetsRepairedDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAssetsRepairedDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/AssetsRepaired/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAssetRepaired(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/AssetsRepaired/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AssetsRepairedDto> GetAssetRepairedById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsRepaired/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AssetsRepairedDto>(body);

            return user;
        }
        public async Task<bool> UpdateAssetRepaired(UpdateAssetsRepairedDto updateAssetsRepairedDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAssetsRepairedDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/AssetsRepaired/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AssetsRepairedDto>> GetAllAssetsRepaired()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/AssetsRepaired");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AssetsRepairedDto>>(body);
            return users;
        }
    }
}

