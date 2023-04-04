using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.AppUser.Models;
using WebLearning.Contract.Dtos.Notification;

namespace WebLearning.AppUser.Controllers.Components
{
    public class HeaderNotifyViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        private readonly INotificationService _notificationService;

        private readonly ICourseService courseService;




        public HeaderNotifyViewComponent(IAccountService accountService, INotificationService notificationService, ICourseService courseService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
            this.courseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listNotification = await _notificationService.GetAllNotificationResponsesByUser(User.Identity.Name);

            NotificationItemViewModel notificationItemViewModels = new();

            CreateNotificationResponseDto createNotificationResponseDto = new();


            var course = await _accountService.GetAccountByEmail(User.Identity.Name);


            notificationItemViewModels.OwnCourseDtos = course.CourseDtos.Where(x => x.Active == true && x.CourseRoleDtos.Any(x => x.RoleId.Equals(course.AccountDto.RoleId))).ToList();

            notificationItemViewModels.NotificationResponseDtos = listNotification.Where(x => x.Notify == true && x.AccountName.Equals(course.AccountDto.Email) && x.RoleId.Equals(course.AccountDto.RoleId)).OrderByDescending(x => x.DateCreated).ToList();

            notificationItemViewModels.QuizMonthlyDtos = course.QuizMonthlyDtos.Where(x => x.Active == true && x.RoleId.Equals(course.AccountDto.RoleId)).ToList();

            return View(notificationItemViewModels);
        }


    }
}
