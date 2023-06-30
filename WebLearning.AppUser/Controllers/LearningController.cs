using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebLearning.ApiIntegration.Services;
using WebLearning.AppUser.Models;

namespace WebLearning.AppUser.Controllers
{
    public class LearningController : Controller
    {
        private readonly ILogger<LearningController> _logger;
        private readonly IAccountService _accountService;

        private readonly INotyfService _notyf;
        public LearningController(ILogger<LearningController> logger, INotyfService notyf, IAccountService accountService)

        {
            _logger = logger;
            _notyf = notyf;
            _accountService = accountService;
        }
        [Authorize]
        [Route("elearning.html")]
        public async Task<IActionResult> Index()
        {
            var account = await _accountService.GetAccountByEmail(User.Identity.Name);

            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            DashboardViewModel dashboardView = new()
            {
                CourseDtos = account.CourseDtos,

                LessionDtos = account.LessionDtos,

                QuizlessionDtos = account.QuizlessionDtos,

                OwnCourseDtos = account.OwnCourseDtos,


                ReportScoreLessionDtos = account.ReportScoreLessionDtos,

                ReportScoreCourseDtos = account.ReportScoreCourseDtos,

                ReportScoreMonthlyDtos = account.ReportScoreMonthlyDtos,
            };



            return View(dashboardView);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/khong-tim-thay-trang.html")]
        public IActionResult NoneExist()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}