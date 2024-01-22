using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Extension;
using WebLearning.Contract.Dtos.Assets.Moved;

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
            string[] checkedDupplicate = table_records.Split(",").ToArray().Distinct().ToArray();

            string result = "";
            TempData["Receiver"] = createAssetsMovedDto.Receiver;
            TempData["ReceiverPhoneNumber"] = createAssetsMovedDto.ReceiverPhoneNumber;
            TempData["SenderPhoneNumber"] = createAssetsMovedDto.SenderPhoneNumber;
            TempData["Description"] = createAssetsMovedDto.Description;
            TempData["DateUsed"] = createAssetsMovedDto.DateUsed;

            if (createAssetsMovedDto.NewDepartmentId == Guid.Empty)
            {
                _notyfService.Error("Mã bộ phân bạn còn trống");
                return Redirect("/tai-san.html");

            }
            var a = (await _assetService.GetAllAssetsSelected(checkedDupplicate)).Where(x => x.Active == false || x.Quantity < createAssetsMovedDto.Quantity);

            if (a != null && a.Any())
            {
                foreach (var er in a)
                {
                    result += er.Id.ToString() + ", ";
                }
                _notyfService.Error("Mã: " + result + "không hợp lệ", 7);

                return Redirect("/tai-san.html");
            }


            AssetMovedView assetMovedView = new();

            for (var i = 0; i < checkedDupplicate.Length; i++)
            {
                assetMovedView.createAssetsMovedDtos.Add(new CreateAssetsMovedDto
                {
                    OldAssestsId = checkedDupplicate[i],
                    Receiver = createAssetsMovedDto.Receiver,
                    ReceiverPhoneNumber = createAssetsMovedDto.ReceiverPhoneNumber,
                    SenderPhoneNumber = createAssetsMovedDto.SenderPhoneNumber,
                    Description = createAssetsMovedDto.Description,
                    Quantity = createAssetsMovedDto.Quantity,
                    DateUsed = createAssetsMovedDto.DateUsed,
                    NewDepartmentId = createAssetsMovedDto.NewDepartmentId,
                    Unit = "Cái",
                });

            }
            var nameSender = await _accountService.GetFullName(User.Identity.Name);
            //var newDep = await _assetDepartmentService.GetAssetDepartmentById(createAssetsMovedDto.AssetsDepartmentId);                
            var code = await _assetMovedService.GetPrintCode();
            foreach (var item in assetMovedView.createAssetsMovedDtos)
            {

                item.Code = code;
                //var assetName = await _assetService.GetAssetByIdForMoving(item.OldAssestsId);
                //assetMovedView.assetMovedTickets.Add(new AssetMovedTicket()
                //{
                //    Id = assetName.AssetId,
                //    Name = assetName.AssetName,
                //    Unit = "Cái",
                //    Quantity = assetName.Quantity,
                //    Status = assetName.AssetsStatusDto.Name,
                //    Note = assetName.Note,
                //});
                //var oldDep = assetName.AssetsDepartmentDto.Name + ", ";
                //assetMovedPrintView.OldDep.Add(oldDep);

                var res = await _assetMovedService.InsertAssetMoved(item);
                if (res != "Cập nhật thành công!")
                {
                    _notyfService.Error(res);
                    return Redirect("/tai-san.html");
                }

            }

            //assetMovedPrintView.Sender = nameSender.accountDetailDto.FullName;
            //assetMovedPrintView.RoleSenderName = nameSender.roleDto.RoleName;
            //assetMovedPrintView.NewDep = newDep.Name;

            //assetMovedPrintView.Receiver = createAssetsMovedDto.Receiver;
            //assetMovedPrintView.AssetMovedTickets = assetMovedView.assetMovedTickets;
            //assetMovedPrintView.ReasonMove = assetMovedView.createAssetsMovedDtos[0].Description;

            //HttpContext.Session.Set<AssetMovedPrintView>("AssetMovedPrintView", assetMovedPrintView);

            _notyfService.Success("Cập nhật thành công!");
            return Redirect("/tai-san.html");
        }
        [Route("/cap-nhat-dieu-chuyen/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var asset = await _assetMovedService.GetAssetMovedById(id);
            UpdateAssetsMovedDto updateAssetsMovedDto = new()
            {
                OldAssestsId = asset.OldAssestsId,
                AssestsId = asset.AssestsId,
                NewDepartmentId = asset.NewDepartmentId,
                DateMoved = asset.DateMoved,
                DateUsed = asset.DateUsed,
                Description = asset.Description,
                SenderPhoneNumber = asset.SenderPhoneNumber,
                AssetsMovedStatusId = asset.AssetsMovedStatusId,
                Receiver = asset.Receiver,
                ReceiverPhoneNumber = asset.ReceiverPhoneNumber,
                Quantity = asset.Quantity,
                //NumBravo = asset.NumBravo,
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

                return Redirect($"/chi-tiet-tai-san/{updateAssetsMovedDto.OldAssestsId}");
            }
            return Redirect($"/chi-tiet-tai-san/{updateAssetsMovedDto.OldAssestsId}");
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
        [Route("/lich-su-dieu-chuyen.html")]
        public async Task<IActionResult> HistoryMoved()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var a = await _assetMovedService.GetAllAssetMovedsHistory();

            return View(a);
        }
        [Route("/in-tai-san-dieu-chuyen/{id}")]
        public async  Task<IActionResult> PrintAssetMoved(string id)
        {
            var asset = await _assetMovedService.GetAssetMovedPrintView(id, User.Identity.Name);

            return View(asset);

        }
    }
}
