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
    public class QuizLessionController : Controller
    {
        private readonly ILogger<QuizLessionController> _logger;

        private readonly ILessionService _lessionService;

        private readonly IQuizLessionService _quizService;
        private readonly IAccountService _accountService;
        public INotyfService _notyfService { get; }
        public QuizLessionController(ILogger<QuizLessionController> logger, IHttpClientFactory factory, ILessionService lessionService, INotyfService notyfService, IQuizLessionService quizService, IAccountService accountService)
        {
            _logger = logger;
            _notyfService = notyfService;
            _lessionService = lessionService;
            _quizService = quizService;
            _accountService = accountService;
        }
        [Route("/bai-kiem-tra-theo-bai-hoc.html")]
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
            var allLession = await _lessionService.GetAllLession();

            ViewData["DanhMuc"] = new SelectList(allLession, "Id", "Name");

            return View(Roke);
        }
        [Route("/chi-tiet-bai-kiem-tra/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _quizService.AdminGetQuizById(id);


            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(question.LessionDto.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-theo-bai-hoc.html");

            }
            var lessionId = await _lessionService.GetLessionById(question.LessionId);

            ViewData["DanhMuc"] = lessionId.Name;

            return View(question);
        }
        [Route("/tao-moi-bai-kiem-tra.html")]

        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allLession = await _lessionService.GetAllLession();

            List<SelectListItem> list = new();

            foreach (var item in allLession)
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
        [Route("/tao-moi-bai-kiem-tra.html")]

        public async Task<IActionResult> Create([FromForm] CreateQuizLessionDto createQuizDto, Guid notificationId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuizLessionValidation obj = new();

            ValidationResult validationResult = obj.Validate(createQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }

                var allLession = await _lessionService.GetAllLession();



                List<SelectListItem> list = new();

                foreach (var item in allLession)
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
                var checkRole = await _lessionService.GetLessionById(createQuizDto.LessionId);

                var accountId = await _accountService.GetFullName(User.Identity.Name);

                if (accountId.RoleId.Equals(checkRole.CourseDto.CreatedBy) == false)
                {
                    _notyfService.Error("Bạn không thể thêm mới bài kiểm tra cho chương này");

                    return Redirect("/bai-kiem-tra-theo-bai-hoc.html");

                }
                var result = await _quizService.InsertQuiz(createQuizDto);
                if (result == true)
                {
                    _notyfService.Success("Tạo thành công");

                    return RedirectToAction("Index", "QuizLession");
                }

                _notyfService.Error("Tạo không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizLession");

            }
            return View(createQuizDto);
        }
        [Route("/cap-nhat-bai-kiem-tra/{id}")]

        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allLession = await _lessionService.GetAllLession();

            ViewData["DanhMuc"] = new SelectList(allLession, "Id", "Name");

            var result = await _quizService.AdminGetQuizById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.LessionDto.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-theo-bai-hoc.html");

            }
            var updateResult = new UpdateQuizLessionDto()
            {
                Name = result.Name,

                Description = result.Description,

                Active = result.Active,

                TimeToDo = result.TimeToDo,

                SortItem = result.SortItem,

                ScorePass = result.ScorePass,

                Notify = result.Notify,

                DescNotify = result.DescNotify,


            };
            return View(updateResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/cap-nhat-bai-kiem-tra/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateQuizLessionDto updateQuizDto, Guid notificationId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            UpdateQuizLessionValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allLession = await _lessionService.GetAllLession();

                List<SelectListItem> list = new();

                foreach (var item in allLession)
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

                    return RedirectToAction("Index", "QuizLession");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizLession");
            }
            return View(updateQuizDto);
        }
        [HttpGet]
        [Route("/xoa-bai-kiem-tra/{id}")]

        public async Task<IActionResult> Delete(Guid id, QuizlessionDto quizDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allLession = await _lessionService.GetAllLession();

            ViewData["DanhMuc"] = new SelectList(allLession, "Id", "Name");

            var result = await _quizService.AdminGetQuizById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.LessionDto.CourseDto.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-theo-bai-hoc.html");

            }
            var lessionId = await _lessionService.GetLessionById(result.LessionId);


            quizDto.Name = result.Name;

            quizDto.LessionDto = lessionId;

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
        [Route("/xoa-bai-kiem-tra/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _quizService.DeleteQuiz(id);

            _notyfService.Success("Xóa thành công");

            return RedirectToAction("Index", "QuizLession");
        }

        [HttpPost]
        [Route("/chi-tiet-bai-kiem-tra/{id}")]
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
                var result = question.QuestionLessionDtos.Where(x => x.Name.Contains(keyword)).ToList();

                if (result.Count == 0)
                {
                    _notyfService.Error("Không tìm thấy kết quả");
                    return View(question);
                }
                else
                {
                    QuizlessionDto quizlessionDto = new()
                    {
                        ID = question.ID,
                        LessionId = question.LessionId,
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
                        QuestionLessionDtos = new List<QuestionLessionDto>(result),
                    };
                    ViewBag.Question = quizlessionDto;

                    return View(quizlessionDto);
                }
            }

        }
    }

}
