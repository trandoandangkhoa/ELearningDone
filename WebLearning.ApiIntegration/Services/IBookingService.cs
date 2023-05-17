using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;

namespace WebLearning.ApiIntegration.Services
{
    public interface IBookingService
    {
        Task<string> GetUrlBooking(string role);
        Task<IEnumerable<AppointmentSlot>> GetAppointmentSlots();
        Task<IEnumerable<HistoryAddSlot>> BookSuccessed(string email);
        Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance);
        Task<Result> ConfirmedBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId,string email);
        Task<Result> ConfirmedBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> ReplyBookingAccepted(Guid fromId, Guid toId);

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

        public async Task<Result> ConfirmedBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId,string email)
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

        public Task<IEnumerable<AppointmentSlot>> GetAppointmentSlots()
        {
            throw new NotImplementedException();
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

        public async Task<Result> ReplyBookingAccepted(Guid fromId, Guid toId)
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
    }
}
