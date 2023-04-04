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
    public interface IReportScoreMonthlyService
    {
        public Task<PagedViewModel<ReportScoreMonthlyExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        public Task<IEnumerable<ReportScoreMonthlyDto>> GetAllReportScores();

        public Task<IEnumerable<ReportScoreMonthlyDto>> UserGetAllReportScores(string accountName);

        public Task<ReportScoreMonthlyDto> GetReportScoreById(Guid id);
        public Task<bool> InsertReportScore(Guid quizMonthlyId, string accountName, CreateReportScoreMonthlyDto createReportScoreMonthlyDto);
        public Task<CertificateMonthly> GetCertificate(Guid quizMonthlyId, string accountName);
        Task<IEnumerable<ReportScoreMonthlyExport>> ExportReportScoreMonthlyDtos(string fromDate, string toDate, bool passed);

        public Task<bool> DeleteReportScore(Guid id);

        public Task<ReportScoreMonthlyDto> CheckExist(Guid quizMonthlyId, string accountName);
    }
    public class ReportScoreMonthlyService : IReportScoreMonthlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportScoreMonthlyService(IHttpClientFactory httpClientFactory,
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

        public Task<IEnumerable<ReportScoreMonthlyDto>> GetAllReportScores()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreMonthlyExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

                    var response = await client.GetAsync($"/api/ReportScoreMonthlies/ReportScoreMonthlyPaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreMonthlyExport>>(body);

                    return users;
                }
                else if (a != null)
                {

                    var response = await client.GetAsync($"/api/ReportScoreMonthlies/ReportScoreMonthlyPaging?PageIndex=" +
                        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

                    var body = await response.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreMonthlyExport>>(body);

                    return users;
                }
            }
            else
            {
                var response = await client.GetAsync($"/api/ReportScoreMonthlies/ReportScoreMonthlyPaging?PageIndex=" +
                $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

                var body = await response.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<PagedViewModel<ReportScoreMonthlyExport>>(body);

                return users;
            }

            return default;
        }

        public Task<ReportScoreMonthlyDto> GetReportScoreById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ReportScoreMonthlyDto>> UserGetAllReportScores(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreMonthlies/UserGetReportScores?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreMonthlyDto>>(body);

            return users;
        }
        public async Task<bool> InsertReportScore(Guid quizMonthlyId, string accountName, CreateReportScoreMonthlyDto createReportScoreMonthlyDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(createReportScoreMonthlyDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ReportScoreMonthlies?quizMonthlyId={quizMonthlyId}&accountName={accountName}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<ReportScoreMonthlyDto> CheckExist(Guid quizMonthlyId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ReportScoreMonthlies/CheckExist?quizMonthlyId={quizMonthlyId}&accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var exist = JsonConvert.DeserializeObject<ReportScoreMonthlyDto>(body);

            return exist;
        }

        public async Task<CertificateMonthly> GetCertificate(Guid quizMonthlyId, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ReportScoreMonthlies/GetCertificate/{quizMonthlyId}/{accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var exist = JsonConvert.DeserializeObject<CertificateMonthly>(body);

            return exist;
        }

        public async Task<IEnumerable<ReportScoreMonthlyExport>> ExportReportScoreMonthlyDtos(string fromDate, string toDate, bool passed)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/ReportScoreMonthlies/ExportExcel?fromDate={fromDate}&toDate={toDate}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<ReportScoreMonthlyExport>>(body);

            return users;
        }
    }
}
