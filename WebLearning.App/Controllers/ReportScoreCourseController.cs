using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;

namespace WebLearning.App.Controllers
{
    public class ReportScoreCourseController : Controller
    {
        private readonly ILogger<ReportScoreCourseController> _logger;

        private readonly IReportScoreCourseService _reportScoreCourseService;
        private readonly IQuizCourseService _quizCourseService;
        private readonly ICourseService _CourseService;
        public INotyfService _notyfService { get; }

        public ReportScoreCourseController(ILogger<ReportScoreCourseController> logger, IHttpClientFactory factory, INotyfService notyfService, IReportScoreCourseService reportScoreCourseService, IQuizCourseService quizCourseService, ICourseService CourseService)
        {
            _logger = logger;
            _notyfService = notyfService;
            _reportScoreCourseService = reportScoreCourseService;
            _quizCourseService = quizCourseService;
            _CourseService = CourseService;
        }
        [Route("/bao-cao-tung-khoa-hoc.html")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
            };
            var Roke = await _reportScoreCourseService.GetPaging(request);
            foreach (var item in Roke.Items)
            {
                var nameCourse = await _CourseService.GetName(item.CourseId);
                var nameQuiz = await _quizCourseService.GetName(item.QuizCourseId);
                item.CourseName = nameCourse.Name;
                item.QuizName = nameQuiz.Name;
            }
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }

            return View(Roke);
        }

        public async Task<IActionResult> Export(string fromDate, string toDate, bool passed)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var export = await _reportScoreCourseService.ExportReportScoreCourseDtos(fromDate, toDate, passed);

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("kết quả kiểm tra từng khóa");
                sheet.Cells[1, 1].Value = "Id";
                sheet.Cells[1, 2].Value = "Tên bài kiểm tra";
                sheet.Cells[1, 3].Value = "Tên khóa học";
                sheet.Cells[1, 4].Value = "Email";
                sheet.Cells[1, 5].Value = "Họ và tên";
                sheet.Cells[1, 6].Value = "Thời gian nộp bài";
                sheet.Cells[1, 7].Value = "Điểm";
                sheet.Cells[1, 8].Value = "Hoàn thành";
                sheet.Cells[1, 9].Value = "Địa chỉ IP";

                int rowInd = 2;

                foreach (var lo in export)
                {
                    sheet.Cells[rowInd, 1].Value = lo.Id;
                    sheet.Cells[rowInd, 2].Value = lo.QuizName;
                    sheet.Cells[rowInd, 3].Value = lo.CourseName;
                    sheet.Cells[rowInd, 4].Value = lo.UserName;
                    sheet.Cells[rowInd, 5].Value = lo.FullName;
                    sheet.Cells[rowInd, 6].Value = lo.CompletedDate.ToString("dd/MM/yyyy");
                    sheet.Cells[rowInd, 7].Value = lo.TotalScore;
                    sheet.Cells[rowInd, 8].Value = lo.Passed;
                    sheet.Cells[rowInd, 9].Value = lo.IpAddress;
                    rowInd++;
                }
                package.Save();
            }

            stream.Position = 0;

            var fileName = $"KetQuaKiemTraTungBai_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

    }
}
