using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Status;

namespace WebLearning.AppUser.Controllers
{
    public class AssetStatusController : Controller
    {
        private readonly ILogger<AssetStatusController> _logger;
        private readonly IAssetStatusService _assetStatusService;
        public INotyfService _notyfService { get; }

        public AssetStatusController(ILogger<AssetStatusController> logger, IHttpClientFactory factory, IAssetStatusService assetStatusService, INotyfService notyfService)
        {
            _logger = logger;
            _assetStatusService = assetStatusService;
            _notyfService = notyfService;
        }
        [Route("/tinh-trang-tai-san.html")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
            };
            var Roke = await _assetStatusService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return Redirect("/dang-nhap.html");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-tinh-trang-tai-san/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var allCourse = await _assetStatusService.GetAllAssetsStatus();

            List<SelectListItem> list = new();

            foreach (var item in allCourse)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }

            ViewBag.Status = list;
            var AssetStatus = await _assetStatusService.GetAssetStatusById(id);
            return View(AssetStatus);
        }
        [HttpGet]
        [Route("/tao-moi-tinh-trang-tai-san.html")]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-tinh-trang-tai-san.html")]
        public async Task<IActionResult> Create(CreateAssetsStatusDto createAssetStatusDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAssetStatusDto != null)
            {
                await _assetStatusService.InsertAssetStatus(createAssetStatusDto);
                _notyfService.Success("Tạo thành công");

                return Redirect("/tinh-trang-tai-san.html");

            }
            return View(createAssetStatusDto);
        }
        [HttpGet]
        [Route("/cap-nhat-tinh-trang-tai-san/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _assetStatusService.GetAssetStatusById(id);
            var updateResult = new UpdateAssetsStatusDto()
            {
                Name = result.Name,
            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-tinh-trang-tai-san/{id}")]
        public async Task<IActionResult> Edit(UpdateAssetsStatusDto updateAssetsStatusDto, int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsStatusDto != null)
            {
                await _assetStatusService.UpdateAssetStatus(updateAssetsStatusDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect("/tinh-trang-tai-san.html");
            }
            return View(updateAssetsStatusDto);
        }
        [HttpGet]
        [Route("/xoa-tinh-trang-tai-san/{id}")]

        public async Task<IActionResult> Delete(AssetsStatusDto dto, int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetStatus = await _assetStatusService.GetAssetStatusById(id);
            dto.Id = assetStatus.Id;
            dto.Name = assetStatus.Name;
            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-tinh-trang-tai-san/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetStatusService.DeleteAssetStatus(id);
            _notyfService.Success("Xóa thành công");
            return Redirect("/tinh-trang-tai-san.html");
        }
    }
}
