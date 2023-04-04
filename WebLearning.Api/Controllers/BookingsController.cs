using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Contract.Dtos;
using Microsoft.Extensions.Configuration;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBookingService _bookingsService;
        public BookingsController(ILogger<BookingsController> logger, IConfiguration configuration, IBookingService bookingsService)
        {
            _logger = logger;
            _configuration = configuration;
            _bookingsService = bookingsService;
        }
        [HttpGet]

        public async Task<IActionResult> GetUrlAddress(string role)

        {
            var urlAddress =  await _bookingsService.GetUrlBooking(role);
            if(urlAddress == null) { return StatusCode(StatusCodes.Status400BadRequest); }
            return StatusCode(StatusCodes.Status200OK,urlAddress);

        }
    }
}
