using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebLearning.ApiIntegration.Services;
using WebLearning.App.Models;

namespace WebLearning.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _loginService;

        private readonly ICourseService _courseService;
        private readonly ILessionService _lessionService;
        private readonly IQuizCourseService _quizCourseService;

        private readonly IQuizLessionService _quizLessionService;

        private readonly IQuizMonthlyService _quizMonthlyService;
        private readonly IAccountService _accountService;

        public HomeController(ILogger<HomeController> logger, ILoginService loginService, ICourseService courseService, ILessionService lessionService, IQuizCourseService quizCourseService, IQuizLessionService quizLessionService, IQuizMonthlyService quizMonthlyService,
            IAccountService accountService, IReportScoreCourseService reportScoreCourseService)
        {
            _logger = logger;
            _loginService = loginService;
            _courseService = courseService;
            _lessionService = lessionService;
            _quizCourseService = quizCourseService;
            _quizLessionService = quizLessionService;
            _quizMonthlyService = quizMonthlyService;
            _accountService = accountService;
        }
        [Authorize]
        public async Task<IActionResult> Index(DashboardViewModel dashboardViewModel)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            dashboardViewModel.CourseDtos = await _courseService.GetAllCourse();
            dashboardViewModel.LessionDtos = await _lessionService.GetAllLession();
            dashboardViewModel.QuizCourseDtos = await _quizCourseService.GetAllQuiz();
            dashboardViewModel.QuizlessionDtos = await _quizLessionService.GetAllQuiz();
            dashboardViewModel.QuizMonthlyDtos = await _quizMonthlyService.GetAllQuiz();
            dashboardViewModel.AccountDtos = await _accountService.GetAccount();
            return View(dashboardViewModel);
        }

        public IActionResult Privacy()
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