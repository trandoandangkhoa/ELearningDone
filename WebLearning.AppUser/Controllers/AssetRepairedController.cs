using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.Assets.Repair;

namespace WebLearning.AppUser.Controllers
{
    public class AssetRepairedController : Controller
    {
        private readonly ILogger<AssetRepairedController> _logger;
        private readonly IAssetRepairedService _assetRepairedService;
        private readonly IAssetService _assetService;
        private readonly IAccountService _accountService;
        private readonly IAssetDepartmentService _assetDepartmentService;
        public INotyfService _notyfService { get; }

        public AssetRepairedController(ILogger<AssetRepairedController> logger, IHttpClientFactory factory, IAssetRepairedService assetRepairedService, INotyfService notyfService, IAssetService assetService
            , IAccountService accountService, IAssetDepartmentService assetDepartmentService)
        {
            _logger = logger;
            _assetRepairedService = assetRepairedService;
            _notyfService = notyfService;
            _assetService = assetService;
            _accountService = accountService;
            _assetDepartmentService = assetDepartmentService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(string table_records, CreateAssetsRepairedDto createAssetsRepairedDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetId = table_records.Split(",");
            TempData["LocationRepaired"] = createAssetsRepairedDto.LocationRepaired;
            TempData["DateRepaired"] = createAssetsRepairedDto.DateRepaired;

            List<CreateAssetsRepairedDto> createAssetsRepairedDtos = new();

            for (var i = 0; i < assetId.Length; i++)
            {
                createAssetsRepairedDtos.Add(new CreateAssetsRepairedDto
                {
                    Id = Guid.NewGuid(),

                    AssestsId = assetId[i],
                    DateRepaired = createAssetsRepairedDto.DateRepaired,
                    LocationRepaired = createAssetsRepairedDto.LocationRepaired,
                });

            }
            foreach (var item in createAssetsRepairedDtos)
            {
                if (string.IsNullOrEmpty(item.AssestsId))
                {
                    _notyfService.Error("Mã tài sản  bạn còn trống");
                    return Redirect("/tai-san.html");

                }

                var res = await _assetRepairedService.InsertAssetRepaired(item);


            }


            _notyfService.Success("Thêm thành công!");
            return Redirect("/tai-san.html");
        }
        [Route("/cap-nhat-thong-tin-sua-chua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var asset = await _assetRepairedService.GetAssetRepairedById(id);
            UpdateAssetsRepairedDto updateAssetsRepairedDto = new()
            {
                AssetsId = asset.AssestsId,
                DateRepaired = asset.DateRepaired,
                LocationRepaired = asset.LocationRepaired,

            };
            return View(updateAssetsRepairedDto);
        }
        [Route("/cap-nhat-thong-tin-sua-chua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAssetsRepairedDto updateAssetsRepairedDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsRepairedDto != null)
            {
                await _assetRepairedService.UpdateAssetRepaired(updateAssetsRepairedDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect($"/chi-tiet-tai-san/{updateAssetsRepairedDto.AssetsId}");
            }
            return Redirect($"/chi-tiet-tai-san/{updateAssetsRepairedDto.AssetsId}");
        }
        [Route("/chi-tiet-tai-san/{assetId}/xoa-dieu-chuyen/{id}")]
        public async Task<IActionResult> Delete(Guid id, string assetId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetRepairedService.DeleteAssetRepaired(id);
            _notyfService.Success("Xóa thành công");
            return Redirect($"/chi-tiet-tai-san/{assetId}");
        }

    }
}
