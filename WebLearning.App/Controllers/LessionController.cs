using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class LessionController : Controller
    {
        private readonly ILogger<LessionController> _logger;

        private readonly ILessionService _lessionService;

        private readonly ICourseService _courseService;

        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        public INotyfService _notyfService { get; }
        public LessionController(ILogger<LessionController> logger, IHttpClientFactory factory, ILessionService lessionService, INotyfService notyfService, ICourseService courseService
                                , IRoleService roleService, IAccountService accountService)
        {
            _logger = logger;
            _courseService = courseService;
            _notyfService = notyfService;
            _lessionService = lessionService;
            _roleService = roleService;
            _accountService = accountService;
        }
        [Route("/chuong.html", Name = "LessionIndex")]
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
            var Roke = await _lessionService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }
            var allCourse = await _courseService.GetAllCourse();

            ViewData["DanhMuc"] = new SelectList(allCourse, "Id", "Name");

            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}", Name = "LessionDetail")]
        public async Task<IActionResult> Detail(Guid id, string Alias)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _lessionService.GetLessionById(id);

            var allLession = await _lessionService.GetAllLession();

            ViewData["DanhMuc"] = new SelectList(allLession, "Id", "Name");
            TempData["LessionId"] = id.ToString();
            TempData["AliasLession"] = Alias.ToString();
            return View(role);
        }


        //CREATE LESSION
        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/tao-moi-chuong.html", Name = "Create")]
        public async Task<IActionResult> Create(Guid CourseId, string AliasCourse)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allCourse = await _courseService.GetAllCourse();

            List<SelectListItem> list = new();

            foreach (var item in allCourse)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }

            ViewBag.Notification = list;

            var model = new CreateLessionAdminView();
            model.CreateLessionVideoDto = new CreateLessionVideoDto();
            model.CreateLessionFileDocumentDto = new CreateLessionFileDocumentDto();

            TempData["CourseId"] = CourseId.ToString();
            TempData["AliasCourse"] = AliasCourse.ToString();

            return View(model);
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/tao-moi-chuong.html", Name = "Create")]
        public async Task<IActionResult> Create(string LinkVideo, string AliasCourse, Guid CourseId, [FromForm] CreateLessionAdminView createLessionAdminView, IFormFile video, IFormFile document)
        {

            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            if (createLessionAdminView != null)
            {

                CreateLessionValidation obj = new();

                ValidationResult validationResult = obj.Validate(createLessionAdminView);

                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                    }
                    var allCourse = await _courseService.GetAllCourse();

                    List<SelectListItem> list = new();

                    foreach (var item in allCourse)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Id.ToString(),
                        });
                    }

                    ViewBag.Notification = list;
                    return View("Create", createLessionAdminView);
                };
                if (video != null)
                {

                    createLessionAdminView.CreateLessionVideoDto.ImageFile = video;

                }
                if (LinkVideo != null)
                {
                    createLessionAdminView.CreateLessionVideoDto.LinkVideo = LinkVideo;
                }
                if (document != null)
                {
                    createLessionAdminView.CreateLessionFileDocumentDto.FileDocument = document;
                }

                createLessionAdminView.CreateLessionDto.Author = User.Identity.Name;

                if (createLessionAdminView.CreateLessionDto.CourseId == Guid.Empty)
                {
                    createLessionAdminView.CreateLessionDto.CourseId = CourseId;
                }
                var checkRole = await _courseService.GetCourseById(createLessionAdminView.CreateLessionDto.CourseId);

                var accountId = await _accountService.GetFullName(User.Identity.Name);

                if (accountId.RoleId.Equals(checkRole.CreatedBy) == false)
                {
                    _notyfService.Error("Bạn không thể mới chương cho khóa học này");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/tao-moi-chuong.html");

                }
                var result = await _lessionService.InsertLession(createLessionAdminView);

                if (result == true)
                {
                    _notyfService.Success("Tạo thành công");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/tao-moi-chuong.html");
                }

                _notyfService.Error("Tạo không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/tao-moi-chuong.html");

            }
            return View(createLessionAdminView);
        }



        //EDIT LESSION

        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/cap-nhat-chuong/{Alias}/{id}/", Name = "Edit")]
        public async Task<IActionResult> Edit(Guid id, Guid CourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _lessionService.GetLessionById(id);

            var updateResult = new UpdateLessionDto()
            {
                Name = result.Name,

                Active = result.Active,

                ShortDesc = result.ShortDesc,

                DateCreated = result.DateCreated,

                Alias = result.Alias,


                Notify = result.Notify,

                DescNotify = result.DescNotify,

                Author = result.Author,
            };
            TempData["CourseId"] = CourseId.ToString();
            return View(updateResult);
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/cap-nhat-chuong/{Alias}/{id}/", Name = "Edit")]
        public async Task<IActionResult> Edit([FromForm] UpdateLessionDto updateLessionDto, Guid id, string AliasCourse, Guid CourseId)
        {
            UpdateLessionValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateLessionDto);
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }

                var allCourse = await _courseService.GetAllCourse();

                List<SelectListItem> list = new();

                foreach (var item in allCourse)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                    });
                }

                ViewBag.Notification = list;
                return View("Edit", updateLessionDto);
            };
            if (updateLessionDto != null)
            {
                if (User.Identity.Name == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                updateLessionDto.Author = User.Identity.Name;

                var result = await _lessionService.UpdateLession(updateLessionDto, id);

                if (result == true)
                {
                    _notyfService.Success("Cập nhật thành công");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
            }
            TempData["CourseId"] = CourseId.ToString();
            TempData["AliasCourse"] = AliasCourse.ToString();
            return View(updateLessionDto);

        }




        //DELETE VIDEO

        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/xoa-chuong/{Alias}/{id}/", Name = "Delete")]
        public async Task<IActionResult> Delete(LessionDto lessionDto, Guid id, Guid CourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allRole = await _courseService.GetAllCourse();
            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");
            var role = await _lessionService.GetLessionById(id);

            lessionDto.Id = role.Id;

            lessionDto.Name = role.Name;


            lessionDto.Active = role.Active;

            lessionDto.CourseDto = role.CourseDto;

            lessionDto.DateCreated = role.DateCreated;

            lessionDto.Alias = role.Alias;

            lessionDto.LessionVideoDtos = role.LessionVideoDtos;

            lessionDto.Author = role.Author;

            lessionDto.ShortDesc = role.ShortDesc;
            lessionDto.Code = role.Code;
            TempData["CourseId"] = CourseId.ToString();
            return View(lessionDto);
        }


        [HttpPost]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/xoa-chuong/{Alias}/{id}/", Name = "Delete")]
        public async Task<IActionResult> Delete(string AliasCourse, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _lessionService.DeleteLession(id);

            _notyfService.Success("Xóa thành công");
            string CourseId = TempData["CourseId"].ToString();
            return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
        }



        //EDIT VIDEO


        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/cap-nhat-bai-giang.html", Name = "EditVideo")]
        public async Task<IActionResult> EditVideo(Guid id, Guid CourseId, string AliasCourse, string Alias)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _lessionService.GetLessionVideoById(id);


            var updateResult = new UpdateLessionVideoDto()
            {
                Caption = result.Caption,

                SortOrder = result.SortOrder,

                LinkVideo = result.LinkVideo,



            };
            TempData["CourseId"] = CourseId.ToString();
            TempData["AliasCourse"] = AliasCourse.ToString();
            TempData["LessionId"] = result.LessionId.ToString();
            TempData["AliasLession"] = result.Alias.ToString();
            return View(updateResult);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/cap-nhat-bai-giang.html", Name = "EditVideo")]
        public async Task<IActionResult> EditVideo([FromForm] UpdateLessionVideoDto updateLessionVideoDto, string Alias, Guid id, IFormFile fThumb, Guid CourseId, string AliasCourse)
        {
            if (updateLessionVideoDto != null)
            {
                string AliasLession = null;
                string LessionId = null;
                if (TempData.ContainsKey("AliasLession") && TempData.ContainsKey("LessionId"))
                {
                    AliasLession = TempData["AliasLession"].ToString();
                    LessionId = TempData["LessionId"].ToString();
                }

                if (fThumb == null && updateLessionVideoDto.LinkVideo == null)
                {
                    _notyfService.Error("Vui lòng chọn video bài giảng!");

                    return RedirectToRoute("EditVideo");
                }
                else
                {
                    var token = HttpContext.Session.GetString("Token");

                    if (token == null)
                    {
                        return Redirect("/dang-nhap.html");
                    }
                    if (fThumb != null)
                    {
                        updateLessionVideoDto.ImageFile = fThumb;


                        var result = await _lessionService.UpdateImage(id, updateLessionVideoDto);

                        if (result == true)
                        {
                            _notyfService.Success("Cập nhật thành công");

                            return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                        }

                        _notyfService.Error("Cập nhật không thành công!");

                        _notyfService.Warning("Vui lòng điền đủ các giá trị");

                        return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                    }
                    var linkVideo = await _lessionService.UpdateImage(id, updateLessionVideoDto);

                    if (linkVideo == true)
                    {
                        _notyfService.Success("Cập nhật thành công");

                        return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                    }

                    _notyfService.Error("Cập nhật không thành công!");

                    _notyfService.Warning("Vui lòng điền đủ các giá trị");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                }

            }
            return View(updateLessionVideoDto);
        }



        //DELETE VIDEO LESSION AND FILE DOCUMENT


        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/xoa-bai-giang.html", Name = "DeleteVideo")]
        public async Task<IActionResult> DeleteVideo(LessionVideoDto lessionVideoDto, string Alias, Guid id, Guid CourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _lessionService.GetLessionVideoById(id);

            lessionVideoDto.Id = result.Id;

            lessionVideoDto.LessionId = result.LessionId;

            lessionVideoDto.Caption = result.Caption;

            lessionVideoDto.ImagePath = result.ImagePath;

            lessionVideoDto.DateCreated = result.DateCreated;

            lessionVideoDto.SortOrder = result.SortOrder;

            lessionVideoDto.FileSize = result.FileSize;

            lessionVideoDto.Code = result.Code;

            TempData["CourseId"] = CourseId.ToString();
            TempData["LessionId"] = result.LessionId.ToString();
            TempData["AliasLession"] = result.Alias.ToString();
            return View(lessionVideoDto);

        }

        [HttpPost]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/xoa-bai-giang.html", Name = "DeleteVideo")]
        public async Task<IActionResult> DeleteVideo(Guid id, Guid CourseId, string AliasCourse, string Alias)
        {
            var token = HttpContext.Session.GetString("Token");
            string LessionId = TempData["LessionId"].ToString();
            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _lessionService.RemoveImage(id);

            _notyfService.Success("Xóa video thành công");

            return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{LessionId}");

        }
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/xoa-tai-lieu/{imgId}")]
        public async Task<IActionResult> DeleteDocument(Guid imgId, Guid CourseId, string AliasCourse)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _lessionService.RemoveDocument(imgId);

            _notyfService.Success("Xóa tài liệu thành công");

            return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");

        }

        //ADD VIDEO LESSION


        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/tao-moi-bai-giang.html", Name = "CreateVideo")]
        public async Task<IActionResult> CreateVideo(Guid id, Guid CourseId, string AliasCourse, string Alias)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allLession = await _lessionService.GetAllLession();

            if (allLession.Count() == 1)
            {
                _notyfService.Warning("Bài học của bạn chỉ có một, vui lòng thêm bài học");
                return View();
            }
            TempData["CourseId"] = CourseId.ToString();
            TempData["AliasCourse"] = AliasCourse.ToString();
            TempData["AliasLession"] = Alias.ToString();
            TempData["LessionId"] = id.ToString();
            return View();
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/tao-moi-bai-giang.html", Name = "CreateVideo")]
        public async Task<IActionResult> CreateVideo(Guid id, [FromForm] CreateLessionVideoDto createLessionVideoDto, IFormFile fThumb, Guid CourseId, string AliasCourse, string Alias)
        {
            if (createLessionVideoDto != null)
            {
                if (fThumb == null && createLessionVideoDto.LinkVideo == null)
                {
                    _notyfService.Error("Vui lòng chọn video bài giảng!");

                    return RedirectToRoute("CreateVideo");
                }
                else
                {
                    var token = HttpContext.Session.GetString("Token");

                    if (token == null)
                    {
                        return Redirect("/dang-nhap.html");
                    }
                    createLessionVideoDto.LessionId = id;

                    if (fThumb != null)
                    {
                        createLessionVideoDto.ImageFile = fThumb;


                        var fileUpload = await _lessionService.AddImage(id, createLessionVideoDto);
                        if (fileUpload == true)
                        {
                            _notyfService.Success("Thêm thành công");

                            return RedirectToRoute("CreateVideo");
                        }

                        _notyfService.Error("Thêm không thành công!");

                        _notyfService.Warning("Vui lòng điền đủ các giá trị");

                        return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/tao-moi-bai-giang.html");
                    }
                    var linkVideo = await _lessionService.AddImage(id, createLessionVideoDto);

                    if (linkVideo == true)
                    {
                        _notyfService.Success("Thêm thành công");

                        return RedirectToRoute("CreateVideo");
                    }

                    _notyfService.Error("Thêm không thành công!");

                    _notyfService.Warning("Vui lòng điền đủ các giá trị");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/tao-moi-bai-giang.html");
                }


            }
            return View(createLessionVideoDto);
        }


        //ADD FILE DOCUMENT

        [HttpGet]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/them-tai-lieu-tham-khao.html", Name = "CreateFileDocument")]
        public async Task<IActionResult> CreateFileDocument(Guid id, Guid CourseId, string AliasCourse)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allLession = await _lessionService.GetAllLession();
            if (allLession.Count() == 1)
            {
                _notyfService.Warning("Bài học của bạn chỉ có một, vui lòng thêm bài học");
                return View();
            }
            ViewData["DanhMuc"] = new SelectList(allLession, "Id", "Name");
            TempData["CourseId"] = CourseId.ToString();
            TempData["AliasCourse"] = AliasCourse.ToString();
            return View();
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}/them-tai-lieu-tham-khao.html", Name = "CreateFileDocument")]
        public async Task<IActionResult> CreateFileDocument(Guid id, [FromForm] CreateLessionFileDocumentDto createLessionFileDocumentDto, IFormFile fThumb, Guid CourseId, string AliasCourse, string Alias)
        {
            if (createLessionFileDocumentDto != null)
            {
                createLessionFileDocumentDto.FileDocument = fThumb;
                var token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                var result = await _lessionService.AddDocument(id, createLessionFileDocumentDto);
                if (result == true)
                {
                    _notyfService.Success("Thêm thành công");

                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/chi-tiet-chuong/{Alias}/{id}");
                }

                _notyfService.Error("Thêm không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");

            }
            return View(createLessionFileDocumentDto);
        }
    }
}
