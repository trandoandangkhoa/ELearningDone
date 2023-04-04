using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Certificate;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.ApiIntegration.Services
{
    public interface IReportScoreCourseService
    {
        public Task<PagedViewModel<ReportScoreCourseExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<ReportScoreCourseDto>> GetAllReportScores();

        public Task<IEnumerable<ReportScoreCourseDto>> UserGetAllReportScores(string accountName);

        public Task<ReportScoreCourseDto> GetReportScoreById(Guid id);
        public Task<CertificateCourse> GetCertificate(Guid quizCourseId, string accountName);

        public Task<bool> InsertReportScore(Guid quizCourseId, string accountName, CreateReportScoreCourseDto createReportScoreCourseDto);

        public Task<bool> DeleteReportScore(Guid id);
        Task<IEnumerable<ReportScoreCourseExport>> ExportReportScoreCourseDtos(string fromDate, string toDate, bool passed);

        public Task<ReportScoreCourseDto> CheckExist(Guid quizCourseId, string accountName);
    }
    public class ReportScoreCourseService : IReportScoreCourseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportScoreCourseService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> DeleteReportScore(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReportScoreCourseDto>> GetAllReportScores()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreCourseExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

                    var response = await client.GetAsync($"/api/ReportScoreCourses/ReportScoreCoursePaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreCourseExport>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/api/ReportScoreCourses/ReportScoreCoursePaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreCourseExport>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/api/ReportScoreCourses/ReportScoreCoursePaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreCourseExport>>(body);

                return users;
            }

            return default;
        }

        public Task<ReportScoreCourseDto> GetReportScoreById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ReportScoreCourseDto>> UserGetAllReportScores(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreCourses/UserGetReportScores?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreCourseDto>>(body);

            return users;
        }
        public async Task<bool> InsertReportScore(Guid quizCourseId, string accountName, CreateReportScoreCourseDto createReportScoreCourseDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(createReportScoreCourseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ReportScoreCourses?quizCourseId={quizCourseId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<ReportScoreCourseDto> CheckExist(Guid quizCourseId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ReportScoreCourses/CheckExist?quizCourseId={quizCourseId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var exist = JsonConvert.DeserializeObject<ReportScoreCourseDto>(body);

            return exist;
        }

        public async Task<CertificateCourse> GetCertificate(Guid quizCourseId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ReportScoreCourses/GetCertificate/{quizCourseId}/{accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var exist = JsonConvert.DeserializeObject<CertificateCourse>(body);

            return exist;
        }

        public async Task<IEnumerable<ReportScoreCourseExport>> ExportReportScoreCourseDtos(string fromDate, string toDate, bool passed)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreCourses/ExportExcel?fromDate={fromDate}&toDate={toDate}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreCourseExport>>(body);

            return users;
        }
    }
}
