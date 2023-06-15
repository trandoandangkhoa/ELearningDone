using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Category;

namespace WebLearning.AppUser.Controllers
{
    public class AssetCategoryController : Controller
    {
        private readonly ILogger<AssetCategoryController> _logger;
        private readonly IAssetCategoryService _assetCategoryService;
        public INotyfService _notyfService { get; }

        public AssetCategoryController(ILogger<AssetCategoryController> logger, IHttpClientFactory factory, IAssetCategoryService assetCategoryService, INotyfService notyfService)
        {
            _logger = logger;
            _assetCategoryService = assetCategoryService;
            _notyfService = notyfService;
        }
        [Route("/loai-tai-san.html")]
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
            var Roke = await _assetCategoryService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return Redirect("/dang-nhap.html");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-loai-tai-san/{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var allCourse = await _assetCategoryService.GetAllAssetsCategory();

            List<SelectListItem> list = new();

            foreach (var item in allCourse)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }

            ViewBag.Category = list;
            var AssetCategory = await _assetCategoryService.GetAssetCategoryById(id);
            return View(AssetCategory);
        }
        [HttpGet]
        [Route("/tao-moi-loai-tai-san.html")]
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
        [Route("/tao-moi-loai-tai-san.html")]
        public async Task<IActionResult> Create(CreateAssetsCategoryDto createAssetCategoryDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAssetCategoryDto != null)
            {
                await _assetCategoryService.InsertAssetCategory(createAssetCategoryDto);
                _notyfService.Success("Tạo thành công");

                return Redirect("/loai-tai-san.html");

            }
            return View(createAssetCategoryDto);
        }
        [HttpGet]
        [Route("/cap-nhat-loai-tai-san/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _assetCategoryService.GetAssetCategoryById(id);
            var updateResult = new UpdateAssetsCategoryDto()
            {
                Name = result.Name,
            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-loai-tai-san/{id}")]
        public async Task<IActionResult> Edit(UpdateAssetsCategoryDto updateAssetsCategoryDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsCategoryDto != null)
            {
                await _assetCategoryService.UpdateAssetCategory(updateAssetsCategoryDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect("/loai-tai-san.html");
            }
            return View(updateAssetsCategoryDto);
        }
        [HttpGet]
        [Route("/xoa-loai-tai-san/{id}")]

        public async Task<IActionResult> Delete(AssetsCategoryDto dto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetCategory = await _assetCategoryService.GetAssetCategoryById(id);
            dto.Id = assetCategory.Id;
            dto.Name = assetCategory.Name;
            dto.CatCode = assetCategory.CatCode;

            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-loai-tai-san/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetCategoryService.DeleteAssetCategory(id);
            _notyfService.Success("Xóa thành công");
            return Redirect("/loai-tai-san.html");
        }
    }
}
