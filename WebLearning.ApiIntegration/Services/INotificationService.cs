using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.Notification;

namespace WebLearning.ApiIntegration.Services
{
    public interface INotificationService
    {

        public Task<IEnumerable<NotificationResponseDto>> GetAllNotificationResponsesByUser(string accoutName);

        public Task<List<NotificationResponseDto>> UserGetListNotificationResponse(string accountName);



        public Task<bool> InsertNotificationResponse(CreateNotificationResponseDto createNotificationResponseDto);

        public Task<bool> DeleteNotification(Guid id);
        public Task<bool> UpdateNotificationResponse(UpdateNotificationResponseDto updateNotificationResponseDto, Guid Id, string accountName);



    }
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> DeleteNotification(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Notifications/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateNotificationResponse(UpdateNotificationResponseDto updateNotificationResponseDto, Guid id, string accountName)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateNotificationResponseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Notifications/{accountName}/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }
        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationResponsesByUser(string accoutName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Notifications/ShowNotificationResponse?accountName={accoutName}");

            var body = await response.Content.ReadAsStringAsync();

            var notifications = JsonConvert.DeserializeObject<IEnumerable<NotificationResponseDto>>(body);

            return notifications;
        }
        public async Task<bool> InsertNotificationResponse(CreateNotificationResponseDto createNotificationResponseDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createNotificationResponseDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/Notifications/CreateListNotificationResponse", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<NotificationResponseDto>> UserGetListNotificationResponse(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Notifications/{accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var notification = JsonConvert.DeserializeObject<List<NotificationResponseDto>>(body);

            return notification;
        }
    }
}

