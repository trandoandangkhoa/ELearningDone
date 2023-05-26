using Microsoft.Extensions.Configuration;

namespace WebLearning.Application.Services
{
    public interface IBookingService
    {
        Task<string> GetUrlBooking(string role);
    }
    public class BookingService : IBookingService
    {
        private readonly IConfiguration _configuration;
        public BookingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> GetUrlBooking(string role)
        {
            string urlAddress = null;

            if (role == "AdminRole" || role == "ManagerRole" || role == "TeacherRole")
            {
                urlAddress = _configuration.GetValue<string>("UrlBooking:Manager");

            }
            else if (role == "StudentRole")
            {
                urlAddress = _configuration.GetValue<string>("UrlBooking:User");
            }
            return Task.FromResult(urlAddress);
        }
    }
}
