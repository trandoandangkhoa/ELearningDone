using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.AppUser.Controllers
{
    public class UserQuizCourseController : Controller
    {
        private readonly IQuizCourseService _quizService;
        private readonly ILessionService _lessionService;

        private readonly ICourseService _courseService;
        private readonly IAnswerCourseService _answerService;
        private readonly IHistorySubmitScoreCourse _historySubmitScoreCourse;
        private readonly IReportScoreCourseService _reportScoreCourseService;

        private readonly INotyfService _notyf;

        public UserQuizCourseController(IQuizCourseService quizService,
            IAnswerCourseService answerService, INotyfService notyf,
            IHistorySubmitScoreCourse historySubmitScoreCourse, IReportScoreCourseService reportScoreCourseService, ICourseService courseService, ILessionService lessionService)
        {
            _quizService = quizService;
            _answerService = answerService;
            _notyf = notyf;
            _historySubmitScoreCourse = historySubmitScoreCourse;
            _reportScoreCourseService = reportScoreCourseService;
            _courseService = courseService;
            _lessionService = lessionService;
        }
        // GET: UserQuizCourseController
        public ActionResult Index()
        {

            return View();
        }

        // GET: UserQuizCourseController/Details/5
        [Route("/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/bai-kiem-tra-cuoi-khoa/{Alias}/{id}/", Name = "QuizCourseDetail")]
        [Authorize]

        public async Task<IActionResult> Detail(Guid id, Guid CourseId, string AliasCourse)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var quiz = await _quizService.GetQuizById(id, User.Identity.Name);


            string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}";


            Guid idAnswer;

            var lessionInCourse = await _courseService.GetCourseById(CourseId);

            var totalQuizLession = 0;

            var totalQuizLessionPassed = 0;

            foreach (var item in lessionInCourse.LessionDtos)
            {
                var lession = await _lessionService.UserGetLessionById(item.Id, User.Identity.Name);

                var quizLession = lession.QuizlessionDtos.Count;

                totalQuizLession += quizLession;

                foreach (var quizLessionId in lession.QuizlessionDtos.Where(x => x.ReportScoreLessionDtos != null))
                {

                    var quizLessionPassed = quizLessionId.ReportScoreLessionDtos.Where(x => x.Passed == true && x.UserName.Equals(User.Identity.Name)).Count();

                    totalQuizLessionPassed += quizLessionPassed;

                }

            }
            if (totalQuizLessionPassed != totalQuizLession)
            {
                _notyf.Error("Vui lòng hoàn thành các bài kiểm tra trước đó");

                return Redirect(url);
            }


            foreach (var item in quiz.QuestionCourseDtos)
            {
                idAnswer = item.Id;
                HttpContext.Session.GetString("AnswerCheck" + idAnswer);

            }

            HttpContext.Session.SetString("QuizId", id.ToString());

            TempData["QuizId"] = id.ToString();

            TempData["CourseId"] = CourseId.ToString();

            TempData["AliasCourse"] = AliasCourse.ToString();

            TempData["Alias"] = quiz.Alias.ToString();

            TempData["QuestionCount"] = quiz.QuestionCourseDtos.Count;

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendResult(string[] OwnAnswer, Guid questionId, Guid quizCourseId)
        {

            try
            {
                string Alias = TempData["Alias"].ToString();

                string CourseId = TempData["CourseId"].ToString();

                string AliasCourse = TempData["AliasCourse"].ToString();

                string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/bai-kiem-tra-cuoi-khoa/{Alias}/{quizCourseId}#{questionId}";

                if (string.IsNullOrEmpty(User.Identity.Name))
                {
                    return Redirect("/dang-nhap.html");
                }
                var answerExist = await _answerService.GetAnswerById(questionId, User.Identity.Name.ToString());

                var keySession = questionId + User.Identity.Name.ToString();


                var quizId = TempData["QuizId"];

                List<CreateAnswerCourseDto> createListAnswer = new();

                List<string> answerDatabase = new();

                List<string> nonExist = new();

                List<AnswerCourseDto> answerLessionDtos = new();

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
                            createListAnswer.Add(new CreateAnswerCourseDto
                            {
                                OwnAnswer = OwnAnswer[i],
                                QuestionCourseId = questionId,
                                QuizCourseId = quizCourseId,
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
                        var resultNonExist = answerExist.FirstOrDefault(x => x.OwnAnswer.Equals(itemNonExist) && x.AccountName.Equals(User.Identity.Name) && x.QuizCourseId.Equals(quizCourseId) && x.QuestionCourseId.Equals(questionId));

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

                            HttpContext.Session.SetString(keySession, createListAnswer[i].OwnAnswer);

                            _notyf.Success("Lưu đáp án vào bộ nhớ thành công", 3);
                        }

                    }


                }




                return Redirect(url);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckResult(Guid quizCourseId, CreateHistorySubmitCourseDto createHistorySubmitCourseDto, CreateReportScoreCourseDto createReportScoreCourseDto)
        {
            try
            {
                string Alias = TempData["Alias"].ToString();

                string CourseId = TempData["CourseId"].ToString();

                string AliasCourse = TempData["AliasCourse"].ToString();

                string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/bai-kiem-tra-cuoi-khoa/{Alias}/{quizCourseId}";

                if (User.Identity.Name == null)
                {
                    return Redirect("/dang-nhap.html");
                }


                var quiz = await _quizService.GetQuizById(quizCourseId, User.Identity.Name.ToString());


                var count = 0;
                foreach (var item in quiz.QuestionCourseDtos)
                {
                    var questionId = item.Id;

                    count++;

                    createHistorySubmitCourseDto.AccountName = User.Identity.Name;


                    var useTest = await _historySubmitScoreCourse.GetHistorySubmitCourseDtoById(questionId, User.Identity.Name);

                    if (useTest.AccountName == null)
                    {
                        var scorecheck = await _historySubmitScoreCourse.CreateHistorySubmitCourseDto(questionId, User.Identity.Name, createHistorySubmitCourseDto);

                        if (scorecheck == false)
                        {
                            _notyf.Warning("Còn một số câu chưa có câu trả lời. Vui lòng kiểm tra lại !", 5);

                            _notyf.Error("Nộp bài không thành công!", 5);

                            return Redirect(url);
                        }

                    }
                    else
                    {
                        _notyf.Error("Bạn đã làm bài kiểm tra này rồi", 5);

                        return Redirect(url);
                    }



                }
                createReportScoreCourseDto.QuizCourseId = quizCourseId;

                createReportScoreCourseDto.UserName = User.Identity.Name;

                var totalScore = await _reportScoreCourseService.InsertReportScore(quizCourseId, User.Identity.Name, createReportScoreCourseDto);

                _notyf.Success("Nộp bài thành công", 3);


                return Redirect(url);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]

        public async Task<IActionResult> Reset(Guid quizCourseId, string accountName)
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

                string AliasCourse = TempData["AliasCourse"].ToString();

                string url = $"/chi-tiet-khoa-hoc/{AliasCourse}/{CourseId}/bai-kiem-tra-cuoi-khoa/{Alias}/{quizCourseId}";


                accountName = User.Identity.Name;

                var resetQuiz = await _quizService.ResetQuizCourse(quizCourseId, accountName);


                return Redirect(url);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
