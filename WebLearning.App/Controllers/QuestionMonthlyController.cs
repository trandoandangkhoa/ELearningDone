using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class QuestionMonthlyController : Controller
    {
        private readonly ILogger<QuestionMonthlyController> _logger;

        private readonly IRoleService _roleService;

        private readonly IQuestionMonthlyService _questionService;

        private readonly IQuizMonthlyService _quizService;

        private readonly ICorrectAnswerMonthlyService _correctAnswerMonthlyService;

        private readonly IOptionMonthlyervice _optionMonthlyervice;
        public INotyfService _notyfService { get; }
        public QuestionMonthlyController(ILogger<QuestionMonthlyController> logger, IHttpClientFactory factory, INotyfService notyfService, IRoleService roleService
                                , IQuestionMonthlyService questionService, IQuizMonthlyService quizService, ICorrectAnswerMonthlyService correctAnswerMonthlyService, IOptionMonthlyervice optionMonthlyervice)
        {
            _logger = logger;
            _notyfService = notyfService;
            _roleService = roleService;
            _questionService = questionService;
            _quizService = quizService;
            _correctAnswerMonthlyService = correctAnswerMonthlyService;
            _optionMonthlyervice = optionMonthlyervice;
        }
        // GET: QuestionController
        [Route("/cau-hoi-kiem-tra-dinh-ki.html")]

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
            var Roke = await _questionService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }
            var allRole = await _roleService.GetAllRoles();

            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "Name");


            return View(Roke);
        }

        // GET: QuestionController/Details/5
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/chi-tiet-cau-hoi-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Details(Guid id, string questionMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _questionService.GetQuestionById(id);

            var quizId = await _quizService.AdminGetQuizById(question.QuizMonthlyId);

            if (quizId != null)
            {
                return View(question);

            };
            if (string.IsNullOrEmpty(questionMonthlyId))
            {
                if (TempData["QuestionMonthlyId"].ToString() != null)
                {
                    questionMonthlyId = TempData["QuestionMonthlyId"].ToString();

                    var itemAfterCreated = await _questionService.GetQuestionById(Guid.Parse(questionMonthlyId));

                    var result = new CreateAllConcerningQuestionMonthlyDto()
                    {
                        QuestionMonthlyDto = itemAfterCreated
                    };

                    return View(result);
                }
            }


            return NotFound();
        }

        // GET: QuestionController/Create
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/tao-moi-cau-hoi-kiem-tra-dinh-ki.html")]

        public async Task<IActionResult> Create(Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allQuiz = await _quizService.GetAllQuiz();

            List<SelectListItem> list = new();
            foreach (var item in allQuiz)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                });
            }
            ViewBag.Notification = list;
            TempData["QuizMonthlyId"] = quizMonthlyId.ToString();

            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/tao-moi-cau-hoi-kiem-tra-dinh-ki.html")]

        public async Task<IActionResult> Create([FromForm] CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto, Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuestionMonthlyValidation obj = new();

            ValidationResult validationResult = obj.Validate(createAllConcerningQuestionMonthlyDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var allQuiz = await _quizService.GetAllQuiz();

                List<SelectListItem> list = new();
                foreach (var item in allQuiz)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.ID.ToString(),
                    });
                }
                ViewBag.Notification = list;
                return View("Create", createAllConcerningQuestionMonthlyDto);
            };
            createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Id = Guid.NewGuid();

            createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.QuizMonthlyId = quizMonthlyId;

            TempData["QuestionMonthlyId"] = createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Id.ToString();

            HttpContext.Session.SetString("QuestionMonthlyId", createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Id.ToString());


            var result = await _questionService.InsertConcerningQuestion(createAllConcerningQuestionMonthlyDto);

            if (result == true)
            {
                _notyfService.Success("Tạo thành công");

                return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/tao-moi-cau-hoi-kiem-tra-dinh-ki.html");
            }
            _notyfService.Error("Tạo không thành công!");

            _notyfService.Warning("Vui lòng điền đủ các giá trị");

            return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/tao-moi-cau-hoi-kiem-tra-dinh-ki.html");
        }

        // GET: QuestionController/Edit/5
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/cap-nhat-cau-hoi-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Edit(Guid id, Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            TempData["QuestionMonthlyId"] = id.ToString();

            HttpContext.Session.SetString("QuestionMonthlyId", id.ToString());

            var result = await _questionService.GetQuestionById(id);

            UpdateAllConcerningQuestionMonthlyDto updateResult = new();

            updateResult.UpdateQuestionMonthly = new();

            updateResult.UpdateQuestionMonthly.Name = result.Name;

            updateResult.UpdateQuestionMonthly.Active = result.Active;

            updateResult.UpdateQuestionMonthly.Point = result.Point;

            updateResult.OptionAndCorrectMonthlyDto.OptionMonthlyDtos = new List<OptionMonthlyDto>(result.OptionMonthlyDtos);

            updateResult.OptionAndCorrectMonthlyDto.CorrectAnswerMonthlyDtos = new List<CorrectAnswerMonthlyDto>(result.CorrectAnswerMonthlyDtos);

            TempData["QuizMonthlyId"] = quizMonthlyId.ToString();
            return View(updateResult);
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/cap-nhat-cau-hoi-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Edit(Guid id, Guid quizMonthlyId, [FromForm] UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            UpdateQuestionMonthlyValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateAllConcerningQuestionMonthlyDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
                var result = await _questionService.GetQuestionById(id);

                updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly = new();

                updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.Name = result.Name;

                updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.Active = result.Active;

                updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.Point = result.Point;

                updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.OptionMonthlyDtos = new List<OptionMonthlyDto>(result.OptionMonthlyDtos);

                updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.CorrectAnswerMonthlyDtos = new List<CorrectAnswerMonthlyDto>(result.CorrectAnswerMonthlyDtos);

                return View("Edit", updateAllConcerningQuestionMonthlyDto);
            };
            if (updateAllConcerningQuestionMonthlyDto.NewOptionMonthlys != null && updateAllConcerningQuestionMonthlyDto.NewOptionMonthlys.Length > 0)
            {
                var createNew = await _questionService.InsertNewOptionAndCorrectAnswer(id, updateAllConcerningQuestionMonthlyDto);

                if (createNew == true)
                {
                    _notyfService.Success("Tạo thành công");

                }
                else
                {
                    _notyfService.Error("Tạo không thành công!");

                    _notyfService.Warning("Vui lòng điền đủ các giá trị");
                }


            }
            if (updateAllConcerningQuestionMonthlyDto.OptionMonthlyId != null && updateAllConcerningQuestionMonthlyDto.OptionMonthlyId.Length > 0)
            {
                var result = await _questionService.UpdateConcerningQuestion(id, updateAllConcerningQuestionMonthlyDto);
                if (result == true)
                {
                    _notyfService.Success("Cập nhật thành công");

                    return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}");
            }
            return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/cap-nhat-cau-hoi-kiem-tra-dinh-ki/{id}");
        }

        // GET: QuestionController/Delete/5
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/xoa-cau-hoi-kiem-tra-dinh-ki/{id}")]

        public async Task<IActionResult> Delete(Guid id, QuestionMonthlyDto questionDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allQuiz = await _quizService.GetAllQuiz();

            ViewData["DanhMuc"] = new SelectList(allQuiz, "Id", "Name");

            var result = await _questionService.GetQuestionById(id);

            questionDto.Name = result.Name;

            questionDto.CorrectAnswer = result.CorrectAnswer;

            questionDto.Active = result.Active;


            questionDto.QuizMonthlyId = result.QuizMonthlyId;

            questionDto.QuizMonthlyDto = result.QuizMonthlyDto;

            questionDto.OptionMonthlyDtos = result.OptionMonthlyDtos;

            questionDto.CorrectAnswerMonthlyDtos = result.CorrectAnswerMonthlyDtos;

            questionDto.Point = result.Point;
            questionDto.Code = result.Code;

            return View(questionDto);
        }

        // POST: QuestionController/Delete/5
        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/xoa-cau-hoi-kiem-tra-dinh-ki/{id}")]

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _questionService.DeleteQuestion(id);

            _notyfService.Success("Xóa thành công");

            return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}");
        }

        [Route("/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/xoa-cau-hoi-kiem-tra-dinh-ki/{id}/xoa-lua-chon/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid optionId, Guid id, Guid quizMonthlyId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _optionMonthlyervice.DeleteOptionMonthly(optionId, id);

            _notyfService.Success("Xóa thành công");

            return Redirect($"/chi-tiet-bai-kiem-tra-dinh-ki/{quizMonthlyId}/xoa-cau-hoi-kiem-tra-dinh-ki/{id}");
        }

    }
}

