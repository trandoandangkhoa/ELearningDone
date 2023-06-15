using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Department;

namespace WebLearning.AppUser.Controllers
{
    public class AssetDepartmentController : Controller
    {
        private readonly ILogger<AssetDepartmentController> _logger;
        private readonly IAssetDepartmentService _assetDepartmentService;
        public INotyfService _notyfService { get; }

        public AssetDepartmentController(ILogger<AssetDepartmentController> logger, IHttpClientFactory factory, IAssetDepartmentService assetDepartmentService, INotyfService notyfService)
        {
            _logger = logger;
            _assetDepartmentService = assetDepartmentService;
            _notyfService = notyfService;
        }
        [Route("/bo-phan.html")]
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
            var Roke = await _assetDepartmentService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return Redirect("/dang-nhap.html");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-bo-phan/{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var allCourse = await _assetDepartmentService.GetAllAssetsDepartment();

            List<SelectListItem> list = new();

            foreach (var item in allCourse)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }

            ViewBag.Department = list;
            var AssetDepartment = await _assetDepartmentService.GetAssetDepartmentById(id);
            return View(AssetDepartment);
        }
        [HttpGet]
        [Route("/tao-moi-bo-phan.html")]
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
        [Route("/tao-moi-bo-phan.html")]
        public async Task<IActionResult> Create(CreateAssetsDepartmentDto createAssetDepartmentDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAssetDepartmentDto != null)
            {
                await _assetDepartmentService.InsertAssetDepartment(createAssetDepartmentDto);
                _notyfService.Success("Tạo thành công");

                return Redirect("/bo-phan.html");

            }
            return View(createAssetDepartmentDto);
        }
        [HttpGet]
        [Route("/cap-nhat-bo-phan/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _assetDepartmentService.GetAssetDepartmentById(id);
            var updateResult = new UpdateAssetsDepartmentDto()
            {
                Name = result.Name,
            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-bo-phan/{id}")]
        public async Task<IActionResult> Edit(UpdateAssetsDepartmentDto updateAssetsDepartmentDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsDepartmentDto != null)
            {
                await _assetDepartmentService.UpdateAssetDepartment(updateAssetsDepartmentDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect("/bo-phan.html");
            }
            return View(updateAssetsDepartmentDto);
        }
        [HttpGet]
        [Route("/xoa-bo-phan/{id}")]

        public async Task<IActionResult> Delete(AssetsDepartmentDto dto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetDepartment = await _assetDepartmentService.GetAssetDepartmentById(id);
            dto.Id = assetDepartment.Id;
            dto.Name = assetDepartment.Name;
            dto.Code = assetDepartment.Code;

            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-bo-phan/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetDepartmentService.DeleteAssetDepartment(id);
            _notyfService.Success("Xóa thành công");
            return Redirect("/bo-phan.html");
        }
    }
}
