using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Course.CourseAdminView;
using WebLearning.Contract.Dtos.Lession;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseService _courseService;
        private readonly IAccountService _accountService;
        public INotyfService _notyfService { get; }

        public CourseController(ILogger<CourseController> logger, ICourseService courseService, INotyfService notyfService, IAccountService accountService)
        {
            _logger = logger;
            _courseService = courseService;
            _notyfService = notyfService;
            _accountService = accountService;
        }
        [Route("/khoa-hoc.html", Name = "CourseIndex")]
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

            var Roke = await _courseService.GetPaging(request);
            if (Roke == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View(Roke);
        }
        [Route("/chi-tiet-khoa-hoc/{Alias}/{id}", Name = "CourseDetail")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var course = await _courseService.GetCourseById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(course.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/khoa-hoc.html");

            }
            return View(course);
        }
        [HttpGet]
        [Route("/tao-moi-khoa-hoc.html", Name = "CourseCreate")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-khoa-hoc.html", Name = "CourseCreate")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateCourseDto createCourseDto, IFormFile fThumb, Guid notificationId)
        {

            CreateCourseValidation obj = new();


            createCourseDto.Image = fThumb;

            ValidationResult validationResult = obj.Validate(createCourseDto);


            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }

                return View("Create", createCourseDto);
            };
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var accountId = await _accountService.GetFullName(User.Identity.Name);

            createCourseDto.CreatedBy = accountId.RoleId;

            var result = await _courseService.InsertCourse(createCourseDto);


            if (result != true)
            {
                _notyfService.Error("Thêm không thành công!");

                return RedirectToAction("Index", "Course");
            }
            _notyfService.Success("Thêm thành công!");

            return RedirectToAction("Index", "Course");




        }
        [HttpGet]
        [Route("/cap-nhat-khoa-hoc/{Alias}/{id}", Name = "CourseEdit")]

        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var result = await _courseService.GetCourseById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/khoa-hoc.html");

            }
            UpdateCourseAdminView updateCourseAdminView = new();

            updateCourseAdminView.UpdateCourseDto.Name = result.Name;

            updateCourseAdminView.UpdateCourseDto.Active = result.Active;

            updateCourseAdminView.UpdateCourseDto.Alias = result.Alias;

            updateCourseAdminView.UpdateCourseDto.DescNotify = result.DescNotify;

            updateCourseAdminView.UpdateCourseDto.Notify = result.Notify;

            updateCourseAdminView.UpdateCourseDto.Description = result.Description;

            updateCourseAdminView.UpdateCourseDto.DateCreated = result.DateCreated;

            return View(updateCourseAdminView);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("/cap-nhat-khoa-hoc/{Alias}/{id}", Name = "CourseEdit")]

        public async Task<IActionResult> Edit([FromForm] UpdateCourseAdminView updateCourseAdminView, Guid id, IFormFile fThumb, Guid notificationId)
        {


            UpdateCourseValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateCourseAdminView);

            if (fThumb != null)
            {
                updateCourseAdminView.UpdateCourseImageDto.ImageFile = fThumb;
            }

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }

                return View("Edit", updateCourseAdminView);
            };
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _courseService.UpdateCourse(id, updateCourseAdminView);

            if (result != true)
            {

                _notyfService.Error("Cập nhật không thành công!");

                return RedirectToAction("Index", "Course");
            }
            _notyfService.Success("Cập nhật thành công!");

            return RedirectToAction("Index", "Course");
            //TempData["fileError"] = "The image must not be empty!";


        }
        [HttpGet]
        [Route("/xoa-khoa-hoc/{Alias}/{id}", Name = "CourseDelete")]

        public async Task<IActionResult> Delete(CourseDto courseDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var course = await _courseService.GetCourseById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(course.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/khoa-hoc.html");

            }
            courseDto.Id = course.Id;

            courseDto.Name = course.Name;

            courseDto.Description = course.Description;

            courseDto.Active = course.Active;

            courseDto.DateCreated = course.DateCreated;

            courseDto.Alias = course.Alias;

            courseDto.CourseImageVideoDto = course.CourseImageVideoDto;

            courseDto.LessionDtos = course.LessionDtos;

            courseDto.Code = course.Code;
            return View(courseDto);
        }
        [HttpPost]
        [Route("/xoa-khoa-hoc/{Alias}/{id}", Name = "CourseDelete")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _courseService.DeleteCourse(id);

            _notyfService.Success("Xóa thành công");

            return RedirectToAction("Index", "Course");
        }
        [HttpPost]
        [Route("/chi-tiet-khoa-hoc/{Alias}/{id}")]
        public async Task<IActionResult> Detail(string keyword, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _courseService.GetCourseById(id);

            if (keyword == null)
            {
                return View(question);

            }
            else
            {
                var result = question.LessionDtos.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword)).ToList();

                if (result.Count == 0)
                {
                    _notyfService.Error("Không tìm thấy kết quả");
                    return View(question);
                }
                else
                {
                    CourseDto courseDto = new()
                    {
                        Id = question.Id,
                        Code = question.Code,
                        Name = question.Name,
                        Description = question.Description,
                        Active = question.Active,
                        DateCreated = question.DateCreated,
                        Notify = question.Notify,
                        DescNotify = question.DescNotify,
                        Alias = question.Alias,
                        LessionDtos = new List<LessionDto>(result),
                    };
                    ViewBag.Question = courseDto;

                    return View(courseDto);
                }
            }

        }
    }

}

