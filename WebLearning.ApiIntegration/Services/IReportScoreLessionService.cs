using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.ApiIntegration.Services
{
    public interface IReportScoreLessionService
    {
        public Task<PagedViewModel<ReportScoreLessionExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<ReportScoreLessionDto>> GetAllReportScores();

        public Task<ReportScoreLessionDto> CheckExist(Guid quizCourseId, string accountName);

        public Task<IEnumerable<ReportScoreLessionDto>> UserGetAllReportScores(string accountName);

        public Task<ReportScoreLessionDto> GetReportScoreById(Guid id);
        public Task<bool> InsertReportScore(Guid quizLessionId, string accountName, CreateReportScoreLessionDto createReportScoreLessionDto);

        public Task<bool> UpdateReportScore(Guid quizLessionId, string accountName, UpdateReportScoreLessionDto updateReportScoreLessionDto);

        public Task<bool> DeleteReportScore(Guid id);

        public Task<bool> ResetReportScore(Guid quizLessionId, string accountName);
        Task<IEnumerable<ReportScoreLessionExport>> ExportReportScoreLessionDtos(string fromDate, string toDate, bool passed);

    }
    public class ReportScoreLessionService : IReportScoreLessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportScoreLessionService(IHttpClientFactory httpClientFactory,
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

        public Task<IEnumerable<ReportScoreLessionDto>> GetAllReportScores()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreLessionExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

                    var response = await client.GetAsync($"/api/ReportScoreLessions/ReportScoreLessionPaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreLessionExport>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/api/ReportScoreLessions/ReportScoreLessionPaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreLessionExport>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/api/ReportScoreLessions/ReportScoreLessionPaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreLessionExport>>(body);

                return users;
            }

            return default;
        }

        public Task<ReportScoreLessionDto> GetReportScoreById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ReportScoreLessionDto>> UserGetAllReportScores(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreLessions/UserGetReportScores?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreLessionDto>>(body);

            return users;
        }
        public async Task<bool> InsertReportScore(Guid quizLessionId, string accountName, CreateReportScoreLessionDto createReportScoreLessionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(createReportScoreLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ReportScoreLessions?quizLessionId={quizLessionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateReportScore(Guid quizLessionId, string accountName, UpdateReportScoreLessionDto updateReportScoreLessionDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(updateReportScoreLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/ReportScoreLessions?quizLessionId={quizLessionId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetReportScore(Guid quizLessionId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/ReportScoreLessions/ResetAllReport/{quizLessionId}?accountName={accountName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ReportScoreLessionDto> CheckExist(Guid quizLessionId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ReportScoreLessions/CheckExist?quizLessionId={quizLessionId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var exist = JsonConvert.DeserializeObject<ReportScoreLessionDto>(body);

            return exist;
        }

        public async Task<IEnumerable<ReportScoreLessionExport>> ExportReportScoreLessionDtos(string fromDate, string toDate, bool passed)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreLessions/ExportExcel?fromDate={fromDate}&toDate={toDate}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreLessionExport>>(body);

            return users;
        }
    }
}
