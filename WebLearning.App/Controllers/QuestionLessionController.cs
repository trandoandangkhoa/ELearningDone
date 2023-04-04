using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class QuestionLessionController : Controller
    {
        private readonly ILogger<QuestionLessionController> _logger;

        private readonly ILessionService _lessionService;

        private readonly ICourseService _courseService;

        private readonly IQuestionLessionService _questionService;

        private readonly IQuizLessionService _quizService;

        private readonly IOptionLessionService _optionLessionService;

        private readonly ICorrectAnswerLessionService _correctAnswerLessionService;

        public INotyfService _notyfService { get; }
        public QuestionLessionController(ILogger<QuestionLessionController> logger, IHttpClientFactory factory, ILessionService lessionService, INotyfService notyfService, ICourseService courseService
                                , IQuestionLessionService questionService, IQuizLessionService quizService, IOptionLessionService optionLessionService, ICorrectAnswerLessionService correctAnswerLessionService)
        {
            _logger = logger;
            _courseService = courseService;
            _notyfService = notyfService;
            _lessionService = lessionService;
            _questionService = questionService;
            _quizService = quizService;
            _optionLessionService = optionLessionService;
            _correctAnswerLessionService = correctAnswerLessionService;
        }
        // GET: QuestionController
        [Route("/cau-hoi-kiem-tra-theo-bai-hoc.html")]
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
            var allCourse = await _courseService.GetAllCourse();

            ViewData["DanhMuc"] = new SelectList(allCourse, "Id", "Name");

            return View(Roke);
        }

        // GET: QuestionController/Details/5
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/chi-tiet-cau-hoi-kiem-tra-theo-bai-hoc/{id}")]

        public async Task<IActionResult> Details(Guid id, string questionLessionId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var question = await _questionService.GetQuestionById(id);

            var quizId = await _quizService.AdminGetQuizById(question.QuizLessionId);

            if (quizId != null)
            {
                return View(question);

            };
            if (string.IsNullOrEmpty(questionLessionId))
            {
                if (TempData["QuestionLessionId"].ToString() != null)
                {
                    questionLessionId = TempData["QuestionLessionId"].ToString();

                    var itemAfterCreated = await _questionService.GetQuestionById(Guid.Parse(questionLessionId));

                    var result = new CreateAllConcerningQuestionLessionDto()
                    {
                        QuestionLessionDto = itemAfterCreated
                    };

                    return View(result);
                }
            }


            return NotFound();
        }

        // GET: QuestionController/Create
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/tao-moi-cau-hoi-kiem-tra-theo-bai-hoc.html")]
        public async Task<IActionResult> Create(Guid quizLessionId)
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
            TempData["QuizLessionId"] = quizLessionId.ToString();
            return View();

        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/tao-moi-cau-hoi-kiem-tra-theo-bai-hoc.html")]
        public async Task<IActionResult> Create([FromForm] CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto, Guid quizLessionId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuestionLessionValidation obj = new();

            ValidationResult validationResult = obj.Validate(createAllConcerningQuestionLessionDto);

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
                return View("Create", createAllConcerningQuestionLessionDto);
            };
            createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Id = Guid.NewGuid();

            createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.QuizLessionId = quizLessionId;

            TempData["QuestionLessionId"] = createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Id.ToString();

            HttpContext.Session.SetString("QuestionLessionId", createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Id.ToString());


            var result = await _questionService.InsertConcerningQuestion(createAllConcerningQuestionLessionDto);

            if (result == true)
            {
                _notyfService.Success("Tạo thành công");

                return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}/tao-moi-cau-hoi-kiem-tra-theo-bai-hoc.html");
            }
            _notyfService.Error("Tạo không thành công!");

            _notyfService.Warning("Vui lòng điền đủ các giá trị");

            return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}/tao-moi-cau-hoi-kiem-tra-theo-bai-hoc.html");

        }

        // GET: QuestionController/Edit/5
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/cap-nhat-cau-hoi-kiem-tra-theo-bai-hoc/{id}")]

        public async Task<IActionResult> Edit(Guid id, Guid quizLessionId)
        {

            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _questionService.GetQuestionById(id);
            TempData["QuestionLessionId"] = id.ToString();

            HttpContext.Session.SetString("QuestionLessionId", id.ToString());


            UpdateAllConcerningQuestionLesstionDto updateResult = new();

            updateResult.UpdateQuestionLession = new();

            updateResult.UpdateQuestionLession.Name = result.Name;

            updateResult.UpdateQuestionLession.Active = result.Active;

            updateResult.UpdateQuestionLession.Point = result.Point;

            updateResult.OptionAndCorrectLessionDto.OptionLessionDtos = new List<OptionLessionDto>(result.OptionLessionDtos);

            updateResult.OptionAndCorrectLessionDto.CorrectAnswerLessionDtos = new List<CorrectAnswerLessionDto>(result.CorrectAnswerLessionDtos);
            TempData["QuizLessionId"] = quizLessionId.ToString();

            return View(updateResult);
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/cap-nhat-cau-hoi-kiem-tra-theo-bai-hoc/{id}")]
        public async Task<IActionResult> Edit(Guid id, Guid quizLessionId, [FromForm] UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            UpdateQuestionLessionValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateAllConcerningQuestionLesstionDto);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);

                }
                var result = await _questionService.GetQuestionById(id);

                updateAllConcerningQuestionLesstionDto.UpdateQuestionLession = new();

                updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.Name = result.Name;

                updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.Active = result.Active;

                updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.Point = result.Point;

                updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.OptionLessionDtos = new List<OptionLessionDto>(result.OptionLessionDtos);

                updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.CorrectAnswerLessionDtos = new List<CorrectAnswerLessionDto>(result.CorrectAnswerLessionDtos);

                return View("Edit", updateAllConcerningQuestionLesstionDto);
            };
            if (updateAllConcerningQuestionLesstionDto.NewOptionLessions != null && updateAllConcerningQuestionLesstionDto.NewOptionLessions.Length > 0)
            {
                var createNew = await _questionService.InsertNewOptionAndCorrectAnswer(id, updateAllConcerningQuestionLesstionDto);

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
            if (updateAllConcerningQuestionLesstionDto.OptionLessionId != null && updateAllConcerningQuestionLesstionDto.OptionLessionId.Length > 0)
            {
                var result = await _questionService.UpdateConcerningQuestion(id, updateAllConcerningQuestionLesstionDto);
                if (result == true)
                {
                    _notyfService.Success("Cập nhật thành công");
                    return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}");

                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}");
            }
            return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}/cap-nhat-cau-hoi-kiem-tra-theo-bai-hoc/{id}");

        }

        // GET: QuestionController/Delete/5
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/xoa-cau-hoi-kiem-tra-theo-bai-hoc/{id}")]

        public async Task<IActionResult> Delete(Guid id, QuestionLessionDto questionDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _questionService.GetQuestionById(id);

            questionDto.Name = result.Name;

            questionDto.QuizLessionId = result.QuizLessionId;

            questionDto.OptionLessionDtos = result.OptionLessionDtos;

            questionDto.CorrectAnswerLessionDtos = result.CorrectAnswerLessionDtos;

            questionDto.QuizlessionDto = result.QuizlessionDto;

            questionDto.Point = result.Point;
            questionDto.Code = result.Code;

            return View(questionDto);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/xoa-cau-hoi-kiem-tra-theo-bai-hoc/{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid quizLessionId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _questionService.DeleteQuestion(id);

            _notyfService.Success("Xóa thành công");

            return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}");
        }

        [Route("/chi-tiet-bai-kiem-tra/{quizLessionId}/xoa-cau-hoi-kiem-tra-theo-bai-hoc/{id}/xoa-lua-chon/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid optionId, Guid id, Guid quizLessionId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _optionLessionService.DeleteOptionLession(optionId, id);

            _notyfService.Success("Xóa thành công");

            return Redirect($"/chi-tiet-bai-kiem-tra/{quizLessionId}/xoa-cau-hoi-kiem-tra-theo-bai-hoc/{id}");
        }


    }
}

