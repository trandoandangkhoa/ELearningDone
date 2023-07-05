using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;

namespace WebLearning.AppUser.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IAssetDepartmentService _assetDepartmentService;
        private readonly IAssetStatusService _assetStatusService;
        private readonly IAssetMovedService _assetMoveService;
        private readonly IAssetSupplierService _assetSupplierService;
        public INotyfService _notyfService { get; }


        public AssetController(IAssetService assetService, INotyfService notyfService
                               , IAssetCategoryService assetCategoryService, IAssetDepartmentService assetDepartmentService, IAssetStatusService assetStatusService, IAssetMovedService assetMoveService, IAssetSupplierService assetSupplierService)
        {
            _assetService = assetService;
            _notyfService = notyfService;
            _assetCategoryService = assetCategoryService;
            _assetDepartmentService = assetDepartmentService;
            _assetStatusService = assetStatusService;
            _assetMoveService = assetMoveService;
            _assetSupplierService = assetSupplierService;
        }

        [Route("/tai-san.html")]
        public async Task<IActionResult> Index(string keyword, Guid[] assetsCategoryId, Guid[] assetsDepartmentId, int[] assetsStatusId, string url, bool active, int pageSize, int pageIndex = 1)
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
                PageSize = pageSize,
                Active = true,
            };
            if (active == true) request.Active = false;

            if (keyword != null)
            {
                TempData["Keyword"] = keyword;
            }
            if (pageSize > 0)
            {
                TempData["PageSize"] = pageSize.ToString();

            }
            //var allDep = await _assetDepartmentService.GetAllAssetsDepartment();

            //ViewData["Dep"] = new SelectList(allDep, "Id", "Name");
            var Roke = await _assetService.GetPaging(request, assetsCategoryId, assetsDepartmentId, assetsStatusId);

            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return Redirect("/dang-nhap.html");
            }

            return View(Roke);

        }
        [Route("/chi-tiet-tai-san/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var asset = await _assetService.GetAssetById(id);

            var allCat = await _assetCategoryService.GetAllAssetsCategory();
            var allDep = await _assetDepartmentService.GetAllAssetsDepartment();
            var allStatus = await _assetStatusService.GetAllAssetsStatus();
            var allSup = await _assetSupplierService.GetAllAssetsSupplier();

            ViewData["Cat"] = new SelectList(allCat, "Id", "Name");
            ViewData["Dep"] = new SelectList(allDep, "Id", "Name");
            ViewData["Status"] = new SelectList(allStatus, "Id", "Name");
            ViewData["Supplier"] = new SelectList(allSup, "Id", "CompanyName");

            return View(asset);
        }
        [HttpGet]
        [Route("/tao-moi-tai-san.html")]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allCat = await _assetCategoryService.GetAllAssetsCategory();
            var allDep = await _assetDepartmentService.GetAllAssetsDepartment();
            var allStatus = await _assetStatusService.GetAllAssetsStatus();
            var allSup = await _assetSupplierService.GetAllAssetsSupplier();
            ViewData["Cat"] = new SelectList(allCat, "Id", "Name");
            ViewData["Dep"] = new SelectList(allDep, "Id", "Name");
            ViewData["Status"] = new SelectList(allStatus, "Id", "Name");
            ViewData["Supplier"] = new SelectList(allSup, "Id", "CompanyName");

            return View();
        }
        [HttpPost]
        [Route("/tao-moi-tai-san.html")]
        public async Task<IActionResult> Create(CreateAssetsDto createAssetDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAssetDto != null)
            {
                await _assetService.InsertAsset(createAssetDto);
                _notyfService.Success("Tạo thành công");

                return Redirect("/tai-san.html");

            }
            return View(createAssetDto);
        }
        public async Task<ActionResult> Export(string address, string querySearch, Guid[] assetsCategoryId, Guid[] assetsDepartmentId, int[] assetsStatusId, string url, bool active, int pageSize, int pageIndex = 1)
        {

            var token = HttpContext.Session.GetString("Token");

            var export = await _assetService.Export(querySearch);

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Tài sản còn hoạt động");
                sheet.Cells[1, 1].Value = "Id";
                sheet.Cells[1, 2].Value = "Mã tài sản";
                sheet.Cells[1, 3].Value = "Tên tài sản";
                sheet.Cells[1, 4].Value = "Mã hóa đơn";
                sheet.Cells[1, 5].Value = "Số seri";
                sheet.Cells[1, 6].Value = "Đơn Giá";
                sheet.Cells[1, 7].Value = "Nhà cung cấp";
                sheet.Cells[1, 8].Value = "Loại";
                sheet.Cells[1, 9].Value = "Model";
                sheet.Cells[1, 10].Value = "Số lượng";
                sheet.Cells[1, 11].Value = "Đơn vị sử dụng";
                sheet.Cells[1, 12].Value = "Người sử dụng";
                sheet.Cells[1, 13].Value = "Đơn vị quản lí";
                sheet.Cells[1, 14].Value = "Tình trạng";
                sheet.Cells[1, 15].Value = "Thông số";
                sheet.Cells[1, 16].Value = "Ghi chú";
                sheet.Cells[1, 17].Value = "Ngày đưa";
                sheet.Cells[1, 18].Value = "Ngày di chuyển";
                sheet.Cells[1, 19].Value = "Ngày kiểm kê";
                sheet.Cells[1, 20].Value = "Ngày mua";
                sheet.Cells[1, 21].Value = "TG bảo hành";
                sheet.Cells[1, 22].Value = "Hoạt động";

                int rowInd = 2;

                foreach (var lo in export)
                {
                    sheet.Cells[rowInd, 1].Value = lo.Id;
                    sheet.Cells[rowInd, 2].Value = lo.AssetId;
                    sheet.Cells[rowInd, 3].Value = lo.AssetName;
                    sheet.Cells[rowInd, 4].Value = lo.OrderNumber;
                    sheet.Cells[rowInd, 5].Value = lo.SeriNumber;
                    sheet.Cells[rowInd, 6].Value = lo.Price;
                    //sheet.Cells[rowInd, 7].Value = lo.Supplier;
                    sheet.Cells[rowInd, 8].Value = lo.AssetsCategoryDto.Name;
                    sheet.Cells[rowInd, 9].Value = lo.AssetSubCategory;
                    sheet.Cells[rowInd, 10].Value = lo.Quantity;
                    sheet.Cells[rowInd, 11].Value = lo.AssetsDepartmentDto.Name;
                    sheet.Cells[rowInd, 12].Value = lo.Customer;
                    sheet.Cells[rowInd, 13].Value = lo.Manager;
                    sheet.Cells[rowInd, 14].Value = lo.AssetsStatusDto.Name;
                    sheet.Cells[rowInd, 15].Value = lo.Spec;
                    sheet.Cells[rowInd, 16].Value = lo.Note;
                    sheet.Cells[rowInd, 17].Value = lo.DateUsed?.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[rowInd, 18].Value = lo.DateMoved?.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[rowInd, 19].Value = lo.DateChecked.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[rowInd, 20].Value = lo.DateBuyed?.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[rowInd, 21].Value = lo.ExpireDay;
                    if (lo.Active == true)
                    {
                        sheet.Cells[rowInd, 22].Value = "Hoạt động";

                    }
                    else
                    {
                        sheet.Cells[rowInd, 22].Value = "Không hoạt động";

                    }
                    rowInd++;
                }
                package.Save();
            }

            stream.Position = 0;

            var fileName = $"Asset_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsset(string table_records)
        {
            var assetId = table_records.Split(",");
            for (var i = 0; i < assetId.Length; i++)
            {
                var rs = await _assetService.DeleteAsset(assetId[i]);

            }
            _notyfService.Success("Xóa thành công!");
            return Redirect("/tai-san.html");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAssetsDto updateAssetsDto, string id)
        {

            if (updateAssetsDto.AssetsCategoryId == Guid.Empty)
            {
                TempData["ErrorCat"] = "Vui lòng chọn loại tài sản !";
            }

            if (updateAssetsDto.AssetsDepartmentId == Guid.Empty)
            {
                TempData["ErrorDep"] = "Vui lòng chọn đơn vị sử dụng !";
            }
            if (updateAssetsDto.AssetsStatusId == 0)
            {
                TempData["ErrorStatus"] = "Vui lòng chọn tình trạng tài sản !";
            }
            var a = await _assetService.UpdateAsset(updateAssetsDto, id);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");

            }

            return Redirect($"/chi-tiet-tai-san/{id}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var a = await _assetService.DeleteAsset(id);

            if (a == true)
            {
                _notyfService.Success("Xóa thành công");

            }

            return Redirect($"/tai-san.html");
        }
        [HttpPost]
        public async Task<IActionResult> GetItem(string[] id)
        {

            var rs = await _assetService.GetAllAssets();
            rs = rs.Where(x => id.Contains(x.Id));
            return View(rs);
        }
    }
}
