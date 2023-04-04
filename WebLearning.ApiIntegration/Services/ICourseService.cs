using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Course.CourseAdminView;

namespace WebLearning.ApiIntegration.Services
{
    public interface ICourseService
    {
        public Task<PagedViewModel<CourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<IEnumerable<CourseDto>> GetAllCourse();

        public Task<CourseDto> GetCourseById(Guid id);
        public Task<bool> InsertCourse(CreateCourseDto createCourseDto);
        public Task<bool> DeleteCourse(Guid id);
        public Task<bool> UpdateCourse(Guid id, UpdateCourseAdminView updateCourseAdminView);
        Task<CourseDto> GetName(Guid id);

    }
    public class CourseService : ICourseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Courses/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCourse()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Courses");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<CourseDto>>(body);
            return users;
        }

        public async Task<CourseDto> GetCourseById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Courses/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CourseDto>(body);

            return user;
        }

        public async Task<CourseDto> GetName(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Courses/GetName?id={id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CourseDto>(body);

            return user;
        }

        public async Task<PagedViewModel<CourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/coursepaging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<CourseDto>>(body);

            return users;
            //var a = getListPagingRequest.Keyword;

            //if (a != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", a);
            //}
            //var ksession = _httpContextAccessor.HttpContext.Session.GetString("Keyword");

            //if (string.IsNullOrEmpty(ksession))
            //{
            //    if (a == null)
            //    {

            //        var response = await client.GetAsync($"/coursepaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<CourseDto>>(body);

            //        return users;
            //    }
            //    else if (a != null)
            //    {

            //        var response = await client.GetAsync($"/coursepaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<CourseDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/coursepaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<CourseDto>>(body);

            //    return users;
            //}

            //return default;
        }

        public async Task<bool> InsertCourse(CreateCourseDto createCourseDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();


            createCourseDto.Id = Guid.NewGuid();

            createCourseDto.DateCreated = DateTime.Now;

            createCourseDto.Alias = Utilities.SEOUrl(createCourseDto.Name);

            createCourseDto.DescNotify = "Bạn có một khóa học mới";

            if (createCourseDto.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createCourseDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createCourseDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", createCourseDto.Image.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.Name) ? "" : createCourseDto.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.Description) ? "" : createCourseDto.Description.ToString()), "description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.Notify.ToString()) ? "" : createCourseDto.Notify.ToString()), "notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.DescNotify.ToString()) ? "" : createCourseDto.DescNotify.ToString()), "descNotify");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.Active.ToString()) ? "" : createCourseDto.Active.ToString()), "active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.DateCreated.ToString()) ? "" : createCourseDto.DateCreated.ToString()), "dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.Alias) ? "" : createCourseDto.Alias.ToString()), "alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCourseDto.CreatedBy.ToString()) ? "" : createCourseDto.CreatedBy.ToString()), "createdBy");


            var json = JsonConvert.SerializeObject(createCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Courses/", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourse(Guid id, UpdateCourseAdminView updateCourseAdminView)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateCourseAdminView.UpdateCourseDto.DescNotify = "Bạn có một khóa học mới";

            var requestContent = new MultipartFormDataContent();

            if (updateCourseAdminView.UpdateCourseImageDto.ImageFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateCourseAdminView.UpdateCourseImageDto.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateCourseAdminView.UpdateCourseImageDto.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "updateCourseAdminView.UpdateCourseImageDto.ImageFile", updateCourseAdminView.UpdateCourseImageDto.ImageFile.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.Name) ? "" : updateCourseAdminView.UpdateCourseDto.Name.ToString()), "updateCourseAdminView.updateCourseDto.name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.Description) ? "" : updateCourseAdminView.UpdateCourseDto.Description.ToString()), "updateCourseAdminView.updateCourseDto.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.Notify.ToString()) ? "" : updateCourseAdminView.UpdateCourseDto.Notify.ToString()), "updateCourseAdminView.updateCourseDto.notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.DescNotify.ToString()) ? "" : updateCourseAdminView.UpdateCourseDto.DescNotify.ToString()), "updateCourseAdminView.updateCourseDto.descNotify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.Active.ToString()) ? "" : updateCourseAdminView.UpdateCourseDto.Active.ToString()), "updateCourseAdminView.updateCourseDto.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.DateCreated.ToString()) ? "" : updateCourseAdminView.UpdateCourseDto.DateCreated.ToString()), "updateCourseAdminView.updateCourseDto.dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCourseAdminView.UpdateCourseDto.Alias) ? "" : updateCourseAdminView.UpdateCourseDto.Alias.ToString()), "updateCourseAdminView.updateCourseDto.alias");

            var json = JsonConvert.SerializeObject(updateCourseAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Courses/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}