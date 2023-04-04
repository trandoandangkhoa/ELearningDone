using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class QuizCourseController : Controller
    {
        private readonly ILogger<QuizCourseController> _logger;

        private readonly ICourseService _CourseService;

        private readonly IQuizCourseService _quizService;

        private readonly IAccountService _accountService;
        public INotyfService _notyfService { get; }
        public QuizCourseController(ILogger<QuizCourseController> logger, ICourseService CourseService, INotyfService notyfService, IQuizCourseService quizService, IAccountService accountService)
        {
            _logger = logger;
            _notyfService = notyfService;
            _CourseService = CourseService;
            _quizService = quizService;
            _accountService = accountService;
        }
        [Route("bai-kiem-tra-cuoi-khoa.html")]
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
            var Roke = await _quizService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }


            return View(Roke);
        }
        [HttpGet]
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{id}", Name = "AdminQuizCourseDetail")]

        public async Task<IActionResult> Details(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var question = await _quizService.AdminGetQuizById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(question.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-cuoi-khoa.html");

            }
            var CourseId = await _CourseService.GetCourseById(question.CourseId);

            ViewData["DanhMuc"] = CourseId.Name;
            ViewBag.AdminQuizCourseId = id;

            return View(question);
        }
        [Route("/tao-moi-bai-kiem-tra-cuoi-khoa.html")]

        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var allCourse = await _CourseService.GetAllCourse();
            List<SelectListItem> list = new();

            foreach (var item in allCourse)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }
            ViewBag.DanhMuc = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/tao-moi-bai-kiem-tra-cuoi-khoa.html")]

        public async Task<IActionResult> Create([FromForm] CreateQuizCourseDto createQuizDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuizCourseValidation obj = new();

            ValidationResult validationResult = obj.Validate(createQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allCourse = await _CourseService.GetAllCourse();

                var checkRole = await _CourseService.GetCourseById(createQuizDto.CourseId);

                var accountId = await _accountService.GetFullName(User.Identity.Name);

                if (accountId.RoleId.Equals(checkRole.CreatedBy) == false)
                {
                    _notyfService.Error("Bạn không thể thêm mới bài kiểm tra cho chương này");

                    return Redirect("/bai-kiem-tra-cuoi-khoa.html");

                }
                List<SelectListItem> list = new();

                foreach (var item in allCourse)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                    });
                }
                ViewBag.DanhMuc = list;
                return View("Create", createQuizDto);
            };
            if (createQuizDto != null)
            {
                var checkRole = await _CourseService.GetCourseById(createQuizDto.CourseId);

                var accountId = await _accountService.GetFullName(User.Identity.Name);

                if (accountId.RoleId.Equals(checkRole.CreatedBy) == false)
                {
                    _notyfService.Error("Bạn không thể thêm mới bài kiểm tra cho chương này");

                    return Redirect("/bai-kiem-tra-cuoi-khoa.html");

                }
                var result = await _quizService.InsertQuiz(createQuizDto);

                if (result == true)
                {
                    _notyfService.Success("Tạo thành công");

                    return RedirectToAction("Index", "QuizCourse");
                }

                _notyfService.Error("Tạo không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizCourse");

            }

            return View(createQuizDto);
        }
        [Route("/cap-nhat-bai-kiem-tra-cuoi-khoa/{id}")]

        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allCourse = await _CourseService.GetAllCourse();

            ViewData["DanhMuc"] = new SelectList(allCourse, "Id", "Name");

            var result = await _quizService.AdminGetQuizById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-cuoi-khoa.html");

            }
            var updateResult = new UpdateQuizCourseDto()
            {
                Name = result.Name,

                Description = result.Description,

                Active = result.Active,

                TimeToDo = result.TimeToDo,

                ScorePass = result.ScorePass,

                Notify = result.Notify,

                DescNotify = result.DescNotify,

            };
            return View(updateResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/cap-nhat-bai-kiem-tra-cuoi-khoa/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateQuizCourseDto updateQuizDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            UpdateQuizCourseValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allCourse = await _CourseService.GetAllCourse();
                List<SelectListItem> list = new();

                foreach (var item in allCourse)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                    });
                }
                ViewBag.DanhMuc = list;
                return View("Edit", updateQuizDto);
            };
            if (updateQuizDto != null)
            {

                var result = await _quizService.UpdateQuiz(updateQuizDto, id);

                if (result == true)
                {
                    _notyfService.Success("Cập nhật thành công");

                    return RedirectToAction("Index", "QuizCourse");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizCourse");
            }
            return View(updateQuizDto);
        }
        [Route("/xoa-bai-kiem-tra-cuoi-khoa/{id}")]

        public async Task<IActionResult> Delete(Guid id, QuizCourseDto quizDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allCourse = await _CourseService.GetAllCourse();

            ViewData["DanhMuc"] = new SelectList(allCourse, "Id", "Name");

            var result = await _quizService.AdminGetQuizById(id);


            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-cuoi-khoa.html");

            }
            var CourseId = await _CourseService.GetCourseById(result.CourseId);


            quizDto.Name = result.Name;

            quizDto.CourseDto = CourseId;

            quizDto.Active = result.Active;

            quizDto.Description = result.Description;

            quizDto.TimeToDo = result.TimeToDo;

            quizDto.ScorePass = result.ScorePass;

            quizDto.DateCreated = result.DateCreated;

            quizDto.ID = result.ID;
            quizDto.Code = result.Code;

            return View(quizDto);
        }

        [HttpPost]
        [Route("/xoa-bai-kiem-tra-cuoi-khoa/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _quizService.DeleteQuiz(id);

            _notyfService.Success("Xóa thành công");

            return RedirectToAction("Index", "QuizCourse");
        }


        [HttpPost]
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{id}")]
        public async Task<IActionResult> Details(string keyword, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _quizService.AdminGetQuizById(id);

            if (keyword == null)
            {
                return View(question);

            }
            else
            {
                var result = question.QuestionCourseDtos.Where(x => x.Name.Contains(keyword)).ToList();

                if (result.Count == 0)
                {
                    _notyfService.Error("Không tìm thấy kết quả");
                    return View(question);
                }
                else
                {
                    QuizCourseDto quizCourseDto = new()
                    {
                        ID = question.ID,
                        CourseId = question.CourseId,
                        Code = question.Code,
                        Name = question.Name,
                        Description = question.Description,
                        Active = question.Active,
                        DateCreated = question.DateCreated,
                        TimeToDo = question.TimeToDo,
                        ScorePass = question.ScorePass,
                        Notify = question.Notify,
                        DescNotify = question.DescNotify,
                        Alias = question.Alias,
                        QuestionCourseDtos = new List<QuestionCourseDto>(result),
                    };
                    ViewBag.Question = quizCourseDto;

                    return View(quizCourseDto);
                }
            }

        }
    }

}
