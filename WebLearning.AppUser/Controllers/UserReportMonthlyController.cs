using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;

namespace WebLearning.AppUser.Controllers
{
    public class UserReportMonthlyController : Controller
    {
        private readonly IReportScoreMonthlyService _scoreCourseService;

        private readonly INotyfService _notyf;
        public UserReportMonthlyController(IReportScoreMonthlyService scoreCourseService, INotyfService notyfService)
        {
            _scoreCourseService = scoreCourseService;
            _notyf = notyfService;
        }
        [Route("/hoan-thanh-bai-dinh-ki/{quizMonthlyId}/{accountName}")]
        public async Task<IActionResult> Certificate(Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var report = await _scoreCourseService.GetCertificate(quizMonthlyId, User.Identity.Name);

            if (report == null)
            {
                _notyf.Error("Bạn chưa hoàn thành bai định kì");

                return Redirect("/khong-tim-thay-trang.html");
            }
            return View(report);
        }

        //public async Task<IActionResult> ExportToPDF(Guid quizCourseId, string accountName)
        //{
        //    var Renderer = new IronPdf.ChromePdfRenderer();
        //    string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/hoan-thanh-khoa-hoc/{quizCourseId}/{accountName}";

        //    var pdf = Renderer.RenderUrlAsPdf($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/hoan-thanh-khoa-hoc/{quizCourseId}/{accountName}");
        //    pdf.SaveAs("output.pdf");
        //    var browserFetcher = new BrowserFetcher();
        //    await browserFetcher.DownloadAsync();

        //    using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

        //    await using var page = await browser.NewPageAsync();
        //    await page.GoToAsync(url);
        //    await page.PdfAsync($"chungnhankhoahoc{accountName}{DateTime.Today.ToShortDateString().Replace("/", "-")}.pdf");

        //    return default;
        //}

    }
}
