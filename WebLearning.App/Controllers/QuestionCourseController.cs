using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;

namespace WebLearning.App.Controllers
{
    [Authorize]

    public class QuestionCourseController : Controller
    {
        private readonly ILogger<QuestionCourseController> _logger;


        private readonly ICourseService _courseService;

        private readonly IQuestionCourseService _questionService;

        private readonly IQuizCourseService _quizService;

        private readonly IOptionCourseService _optionCourseService;

        private readonly ICorrectAnswerCourseService _correctAnswerCourseService;
        public INotyfService _notyfService { get; }
        public QuestionCourseController(ILogger<QuestionCourseController> logger, INotyfService notyfService, ICourseService courseService
                                , IQuestionCourseService questionService, IQuizCourseService quizService, IOptionCourseService optionCourseService, ICorrectAnswerCourseService correctAnswerCourseService)
        {
            _logger = logger;
            _courseService = courseService;
            _notyfService = notyfService;
            _questionService = questionService;
            _quizService = quizService;
            _optionCourseService = optionCourseService;
            _correctAnswerCourseService = correctAnswerCourseService;
        }
        // GET: QuestionController
        [Route("/cau-hoi-kiem-tra-cuoi-khoa.html")]

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
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/chi-tiet-cau-hoi-kiem-tra-cuoi-khoa/{id}")]
        public async Task<IActionResult> Details(Guid id, string questionCourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var question = await _questionService.GetQuestionById(id);

            var quizId = await _quizService.AdminGetQuizById(question.QuizCourseId);

            if (quizId != null)
            {
                return View(question);

            };
            if (string.IsNullOrEmpty(questionCourseId))
            {
                if (TempData["QuestionCourseId"].ToString() != null)
                {
                    questionCourseId = TempData["QuestionCourseId"].ToString();

                    var itemAfterCreated = await _questionService.GetQuestionById(Guid.Parse(questionCourseId));

                    var result = new CreateAllConcerningQuestionCourseDto()
                    {
                        QuestionCourseDto = itemAfterCreated
                    };

                    return View(result);
                }
            }


            return NotFound();
        }

        // GET: QuestionController/Create
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{id}/tao-moi-cau-hoi-kiem-tra-cuoi-khoa.html")]

        public async Task<IActionResult> Create(Guid id)
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
            TempData["QuizCourseId"] = id.ToString();

            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/tao-moi-cau-hoi-kiem-tra-cuoi-khoa.html")]

        public async Task<IActionResult> Create([FromForm] CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto, Guid quizCourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            CreateQuestionCourseValidation obj = new();

            ValidationResult validationResult = obj.Validate(createAllConcerningQuestionCourseDto);

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

                return View("Create", createAllConcerningQuestionCourseDto);
            };
            createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Id = Guid.NewGuid();

            createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.QuizCourseId = quizCourseId;

            TempData["QuestionCourseId"] = createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Id.ToString();

            HttpContext.Session.SetString("QuestionCourseId", createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Id.ToString());


            var result = await _questionService.InsertConcerningQuestion(createAllConcerningQuestionCourseDto);

            if (result == true)
            {
                _notyfService.Success("Tạo thành công");

                return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/tao-moi-cau-hoi-kiem-tra-cuoi-khoa.html");
            }
            _notyfService.Error("Tạo không thành công!");

            _notyfService.Warning("Vui lòng điền đủ các giá trị");

            return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/tao-moi-cau-hoi-kiem-tra-cuoi-khoa.html");
        }

        // GET: QuestionController/Edit/5
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/cap-nhat-cau-hoi-kiem-tra-cuoi-khoa/{id}")]

        public async Task<IActionResult> Edit(Guid id, Guid quizCourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            TempData["QuestionCourseId"] = id.ToString();

            HttpContext.Session.SetString("QuestionCourseId", id.ToString());


            var result = await _questionService.GetQuestionById(id);

            UpdateAllConcerningQuestionCourseDto updateResult = new();

            updateResult.UpdateQuestionCourse = new();

            updateResult.UpdateQuestionCourse.Name = result.Name;

            updateResult.UpdateQuestionCourse.Active = result.Active;

            updateResult.UpdateQuestionCourse.Point = result.Point;

            updateResult.OptionAndCorrectCourseDto.OptionCourseDtos = new List<OptionCourseDto>(result.OptionCourseDtos);

            updateResult.OptionAndCorrectCourseDto.CorrectAnswerCourseDtos = new List<CorrectAnswerCourseDto>(result.CorrectAnswerCourseDtos);
            TempData["QuizCourseId"] = quizCourseId.ToString();

            return View(updateResult);
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/cap-nhat-cau-hoi-kiem-tra-cuoi-khoa/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto, Guid quizCourseId)
        {
            UpdateQuestionCourseValidation obj = new();

            ValidationResult validationResult = obj.Validate(updateAllConcerningQuestionCourseDto);
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
                var result = await _questionService.GetQuestionById(id);

                updateAllConcerningQuestionCourseDto.UpdateQuestionCourse = new();

                updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.Name = result.Name;

                updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.Active = result.Active;

                updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.Point = result.Point;

                updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.OptionCourseDtos = new List<OptionCourseDto>(result.OptionCourseDtos);

                updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.CorrectAnswerCourseDtos = new List<CorrectAnswerCourseDto>(result.CorrectAnswerCourseDtos);

                return View("Edit", updateAllConcerningQuestionCourseDto);
            };
            if (updateAllConcerningQuestionCourseDto.NewOptionCourses != null && updateAllConcerningQuestionCourseDto.NewOptionCourses.Length > 0)
            {
                var createNew = await _questionService.InsertNewOptionAndCorrectAnswer(id, updateAllConcerningQuestionCourseDto);

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
            if (updateAllConcerningQuestionCourseDto.OptionCourseId != null && updateAllConcerningQuestionCourseDto.OptionCourseId.Length > 0)
            {
                var result = await _questionService.UpdateConcerningQuestion(id, updateAllConcerningQuestionCourseDto);
                if (result == true)
                {
                    _notyfService.Success("Cập nhật thành công");

                    return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}");
                }

                _notyfService.Error("Cập nhật không thành công!");

                _notyfService.Warning("Vui lòng điền đủ các giá trị");

                return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}");
            }
            return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/cap-nhat-cau-hoi-kiem-tra-theo-bai-hoc/{id}");
        }

        // GET: QuestionController/Delete/5
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/xoa-cau-hoi-kiem-tra-cuoi-khoa/{id}")]

        public async Task<IActionResult> Delete(Guid id, QuestionCourseDto questionDto)
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

            questionDto.QuizCourseId = result.QuizCourseId;

            questionDto.Point = result.Point;

            questionDto.QuizCourseDto = result.QuizCourseDto;

            questionDto.OptionCourseDtos = new List<OptionCourseDto>(result.OptionCourseDtos);

            questionDto.CorrectAnswerCourseDtos = new List<CorrectAnswerCourseDto>(result.CorrectAnswerCourseDtos);

            questionDto.Code = result.Code;

            return View(questionDto);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/xoa-cau-hoi-kiem-tra-cuoi-khoa/{id}")]

        public async Task<IActionResult> Delete(Guid id, Guid quizCourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _questionService.DeleteQuestion(id);

            _notyfService.Success("Xóa thành công");

            return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}");

        }
        [Route("/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/xoa-cau-hoi-kiem-tra-cuoi-khoa/{id}/xoa-lua-chon/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid optionId, Guid id, Guid quizCourseId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _optionCourseService.DeleteOptionCourse(optionId, id);

            _notyfService.Success("Xóa thành công");
            return Redirect($"/chi-tiet-bai-kiem-tra-cuoi-khoa/{quizCourseId}/xoa-cau-hoi-kiem-tra-cuoi-khoa/{id}");
        }

    }
}

