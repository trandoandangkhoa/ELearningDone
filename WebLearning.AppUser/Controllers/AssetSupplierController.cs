using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Supplier;

namespace WebLearning.AppUser.Controllers
{
    public class AssetSupplierController : Controller
    {
        private readonly ILogger<AssetSupplierController> _logger;
        private readonly IAssetSupplierService _assetSupplierService;
        public INotyfService _notyfService { get; }

        public AssetSupplierController(ILogger<AssetSupplierController> logger, IHttpClientFactory factory, IAssetSupplierService assetSupplierService, INotyfService notyfService)
        {
            _logger = logger;
            _assetSupplierService = assetSupplierService;
            _notyfService = notyfService;
        }
        [Route("/nha-cung-cap.html")]
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
            var Roke = await _assetSupplierService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return Redirect("/dang-nhap.html");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-nha-cung-cap/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var AssetSupplier = await _assetSupplierService.GetAssetSupplierById(id);
            return View(AssetSupplier);
        }
        [HttpGet]
        [Route("/tao-moi-nha-cung-cap.html")]
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
        [Route("/tao-moi-nha-cung-cap.html")]
        public async Task<IActionResult> Create(CreateAssetsSupplierDto createAssetsSupplierDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAssetsSupplierDto != null)
            {
                await _assetSupplierService.InsertAssetSupplier(createAssetsSupplierDto);
                _notyfService.Success("Tạo thành công");

                return Redirect("/nha-cung-cap.html");

            }
            return View(createAssetsSupplierDto);
        }
        [HttpGet]
        [Route("/cap-nhat-nha-cung-cap/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _assetSupplierService.GetAssetSupplierById(id);
            var updateResult = new UpdateAssetsSupplierDto()
            {
                CompanyTaxCode = result.CompanyTaxCode,
                CompanyName = result.CompanyName,
                Address = result.Address,
                Phone = result.Phone,
                Fax = result.Fax,
            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-nha-cung-cap/{id}")]
        public async Task<IActionResult> Edit(UpdateAssetsSupplierDto updateAssetsSupplierDto, string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsSupplierDto != null)
            {
                await _assetSupplierService.UpdateAssetSupplier(updateAssetsSupplierDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect("/nha-cung-cap.html");
            }
            return View(updateAssetsSupplierDto);
        }
        [HttpGet]
        [Route("/xoa-nha-cung-cap/{id}")]

        public async Task<IActionResult> Delete(AssetsSupplierDto dto, string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetSupplier = await _assetSupplierService.GetAssetSupplierById(id);
            dto.Id = assetSupplier.Id;
            dto.CompanyName = assetSupplier.CompanyName;
            dto.Address = assetSupplier.Address;
            dto.Phone = assetSupplier.Phone;
            dto.CompanyTaxCode = assetSupplier.CompanyTaxCode;
            dto.Fax = assetSupplier.Fax;

            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-nha-cung-cap/{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetSupplierService.DeleteAssetSupplier(id);
            _notyfService.Success("Xóa thành công");
            return Redirect("/nha-cung-cap.html");
        }
    }
}
