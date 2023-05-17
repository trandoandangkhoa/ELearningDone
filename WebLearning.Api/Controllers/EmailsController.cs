using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.BookingCalender;
using WebLearning.Application.Email;
using WebLearning.Domain.Entites;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public EmailsController(ILogger<EmailsController> logger, IConfiguration configuration, IEmailSender emailSender)
        {
            _logger = logger;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        [HttpPost("MailsWeeklyInMonth")]
        public async Task<IActionResult> Send(string emailAccount)
        {
            //var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

            //var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:From");



            //var message = new Message(new string[] { $"{emailAccount}" }, "THÔNG BÁO DỜI LỊCH", $"{body}");

            //_emailSender.ReplyEmail(message, emailAdmin, "sasas", emailAccount);

            return Ok();
        }
    }
}
