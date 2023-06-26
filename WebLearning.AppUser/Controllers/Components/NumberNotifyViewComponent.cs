using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.AppUser.Models;

namespace WebLearning.AppUser.Controllers.Component
{
    public class NumberNotifyViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        private readonly INotificationService _notificationService;

        public NumberNotifyViewComponent(IAccountService accountService, INotificationService notificationService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listNotification = await _notificationService.GetAllNotificationResponsesByUser(User.Identity.Name);

            NotificationItemViewModel notificationItemViewModels = new();

            var ownCourse = await _accountService.GetAccountByEmail(User.Identity.Name);


            notificationItemViewModels.NotificationResponseDtos = listNotification.ToList();


            return View(notificationItemViewModels);
        }
    }
}
