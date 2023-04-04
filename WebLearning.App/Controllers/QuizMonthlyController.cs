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
    public class QuizMonthlyController : Controller
    {
        private readonly ILogger<QuizMonthlyController> _logger;

        private readonly IRoleService _roleService;

        private readonly IQuizMonthlyService _quizService;

        private readonly IAccountService _accountService;
        public INotyfService _notyfService { get; }
        public QuizMonthlyController(ILogger<QuizMonthlyController> logger, IHttpClientFactory factory, IRoleService roleService, INotyfService notyfService, IQuizMonthlyService quizService, IAccountService accountService)
        {
            _logger = logger;
            _notyfService = notyfService;
            _roleService = roleService;
            _quizService = quizService;
            _accountService = accountService;
        }
        [Route("/bai-kiem-tra-dinh-ki.html")]
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
            var allRole = await _roleService.GetAllRoles();

            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");

            return View(Roke);
        }
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Details(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _quizService.AdminGetQuizById(id);
            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(question.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-dinh-ki.html");

            }
            var RoleId = await _roleService.GetRoleById(question.RoleId);

            ViewData["DanhMuc"] = RoleId.RoleName;

            return View(question);
        }
        [Route("/tao-moi-bai-kiem-tra-dinh-ki.html")]

        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allRole = await _roleService.GetAllRoles();

            List<SelectListItem> list = new();

            foreach (var item in allRole)
            {
                list.Add(new SelectListItem
                {
                    Text = item.RoleName,
                    Value = item.Id.ToString(),
                });
            }
            ViewBag.DanhMuc = list;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/tao-moi-bai-kiem-tra-dinh-ki.html")]

        public async Task<IActionResult> Create([FromForm] CreateQuizMonthlyDto createQuizDto, Guid notificationId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuizMonthlyValidation obj = new();

            ValidationResult validationResult = obj.Validate(createQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allRole = await _roleService.GetAllRoles();

                List<SelectListItem> list = new();

                foreach (var item in allRole)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.Id.ToString(),
                    });
                }
                ViewBag.DanhMuc = list;
                return View("Create", createQuizDto);
            };
            if (createQuizDto != null)
            {
                var accountId = await _accountService.GetFullName(User.Identity.Name);

                createQuizDto.CreatedBy = accountId.RoleId;
                var result = await _quizService.InsertQuiz(createQuizDto);

                if (result == true)
                {
                    _notyfService.Success("Tạo thành công");

                    return RedirectToAction("Index", "QuizMonthly");
                }

                _notyfService.Error("Tạo không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizMonthly");

            }

            return View(createQuizDto);
        }
        [Route("/cap-nhat-bai-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            //var allRole = await _roleService.GetAllRoles();

            //ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");

            var result = await _quizService.AdminGetQuizById(id);
            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-dinh-ki.html");

            }
            var updateResult = new UpdateQuizMonthlyDto()
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
        [Route("/cap-nhat-bai-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateQuizMonthlyDto updateQuizDto, Guid notificationId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            UpdateQuizMonthlyValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateQuizDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allRole = await _roleService.GetAllRoles();

                List<SelectListItem> list = new();

                foreach (var item in allRole)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.RoleName,
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

                    return RedirectToAction("Index", "QuizMonthly");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return RedirectToAction("Index", "QuizMonthly");
            }

            return View(updateQuizDto);
        }
        [Route("/xoa-bai-kiem-tra-dinh-ki/{id}")]
        public async Task<IActionResult> Delete(Guid id, QuizMonthlyDto quizDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allRole = await _roleService.GetAllRoles();

            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");

            var result = await _quizService.AdminGetQuizById(id);

            var accountId = await _accountService.GetFullName(User.Identity.Name);

            if (accountId.RoleId.Equals(result.CreatedBy) == false)
            {
                _notyfService.Error("Bạn không có quyền xem mục này");

                return Redirect("/bai-kiem-tra-dinh-ki.html");

            }
            var RoleId = await _roleService.GetRoleById(result.RoleId);


            quizDto.Name = result.Name;

            quizDto.RoleDto = RoleId;

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
        [Route("/xoa-bai-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _quizService.DeleteQuiz(id);

            _notyfService.Success("Xóa thành công");

            return RedirectToAction("Index", "QuizMonthly");
        }
        [HttpPost]
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{id}")]
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
                var result = question.QuestionMonthlyDtos.Where(x => x.Name.Contains(keyword)).ToList();

                if (result.Count == 0)
                {
                    _notyfService.Error("Không tìm thấy kết quả");
                    return View(question);
                }
                else
                {
                    QuizMonthlyDto quizMonthlyDto = new()
                    {
                        ID = question.ID,
                        RoleId = question.RoleId,
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
                        QuestionMonthlyDtos = new List<QuestionMonthlyDto>(result),
                    };
                    ViewBag.Question = quizMonthlyDto;

                    return View(quizMonthlyDto);
                }
            }

        }
    }

}
