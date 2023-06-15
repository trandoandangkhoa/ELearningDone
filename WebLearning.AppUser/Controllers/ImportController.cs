using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos;

namespace WebLearning.AppUser.Controllers
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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import([FromForm] ImportResponse importResponse, IFormFile formFile, CancellationToken cancellationToken)
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("dang-nhap.html");
            //}
            importResponse.File = formFile;
            var rs = await _importExcelService.ImportExcelAssets(importResponse, cancellationToken);

            if (rs == "Import Success")
            {
                _notyfService.Success("Thêm thành công. Vui lòng kiểm tra lại!");
                return Redirect("/tai-san.html");

            }
            else
            {
                _notyfService.Error(rs);
                return Redirect("/tai-san.html");
            }


        }
    }
}
