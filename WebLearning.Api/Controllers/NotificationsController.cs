using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.Notification;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ApiBase
    {
        private readonly ILogger<NotificationsController> _logger;
        private readonly INotificationService _notifieService;
        public NotificationsController(ILogger<NotificationsController> logger, INotificationService notifieService)
        {
            _logger = logger;
            _notifieService = notifieService;
        }
        [HttpGet("NotificationResponse")]
        public async Task<IEnumerable<NotificationResponseDto>> GetNotifiesResponse(string accountName)
        {
            return await _notifieService.GetNotificationResponses(accountName);

        }
        [HttpGet("ShowNotificationResponse")]
        public async Task<IEnumerable<NotificationResponseDto>> ShowNotifiesResponse(string accountName)
        {
            return await _notifieService.ShowNotification(accountName);

        }
        [HttpPost("CreateListNotificationResponse")]
        public async Task<IActionResult> CreateListNotifiesResponse(CreateNotificationResponseDto createNotificationResponseDto, string accountName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _notifieService.InsertNotificationResponse(createNotificationResponseDto, accountName);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpGet("{accountName}")]
        public async Task<List<NotificationResponseDto>> GetNotificationResponseDetail(string accountName)
        {
            return await _notifieService.GetListResponse(accountName);

        }

        [HttpPut("{accountName}/{id}")]
        public async Task<IActionResult> UpdateNotificationResponse(Guid id, string accountName, [FromBody] UpdateNotificationResponseDto updateNotificationResponseDto)
        {
            try
            {
                if (updateNotificationResponseDto == null)
                    return BadRequest();
                if (await _notifieService.GetResponseDetail(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _notifieService.UpdateNotificationResponse(updateNotificationResponseDto, id, accountName);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        [HttpDelete("DeleteAllNotificationResponse")]
        public async Task<IActionResult> DeleteAllNotificationResponse(string accountName)
        {
            try
            {
                await _notifieService.DeleteAllNotificationResponse(accountName);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
