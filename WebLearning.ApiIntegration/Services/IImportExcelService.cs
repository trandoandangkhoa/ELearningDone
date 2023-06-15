using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos;

namespace WebLearning.ApiIntegration.Services
{
    public interface IImportExcelService
    {
        Task<string> ImportExcel(ImportResponse importResponse, CancellationToken cancellationToken, bool role, bool account, bool courseRole, bool lession, bool videoLession
                    , bool quizLession, bool quizCourse, bool quizMonthly, bool questionLession, bool questionCourse, bool questionMonthly);
        Task<string> ImportExcelAssets(ImportResponse importResponse, CancellationToken cancellationToken);
    }
    public class ImportExcelService : IImportExcelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImportExcelService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> ImportExcel([FromForm] ImportResponse importResponse, CancellationToken cancellationToken, bool role, bool account, bool courseRole, bool lession, bool videoLession, bool quizLession, bool quizCourse, bool quizMonthly, bool questionLession, bool questionCourse, bool questionMonthly)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();



            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();
            if (importResponse.File != null)
            {
                byte[] data;
                using (var br = new BinaryReader(importResponse.File.OpenReadStream()))
                {
                    data = br.ReadBytes((int)importResponse.File.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "File", importResponse.File.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(importResponse.Msg) ? "" : importResponse.Msg.ToString()), "msg");

            var json = JsonConvert.SerializeObject(importResponse);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ImportExcel/import?role={role}&account={account}&courseRole={courseRole}&lession={lession}&videoLession={videoLession}&quizLession={quizLession}&quizCourse={quizCourse}&quizMonthly={quizMonthly}&questionLession={questionLession}&questionCourse={questionCourse}&questionMonthly={questionMonthly}", requestContent);
            var token = await response.Content.ReadAsStringAsync();


            return token;
        }
        public async Task<string> ImportExcelAssets([FromForm] ImportResponse importResponse, CancellationToken cancellationToken)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();



            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();
            if (importResponse.File != null)
            {
                byte[] data;
                using (var br = new BinaryReader(importResponse.File.OpenReadStream()))
                {
                    data = br.ReadBytes((int)importResponse.File.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "File", importResponse.File.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(importResponse.Msg) ? "" : importResponse.Msg.ToString()), "msg");

            var json = JsonConvert.SerializeObject(importResponse);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ImportExcel/import/assets", requestContent);
            var token = await response.Content.ReadAsStringAsync();


            return token;
        }
    }

}
