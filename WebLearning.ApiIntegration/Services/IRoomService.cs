using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Contract.Dtos.BookingCalender.Room;

namespace WebLearning.ApiIntegration.Services
{
    public interface IRoomService
    {
        public Task<IEnumerable<RoomDto>> GetAllRooms();

        public Task<RoomDto> GetRoomById(int id);
        public Task<bool> InsertRoom(CreateRoomDto createRoomDto);
        public Task<bool> DeleteRoom(int id);
        public Task<bool> UpdateRoom(UpdateRoomDto updateRoomDto, int Id);

    }
    public class RoomService : IRoomService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoomService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> InsertRoom(CreateRoomDto createRoomDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createRoomDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Rooms/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteRoom(int id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Rooms/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<RoomDto> GetRoomById(int id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Rooms/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<RoomDto>(body);

            return user;
        }
        public async Task<bool> UpdateRoom(UpdateRoomDto updateRoomDto, int id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateRoomDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Rooms/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<RoomDto>> GetAllRooms()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Rooms");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<RoomDto>>(body);
            return users;
        }
    }
}
