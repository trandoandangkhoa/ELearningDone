using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;

namespace WebLearning.AppUser.Controllers
{
    public class AssetMovedController : Controller
    {
        private readonly ILogger<AssetMovedController> _logger;
        private readonly IAssetMovedService _assetMovedService;
        public INotyfService _notyfService { get; }

        public AssetMovedController(ILogger<AssetMovedController> logger, IHttpClientFactory factory, IAssetMovedService assetMovedService, INotyfService notyfService)
        {
            _logger = logger;
            _assetMovedService = assetMovedService;
            _notyfService = notyfService;
        }
        [HttpPost]
        public async Task<IActionResult> MoveAsset(string table_records, CreateAssetsMovedDto createAssetsMovedDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var assetId = table_records.Split(",");
            TempData["Receiver"] = createAssetsMovedDto.Receiver;
            TempData["ReceiverPhoneNumber"] = createAssetsMovedDto.ReceiverPhoneNumber;
            TempData["SenderPhoneNumber"] = createAssetsMovedDto.SenderPhoneNumber;
            TempData["Description"] = createAssetsMovedDto.Description;
            TempData["DateUsed"] = createAssetsMovedDto.DateUsed;

            if (createAssetsMovedDto.AssetsDepartmentId == Guid.Empty)
            {
                _notyfService.Error("Mã bộ phân bạn còn trống");
                return Redirect("/tai-san.html");

            }
            List<CreateAssetsMovedDto> createAssetsMovedDtos = new();
            for (var i = 0; i < assetId.Length; i++)
            {
                createAssetsMovedDtos.Add(new CreateAssetsMovedDto
                {
                    AssestsId = assetId[i],
                    Receiver = createAssetsMovedDto.Receiver,
                    ReceiverPhoneNumber = createAssetsMovedDto.ReceiverPhoneNumber,
                    SenderPhoneNumber = createAssetsMovedDto.SenderPhoneNumber,
                    Description = createAssetsMovedDto.Description,
                    DateUsed = createAssetsMovedDto.DateUsed,
                    AssetsDepartmentId = createAssetsMovedDto.AssetsDepartmentId,
                });

            }
            foreach (var item in createAssetsMovedDtos)
            {
                var res = await _assetMovedService.InsertAssetMoved(item);

            }

            _notyfService.Success("Điều chuyển thành công!");
            return Redirect("/tai-san.html");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id)
        {
            id = updateAssetsMovedDto.IdTemp;
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAssetsMovedDto != null)
            {
                await _assetMovedService.UpdateAssetMoved(updateAssetsMovedDto, id);
                _notyfService.Success("Cập nhật thành công");

                return Redirect($"/chi-tiet-tai-san/{updateAssetsMovedDto.AssetId}");
            }
            return Redirect($"/chi-tiet-tai-san/{updateAssetsMovedDto.AssetId}");
        }
        [Route("/chi-tiet-tai-san/{assetId}/xoa-dieu-chuyen/{id}")]
        public async Task<IActionResult> Delete(Guid id, string assetId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _assetMovedService.DeleteAssetMoved(id);
            _notyfService.Success("Xóa thành công");
            return Redirect($"/chi-tiet-tai-san/{assetId}");
        }
    }
}
