using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Domain.Entites;

namespace WebLearning.ApiIntegration.Services
{
    public interface IBookingService
    {
        Task<string> GetUrlBooking(string role);
        Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlots(DateTime start, DateTime end, int doctor);
        Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlotsManager(DateTime start, DateTime end, int doctor);
        Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlotsAdmin(DateTime start, DateTime end, int doctor);

        Task<IEnumerable<HistoryAddSlot>> BookSuccessed(string email);
        Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance);
        Task<Result> ConfirmedBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> ConfirmedBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> ReplyMoveBookingAccepted(Guid fromId, Guid toId);
        Task<Result> ReplyMoveBookingRejected(Guid fromId, Guid toId);

        Task<Result> DeleteAppointmentBooked(Guid codeId);

    }
    public class BookingService : IBookingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<HistoryAddSlot>> BookSuccessed(string email)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Appointments/booked/successed/email?email={email}");

            var body = await response.Content.ReadAsStringAsync();

            var his = JsonConvert.DeserializeObject<IEnumerable<HistoryAddSlot>>(body);

            return his;
        }

        public async Task<Result> ConfirmedBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateHistoryAddSlotDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Appointments/confirmed/accepted/{fromId}/{toId}/{email}", httpContent);


            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<Result>(body);

            return url;
        }
        public async Task<Result> ConfirmedBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateHistoryAddSlotDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Appointments/confirmed/rejected/{fromId}/{toId}/{email}", httpContent);

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<Result>(body);

            return url;
        }
        public async Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAppointmentSlotAdvance);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Appointments/create/advance", httpContent);

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<List<Result>>(body);

            return url;
        }

        public async Task<Result> DeleteAppointmentBooked(Guid codeId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Appointments/booked/{codeId}");

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<Result>(body);

            return url;
        }

        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlots([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int doctor)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/Appointments?start={start}&end={end}&doctor={doctor}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AppointmentSlotDto>>(body);

            return users;
        }

        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlotsAdmin(DateTime start, DateTime end, int doctor)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/Appointments/admin?start={start}&end={end}&doctor={doctor}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AppointmentSlotDto>>(body);

            return users;
        }

        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentSlotsManager(DateTime start, DateTime end, int doctor)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/Appointments/manager?start={start}&end={end}&doctor={doctor}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AppointmentSlotDto>>(body);

            return users;
        }

        public async Task<string> GetUrlBooking(string role)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Bookings&role={role}");

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<string>(body);

            return url;
        }

        public async Task<Result> ReplyMoveBookingAccepted(Guid fromId, Guid toId)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Appointments/mailreplied/accepted/{fromId}/{toId}/");

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<Result>(body);

            return url;
        }

        public async Task<Result> ReplyMoveBookingRejected(Guid fromId, Guid toId)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Appointments/mailreplied/rejected/{fromId}/{toId}/");

            var body = await response.Content.ReadAsStringAsync();

            var url = JsonConvert.DeserializeObject<Result>(body);

            return url;
        }
    }
}
