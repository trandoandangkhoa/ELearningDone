using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Org.BouncyCastle.Asn1.Ocsp;
using PuppeteerSharp;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Extension;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Persistence.Migrations;

namespace WebLearning.AppUser.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IAssetDepartmentService _assetDepartmentService;
        private readonly IAssetStatusService _assetStatusService;

        public INotyfService _notyfService { get; }

        public AssetController(IAssetService assetService, INotyfService notyfService
                               , IAssetCategoryService assetCategoryService,IAssetDepartmentService assetDepartmentService, IAssetStatusService assetStatusService)
        {
            _assetService = assetService;
            _notyfService = notyfService;
            _assetCategoryService = assetCategoryService;
            _assetDepartmentService = assetDepartmentService;
            _assetStatusService = assetStatusService;
        }

        

        [Route("/tai-san.html")]
        public async Task<IActionResult> Index(string keyword, Guid[] assetsCategoryId, Guid[] assetsDepartmentId, int[] assetsStatusId,string url, bool disable, int pageSize, int pageIndex =1)
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
            if (disable == true) request.Active = false;

                if (keyword != null)
            {
                TempData["Keyword"] = keyword;
            }
            if (pageSize > 0)
            {
                TempData["PageSize"] = pageSize.ToString();

            }

            var Roke = await _assetService.GetPaging(request,assetsCategoryId,assetsDepartmentId,assetsStatusId,url);

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

            var Asset = await _assetService.GetAssetById(id);
            var allCat = await _assetCategoryService.GetAllAssetsCategory();
            var allDep = await _assetDepartmentService.GetAllAssetsDepartment();
            var allStatus = await _assetStatusService.GetAllAssetsStatus();

            ViewData["Cat"] = new SelectList(allCat, "Id", "Name");
            ViewData["Dep"] = new SelectList(allDep, "Id", "Name");
            ViewData["Status"] = new SelectList(allStatus, "Id", "Name");

            return View(Asset);
        }
        [HttpGet]
        [Route("/tao-moi-tai-san.html")]
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
        [HttpPost]
        public async Task<IActionResult> MoveAsset(string[] table_records)
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsset(string[] table_records)
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAssetsDto updateAssetsDto,string id)
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
            if(a == true)
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

        //[HttpGet]
        //[Route("/cap-nhat-tai-san/{id}")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (token == null)
        //    {
        //        return Redirect("/dang-nhap.html");
        //    }
        //    var result = await _assetService.GetAssetById(id);
        //    var updateResult = new UpdateAssetsDto()
        //    {
        //        Name = result.Name,
        //    };
        //    return View(updateResult);
        //}
        //[HttpPost]
        //[Route("/cap-nhat-tai-san/{id}")]
        //public async Task<IActionResult> Edit(UpdateAssetsDto updateAssetsDto, int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (token == null)
        //    {
        //        return Redirect("/dang-nhap.html");
        //    }
        //    if (updateAssetsDto != null)
        //    {
        //        await _assetService.UpdateAsset(updateAssetsDto, id);
        //        _notyfService.Success("Cập nhật thành công");

        //        return Redirect("/tai-san.html");
        //    }
        //    return View(updateAssetsDto);
        //}
        //[HttpGet]
        //[Route("/xoa-tai-san/{id}")]

        //public async Task<IActionResult> Delete(AssetsDto dto, int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (token == null)
        //    {
        //        return Redirect("/dang-nhap.html");
        //    }
        //    var asset = await _assetService.GetAssetById(id);
        //    dto.Id = asset.Id;
        //    dto.Name = asset.Name;
        //    return View(dto);
        //}
        //[HttpPost]
        //[Route("/xoa-tai-san/{id}")]

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (token == null)
        //    {
        //        return Redirect("/dang-nhap.html");
        //    }
        //    await _assetService.DeleteAsset(id);
        //    _notyfService.Success("Xóa thành công");
        //    return Redirect("/tai-san.html");
        //}
    }
}
