using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.AppUser.Models;

namespace WebLearning.AppUser.Controllers.Components
{
    public class HeaderNotifyViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        private readonly INotificationService _notificationService;




        public HeaderNotifyViewComponent(IAccountService accountService, INotificationService notificationService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listNotification = await _notificationService.GetAllNotificationResponsesByUser(User.Identity.Name);

            NotificationItemViewModel notificationItemViewModels = new();

            var course = await _accountService.GetAccountByEmail(User.Identity.Name);


            notificationItemViewModels.OwnCourseDtos = course.OwnCourseDtos;

            notificationItemViewModels.NotificationResponseDtos = listNotification.Where(x => x.Notify == true).ToList();

            notificationItemViewModels.QuizMonthlyDtos = course.QuizMonthlyDtos;

            return View(notificationItemViewModels);
        }


    }
}
