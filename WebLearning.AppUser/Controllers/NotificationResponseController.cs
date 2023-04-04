using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.Notification;

namespace WebLearning.AppUser.Controllers
{
    public class NotificationResponseController : Controller
    {
        private readonly ILogger<NotificationResponseController> _logger;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        private readonly INotyfService _notyf;

        public NotificationResponseController(ILogger<NotificationResponseController> logger, IConfiguration configuration, INotificationService notificationService
                        , INotyfService notyfService)
        {
            _logger = logger;
            _configuration = configuration;
            _notificationService = notificationService;
            _notyf = notyfService;
        }
        [Route("/danh-dau/{id}/{accountName}")]
        public async Task<IActionResult> UpdateNotify(string accountName, Guid id, UpdateNotificationResponseDto updateNotificationResponseDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                var update = await _notificationService.UpdateNotificationResponse(updateNotificationResponseDto, id, accountName);

                if (update == true)
                {
                    return Redirect("/");
                }
                return Redirect("/khong-tim-thay-trang.html");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [Route("/thong-bao.html")]
        public async Task<IActionResult> Index(string accountName)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");

                accountName = User.Identity.Name;

                var update = await _notificationService.GetAllNotificationResponsesByUser(accountName);

                return View(update);

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
