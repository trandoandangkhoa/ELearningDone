using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.AppUser.Controllers
{
    [Authorize]
    public class UserQuizLessionController : Controller
    {
        private readonly IQuizLessionService _quizService;
        private readonly IAnswerLessionService _answerService;
        private readonly IHistorySubmitScoreLession _historySubmitScoreLession;
        private readonly IReportScoreLessionService _reportScoreLessionService;

        private readonly INotyfService _notyf;

        public UserQuizLessionController(IQuizLessionService quizService,
            IAnswerLessionService answerService, INotyfService notyf,
            IHistorySubmitScoreLession historySubmitScoreLession, IReportScoreLessionService reportScoreLessionService)
        {
            _quizService = quizService;
            _answerService = answerService;
            _notyf = notyf;
            _historySubmitScoreLession = historySubmitScoreLession;
            _reportScoreLessionService = reportScoreLessionService;
        }
        // GET: UserQuizCourseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserQuizCourseController/Details/5
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/{LessionId}/bai-kiem-tra-theo-bai/{Alias}/{id}", Name = "QuizLessionDetail")]
        public async Task<IActionResult> Detail(Guid id, Guid CourseId, string AliasCourse, Guid LessionId)
        {

            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}";

            if (CourseId != Guid.Empty)
            {

                var index = await _quizService.FindIndexQuiz(id, LessionId);

                var checkPassed = await _quizService.CheckPassedQuiz(id, User.Identity.Name, LessionId);

                if (index != checkPassed)
                {
                    _notyf.Warning("Bạn chưa hoàn thành các bài kiểm tra trước");

                    return Redirect(url);
                }
            }

            var quiz = await _quizService.GetQuizById(id, User.Identity.Name);

            TempData["QuizId"] = id.ToString();

            TempData["CourseId"] = CourseId.ToString();

            TempData["LessionId"] = LessionId.ToString();


            TempData["AliasCourse"] = AliasCourse.ToString();

            TempData["Alias"] = quiz.Alias.ToString();

            TempData["QuestionCount"] = quiz.QuestionLessionDtos.Count;

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendResult(string[] OwnAnswer, Guid questionId, Guid quizLessionId)
        {

            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Login", "Login");
            }
            string Alias = TempData["Alias"].ToString();

            string CourseId = TempData["CourseId"].ToString();

            string LessionId = TempData["LessionId"].ToString();

            string AliasCourse = TempData["AliasCourse"].ToString();

            string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/{LessionId}/bai-kiem-tra-theo-bai/{Alias}/{quizLessionId}#{questionId}";


            var answerExist = await _answerService.GetAnswerById(questionId, User.Identity.Name.ToString());

            List<CreateAnswerLessionDto> createListAnswer = new();

            List<string> answerDatabase = new();

            List<string> nonExist = new();

            List<AnswerLessionDto> answerLessionDtos = new();

            //Check if exist in database
            for (int i = 0; i < answerExist.Count; i++)
            {
                answerDatabase.Add(answerExist[i].OwnAnswer);
            }
            string[] arrayAnswerDb = answerDatabase.ToArray();

            if (arrayAnswerDb.Length > 0)
            {
                foreach (string answerDb in arrayAnswerDb)
                {
                    if (!OwnAnswer.Contains(answerDb))
                    {

                        nonExist.Add(answerDb);
                    }
                }
            }
            String[] strNonExist = nonExist.ToArray();


            //Add item if not exist in database
            if (OwnAnswer.Length > 0)
            {
                for (int i = 0; i < OwnAnswer.Length; i++)
                {
                    if (OwnAnswer != null)
                    {
                        createListAnswer.Add(new CreateAnswerLessionDto
                        {
                            OwnAnswer = OwnAnswer[i],
                            QuestionId = questionId,
                            QuizLessionId = quizLessionId,
                            AccountName = User.Identity.Name,
                            CheckBoxId = i,
                            Checked = true,
                        });
                    }


                }


            }

            if (strNonExist.Length > 0)
            {
                foreach (var itemNonExist in strNonExist)
                {
                    var resultNonExist = answerExist.FirstOrDefault(x => x.OwnAnswer.Equals(itemNonExist) && x.AccountName.Equals(User.Identity.Name) && x.QuizLessionId.Equals(quizLessionId) && x.QuestionId.Equals(questionId));

                    if (resultNonExist != null)
                    {
                        var resultDelete = await _answerService.DeleteAnswerNonExist(resultNonExist.Id);

                        _notyf.Success("Lưu đáp án vào bộ nhớ thành công", 3);
                    }
                }
            }


            for (int i = 0; i < createListAnswer.Count; i++)
            {
                if (nonExist != null)
                {
                    if (answerExist.Any(x => x.OwnAnswer.Equals(createListAnswer[i].OwnAnswer)) == false)
                    {
                        var result = await _answerService.InsertAnswer(createListAnswer[i]);
                        if (result == false)
                        {
                            _notyf.Warning("Lưu đáp án vào bộ nhớ không thành công", 3);

                        }

                        _notyf.Success("Lưu đáp án vào bộ nhớ thành công", 3);
                    }

                }


            }




            return Redirect(url);

        }

        [HttpPost]
        public async Task<IActionResult> CheckResult(Guid quizLessionId, CreateHistorySubmitLessionDto createHistorySubmitLessionDto, UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto, CreateReportScoreLessionDto createReportScoreLessionDto)
        {
            try
            {
                string Alias = TempData["Alias"].ToString();

                string CourseId = TempData["CourseId"].ToString();

                string LessionId = TempData["LessionId"].ToString();


                string AliasCourse = TempData["AliasCourse"].ToString();

                string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/{LessionId}/bai-kiem-tra-theo-bai/{Alias}/{quizLessionId}";

                if (User.Identity.Name == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                var quiz = await _quizService.GetQuizById(quizLessionId, User.Identity.Name);

                var exist = await _reportScoreLessionService.CheckExist(quizLessionId, User.Identity.Name);

                if (exist.QuizLessionId != Guid.Empty && exist.UserName != null)
                {
                    return Redirect($"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}");
                }
                else
                {
                    var count = 0;
                    foreach (var item in quiz.QuestionLessionDtos)
                    {
                        var questionId = item.Id;

                        count++;

                        createHistorySubmitLessionDto.AccountName = User.Identity.Name;


                        var useTest = await _historySubmitScoreLession.GetHistorySubmitLessionDtoById(questionId, User.Identity.Name);

                        if (useTest.AccountName == null)
                        {
                            var scorecheck = await _historySubmitScoreLession.CreateHistorySubmitLessionDto(questionId, User.Identity.Name, createHistorySubmitLessionDto);

                            if (scorecheck == false)
                            {
                                _notyf.Warning("Còn một số câu chưa có câu trả lời. Vui lòng kiểm tra lại !", 5);
                                _notyf.Error("Nộp bài không thành công!", 5);

                                return Redirect(url);
                            }

                        }

                    }


                    createReportScoreLessionDto.QuizLessionId = quizLessionId;

                    createReportScoreLessionDto.UserName = User.Identity.Name;


                    var totalScore = await _reportScoreLessionService.InsertReportScore(quizLessionId, User.Identity.Name, createReportScoreLessionDto);

                    _notyf.Success("Nộp bài thành công", 3);


                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]

        public async Task<IActionResult> Reset(Guid quizLessionId, string accountName)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                string Alias = TempData["Alias"].ToString();

                string CourseId = TempData["CourseId"].ToString();
                string LessionId = TempData["LessionId"].ToString();

                string AliasCourse = TempData["AliasCourse"].ToString();

                string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/{LessionId}/bai-kiem-tra-theo-bai/{Alias}/{quizLessionId}";

                accountName = User.Identity.Name;

                var resetAnswer = await _answerService.ResetAllAnswer(quizLessionId, accountName);

                var resetHistory = await _historySubmitScoreLession.ResetHistorySubmitLessionDto(quizLessionId, accountName);

                var resetReport = await _reportScoreLessionService.ResetReportScore(quizLessionId, accountName);

                return Redirect(url);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
