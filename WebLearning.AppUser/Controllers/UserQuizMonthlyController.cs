using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.ReportScore;

namespace WebLearning.AppUser.Controllers
{
    [Authorize]
    public class UserQuizMonthlyController : Controller
    {
        private readonly IQuizMonthlyService _quizService;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IAnswerMonthlyService _answerService;
        private readonly IHistorySubmitScoreMonthly _historySubmitScoreMonthly;
        private readonly IReportScoreMonthlyService _reportScoreMonthlyService;

        private readonly INotyfService _notyf;

        public UserQuizMonthlyController(IQuizMonthlyService quizService,
            IAnswerMonthlyService answerService, INotyfService notyf,
            IHistorySubmitScoreMonthly historySubmitScoreMonthly, IReportScoreMonthlyService reportScoreMonthlyService, IRoleService roleService,
             IAccountService accountService)
        {
            _quizService = quizService;
            _answerService = answerService;
            _notyf = notyf;
            _historySubmitScoreMonthly = historySubmitScoreMonthly;
            _reportScoreMonthlyService = reportScoreMonthlyService;
            _roleService = roleService;
            _accountService = accountService;
        }
        // GET: UserQuizMonthlyController
        [Route("/kiem-tra-dinh-ki.html")]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var accountName = User.Identity.Name;

            if (accountName == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var roleUser = await _accountService.GetAccountByEmail(accountName);

            var roleId = await _roleService.GetRoleById(roleUser.AccountDto.RoleId);

            var getOwnQuiz = await _quizService.GetOwnQuiz(roleId.Id);

            return View(getOwnQuiz);
        }
        [Route("/bai-kiem-tra-dinh-ki/{Alias}/{id}")]

        // GET: UserQuizMonthlyController/Details/5
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var quizTotal = await _quizService.GetAllQuiz();

            var quiz = await _quizService.GetQuizById(id, User.Identity.Name);

            Guid idAnswer;
            foreach (var item in quiz.QuestionMonthlyDtos)
            {
                idAnswer = item.Id;
                HttpContext.Session.GetString("AnswerCheck" + idAnswer);

            }

            HttpContext.Session.SetString("QuizId", id.ToString());

            TempData["QuizId"] = id.ToString();

            TempData["Alias"] = quiz.Alias.ToString();

            TempData["QuestionCount"] = quiz.QuestionMonthlyDtos.Count;

            return View(quiz);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendResult(string[] OwnAnswer, Guid questionId, Guid quizMonthlyId)
        {
            try
            {

                string Alias = TempData["Alias"].ToString();

                string url = $"/bai-kiem-tra-dinh-ki/{Alias}/{quizMonthlyId}#{questionId}";

                if (string.IsNullOrEmpty(User.Identity.Name))
                {
                    return Redirect("/dang-nhap.html");
                }
                var answerExist = await _answerService.GetAnswerById(questionId, User.Identity.Name.ToString());

                var keySession = questionId + User.Identity.Name.ToString();


                var quizId = TempData["QuizId"];

                List<CreateAnswerMonthlyDto> createListAnswer = new();

                List<string> answerDatabase = new();

                List<string> nonExist = new();

                List<AnswerMonthlyDto> answerLessionDtos = new();

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
                            createListAnswer.Add(new CreateAnswerMonthlyDto
                            {
                                OwnAnswer = OwnAnswer[i],
                                QuestionMonthlyId = questionId,
                                QuizMonthlyId = quizMonthlyId,
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
                        var resultNonExist = answerExist.FirstOrDefault(x => x.OwnAnswer.Equals(itemNonExist) && x.AccountName.Equals(User.Identity.Name) && x.QuizMonthlyId.Equals(quizMonthlyId) && x.QuestionMonthlyId.Equals(questionId));

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
        public async Task<IActionResult> CheckResult(Guid quizMonthlyId, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto, CreateReportScoreMonthlyDto createReportScoreMonthlyDto)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return Redirect("/dang-nhap.html");
                }
                var quiz = await _quizService.GetQuizById(quizMonthlyId, User.Identity.Name);

                var exist = await _reportScoreMonthlyService.CheckExist(quizMonthlyId, User.Identity.Name);

                if (exist.QuizMonthlyId != Guid.Empty && exist.UserName != null)
                {
                    return Redirect("/kiem-tra-dinh-ki.html");

                }
                else
                {
                    var count = 0;
                    foreach (var item in quiz.QuestionMonthlyDtos)
                    {
                        var questionId = item.Id;

                        count++;

                        createHistorySubmitMonthlyDto.AccountName = User.Identity.Name;


                        var useTest = await _historySubmitScoreMonthly.GetHistorySubmitMonthlyDtoById(questionId, User.Identity.Name);

                        if (useTest.AccountName == null)
                        {
                            var scorecheck = await _historySubmitScoreMonthly.CreateHistorySubmitMonthlyDto(questionId, User.Identity.Name, createHistorySubmitMonthlyDto);

                            if (scorecheck == false)
                            {
                                _notyf.Warning("Còn một số câu chưa có câu trả lời. Vui lòng kiểm tra lại !", 5);

                                _notyf.Error("Nộp bài không thành công!", 5);

                                string url = $"/bai-kiem-tra-dinh-ki/{quiz.Alias}/{quizMonthlyId}#{questionId}";
                            }

                        }


                    }


                    createHistorySubmitMonthlyDto.QuizMonthlyId = quizMonthlyId;

                    createHistorySubmitMonthlyDto.AccountName = User.Identity.Name;



                    var totalScore = await _reportScoreMonthlyService.InsertReportScore(quizMonthlyId, User.Identity.Name, createReportScoreMonthlyDto);

                    _notyf.Success("Nộp bài thành công", 3);


                    return Redirect("/kiem-tra-dinh-ki.html");
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
