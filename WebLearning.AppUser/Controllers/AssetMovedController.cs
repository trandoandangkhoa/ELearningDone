using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Extension;
using WebLearning.Contract.Dtos.Assets;

namespace WebLearning.AppUser.Controllers
{
    public class AssetMovedController : Controller
    {
        private readonly ILogger<AssetMovedController> _logger;
        private readonly IAssetMovedService _assetMovedService;
        private readonly IAssetService _assetService;
        private readonly IAccountService _accountService;
        private readonly IAssetDepartmentService _assetDepartmentService;
        public INotyfService _notyfService { get; }

        public AssetMovedController(ILogger<AssetMovedController> logger, IHttpClientFactory factory, IAssetMovedService assetMovedService, INotyfService notyfService, IAssetService assetService
            , IAccountService accountService, IAssetDepartmentService assetDepartmentService)
        {
            _logger = logger;
            _assetMovedService = assetMovedService;
            _notyfService = notyfService;
            _assetService = assetService;
            _accountService = accountService;
            _assetDepartmentService = assetDepartmentService;
        }
        public AssetMovedPrintView AssetMovedPrint
        {
            get
            {
                var assetMoved = HttpContext.Session.Get<AssetMovedPrintView>("AssetMovedPrintView");
                if (assetMoved == default(AssetMovedPrintView))
                {
                    assetMoved = new AssetMovedPrintView();
                }
                return assetMoved;
            }
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
            List<AssetMovedTicket> assetMovedTickets = new();

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
            var nameSender = await _accountService.GetFullName(User.Identity.Name);
            var newDep = await _assetDepartmentService.GetAssetDepartmentById(createAssetsMovedDto.AssetsDepartmentId);
            AssetMovedPrintView assetMovedPrintView = new AssetMovedPrintView();
            foreach (var item in createAssetsMovedDtos)
            {
                var assetName = await _assetService.GetAssetById(item.AssestsId);
                assetMovedTickets.Add(new AssetMovedTicket()
                {
                    Id = assetName.AssetId,
                    Name = assetName.AssetName,
                    Unit = "Cái",
                    Quantity = assetName.Quantity,
                    Status = assetName.AssetsStatusDto.Name,
                    Note = assetName.Note,
                });
                var oldDep = assetName.AssetsDepartmentDto.Name + ", ";
                assetMovedPrintView.OldDep.Add(oldDep);

                var res = await _assetMovedService.InsertAssetMoved(item);


            }
            assetMovedPrintView.Sender = nameSender.accountDetailDto.FullName;
            assetMovedPrintView.RoleSenderName = nameSender.roleDto.RoleName;
            assetMovedPrintView.NewDep = newDep.Name;

            assetMovedPrintView.Receiver = createAssetsMovedDto.Receiver;
            assetMovedPrintView.AssetMovedTickets = assetMovedTickets;
            assetMovedPrintView.ReasonMove = createAssetsMovedDtos[0].Description;

            HttpContext.Session.Set<AssetMovedPrintView>("AssetMovedPrintView", assetMovedPrintView);

            _notyfService.Success("Điều chuyển thành công!");
            return Redirect("/tai-san.html");
        }
        [Route("/cap-nhat-dieu-chuyen/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var asset = await _assetMovedService.GetAssetMovedById(id);
            UpdateAssetsMovedDto updateAssetsMovedDto = new()
            {
                AssetId = asset.AssetsId,
                AssetsDepartmentId = asset.AssetsDepartmentId,
                DateMoved = asset.DateMoved,
                DateUsed = asset.DateUsed,
                Description = asset.Description,
                SenderPhoneNumber = asset.SenderPhoneNumber,
                MovedStatus = asset.MovedStatus,
                Receiver = asset.Receiver,
                ReceiverPhoneNumber = asset.ReceiverPhoneNumber,
                NumBravo = asset.NumBravo,
                IdTemp = id,
            };
            var allDep = await _assetDepartmentService.GetAllAssetsDepartment();
            ViewData["Dep"] = new SelectList(allDep, "Id", "Name");
            return View(updateAssetsMovedDto);
        }
        [Route("/cap-nhat-dieu-chuyen/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id)
        {
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
        [Route("/in-tai-san-dieu-chuyen.html")]
        public Task<IActionResult> PrintAssetMoved(Guid id, string assetId)
        {
            var asset = HttpContext.Session.Get<AssetMovedPrintView>("AssetMovedPrintView");

            return Task.FromResult<IActionResult>(View(asset));

        }
    }
}
