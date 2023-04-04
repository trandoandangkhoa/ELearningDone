using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CourseRole;

namespace WebLearning.ApiIntegration.Services
{
    public interface ICourseRoleService
    {
        public Task<CourseRoleDto> GetCourseById(Guid courseId, string accountName);
        public Task<CourseRoleDto> AdminGetCourseRole(Guid id, Guid roleId);
        public Task<bool> InsertCourseRole(CreateCourseRoleDto createCourseRoleDto);
        public Task<bool> DeleteCourseRole(Guid id, Guid roleId);
        public Task<PagedViewModel<CourseRoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class CourseRoleService : ICourseRoleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseRoleService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CourseRoleDto> AdminGetCourseRole(Guid id, Guid roleId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CourseRoles/{id}?roleId={roleId}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CourseRoleDto>(body);

            return user;
        }

        public async Task<bool> DeleteCourseRole(Guid id, Guid roleId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/CourseRoles/{id}?roleId={roleId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<CourseRoleDto> GetCourseById(Guid courseId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/CourseRoles/UserGetCourseRole/{courseId}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CourseRoleDto>(body);

            return user;
        }

        public async Task<PagedViewModel<CourseRoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
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

                    var response = await client.GetAsync($"/CourseRolePaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CourseRoleDto>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/CourseRolePaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<CourseRoleDto>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/CourseRolePaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<CourseRoleDto>>(body);

                return users;
            }

            return default;
        }

        public async Task<bool> InsertCourseRole(CreateCourseRoleDto createCourseRoleDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createCourseRoleDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/CourseRoles", httpContent);

            return response.IsSuccessStatusCode;
        }

    }
}
