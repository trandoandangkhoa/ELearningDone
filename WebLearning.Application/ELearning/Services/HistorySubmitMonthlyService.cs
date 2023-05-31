using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IHistorySubmitMonthlyService
    {

        Task<HistorySubmitMonthlyDto> GetHistorySubmitMonthlyById(Guid quizMonthlyId, string accountName);



        Task InsertHistorySubmitMonthlyDto(Guid questionMonthlyId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto);

    }

    public class HistorySubmitMonthlyService : IHistorySubmitMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IAnswerMonthlyService _answerService;
        public HistorySubmitMonthlyService(WebLearningContext context, IMapper mapper, IAnswerMonthlyService answerService)
        {
            _context = context;
            _mapper = mapper;
            _answerService = answerService;
        }


        public async Task<HistorySubmitMonthlyDto> GetHistorySubmitMonthlyById(Guid quizMonthlyId, string accountName)
        {
            var historySubmitMonthly = await _context.HistorySubmitMonthlies.Include(x => x.QuizMonthlies).AsNoTracking().FirstOrDefaultAsync(x => x.QuizMonthlyId.Equals(quizMonthlyId) && x.AccountName == accountName);
            return _mapper.Map<HistorySubmitMonthlyDto>(historySubmitMonthly);
        }


        public async Task InsertHistorySubmitMonthlyDto(Guid questionMonthlyId, string accountName, CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto)
        {
            var answer = await _answerService.GetAnswerById(questionMonthlyId, accountName);

            var question = await _context.QuestionMonthlies.Include(x => x.QuizMonthly).FirstOrDefaultAsync(x => x.Id.Equals(questionMonthlyId));

            var correctAnswer = _context.CorrectAnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(questionMonthlyId)).ToList();

            List<string> correctAnswerMonthlies = new();

            List<string> answerMonthlies = new();

            foreach (var cr in correctAnswer)
            {
                correctAnswerMonthlies.Add(cr.CorrectAnswer);
            }
            foreach (var al in answer)
            {
                answerMonthlies.Add(al.OwnAnswer);
            }
            if (question != null)
            {
                if (answer != null)
                {
                    if (correctAnswer.Count > 0)
                    {
                        var diffDb = answerMonthlies.Where(x => !correctAnswerMonthlies.Contains(x)).ToList();


                        var dif = correctAnswerMonthlies.Where(x => !answerMonthlies.Contains(x)).ToList();



                        if (dif.Count == 0 && diffDb.Count == 0)
                        {
                            createHistorySubmitMonthlyDto.TotalScore += question.Point;
                        }

                        createHistorySubmitMonthlyDto.DateCompoleted = DateTime.Now;

                        createHistorySubmitMonthlyDto.AccountName = accountName;

                        createHistorySubmitMonthlyDto.Id = Guid.NewGuid();

                        createHistorySubmitMonthlyDto.QuizMonthlyId = question.QuizMonthlyId;

                        createHistorySubmitMonthlyDto.Submit = true;

                        HistorySubmitMonthly historySubmitMonthly = _mapper.Map<HistorySubmitMonthly>(createHistorySubmitMonthlyDto);

                        _context.HistorySubmitMonthlies.Add(historySubmitMonthly);

                        await _context.SaveChangesAsync();
                    }

                }
                return;

            }
        }
    }
}
