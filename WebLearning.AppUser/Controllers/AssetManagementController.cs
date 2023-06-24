using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.Assets.Chart;

namespace WebLearning.AppUser.Controllers
{
    public class AssetManagementController : Controller
    {
        private readonly ILogger<AssetManagementController> _logger;
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IAssetService _assetService;
        private readonly IIChartService _chartService;
        public INotyfService _notyfService { get; }

        public AssetManagementController(ILogger<AssetManagementController> logger, IHttpClientFactory factory, IAssetCategoryService assetCategoryService, INotyfService notyfService, IAssetService assetService, IIChartService chartService)
        {
            _logger = logger;
            _assetCategoryService = assetCategoryService;
            _notyfService = notyfService;
            _assetService = assetService;
            _chartService = chartService;
        }
        [Route("/trang-chu-tai-san.html")]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = new HomeViewModel();
            return View(a);
        }
        public async Task<IActionResult> TotalAssetInCategory()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var data = await _chartService.TotalAssetsCategory();

            return View(data);
        }
        public async Task<IActionResult> TotalAssetInStatus()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var data = await _chartService.TotalAssetsStatus();



            return View(data);
        }
        public async Task<IActionResult> TotalAssetAvailable()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var data = await _chartService.TotalAssetsAvailable();



            return View(data);
        }
    }
}
