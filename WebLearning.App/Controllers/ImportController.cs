using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos;

namespace WebLearning.App.Controllers
{
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IImportExcelService _importExcelService;
        public INotyfService _notyfService { get; }

        public ImportController(ILogger<ImportController> logger, IHttpClientFactory factory, IImportExcelService importExcelService, INotyfService notyfService)
        {
            _logger = logger;
            _importExcelService = importExcelService;
            _notyfService = notyfService;
        }
        [Route("/load-file-excel.hmtl", Name = "HomeImport")]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import([FromForm] ImportResponse importResponse, IFormFile formFile, CancellationToken cancellationToken, bool role, bool account, bool courseRole, bool lession, bool videoLession, bool quizLession, bool quizCourse, bool quizMonthly, bool questionLession, bool questionCourse, bool questionMonthly)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            importResponse.File = formFile;
            var rs = await _importExcelService.ImportExcel(importResponse, cancellationToken, role, account, courseRole, lession, videoLession, quizLession, quizCourse, quizMonthly, questionLession, questionCourse, questionMonthly);

            if (rs == "Import Success")
            {
                _notyfService.Success("Thêm thành công. Vui lòng kiểm tra lại!");
                return RedirectToRoute("HomeImport");

            }
            else
            {
                _notyfService.Error(rs);
                return RedirectToRoute("HomeImport");
            }


        }
    }
}
