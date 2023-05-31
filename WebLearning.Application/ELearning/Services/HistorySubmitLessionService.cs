using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IHistorySubmitLessionService
    {
        Task<HistorySubmitLessionDto> GetHistorySubmitLessionById(Guid quizLessionId, string accountName);

        Task InsertHistorySubmitLessionDto(Guid questionLessionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto);
        Task UpdateHistorySubmitLession(Guid questionLessionId, Guid quizLessionId, string accountName, UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto);

        Task ResetHistorySubmitLessionDto(Guid quizLessionId, string accountName);

        Task DeleteHistorySubmitLessionDto(Guid questionId);

    }
    public class HistorySubmitLessionService : IHistorySubmitLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IAnswerLessionService _answerService;
        public HistorySubmitLessionService(WebLearningContext context, IMapper mapper, IAnswerLessionService answerService)
        {
            _context = context;
            _mapper = mapper;
            _answerService = answerService;
        }
        public async Task DeleteHistorySubmitLessionDto(Guid questionId)
        {
            var historySubmitLession = await _context.HistorySubmitLessions.FindAsync(questionId);

            if (historySubmitLession == null)
                throw new Exception();

            _context.HistorySubmitLessions.Remove(historySubmitLession);

            await _context.SaveChangesAsync();
        }


        public async Task<HistorySubmitLessionDto> GetHistorySubmitLessionById(Guid quizLessionId, string accountName)
        {
            var historySubmitLession = await _context.HistorySubmitLessions.Include(x => x.QuizLessions).AsNoTracking().FirstOrDefaultAsync(x => x.QuizLessionId.Equals(quizLessionId) && x.AccountName == accountName);
            return _mapper.Map<HistorySubmitLessionDto>(historySubmitLession);
        }

        public async Task InsertHistorySubmitLessionDto(Guid questionLessionId, string accountName, CreateHistorySubmitLessionDto createHistorySubmitLessionDto)
        {
            var answer = await _answerService.GetAnswerById(questionLessionId, accountName);

            var question = await _context.QuestionLessions.Include(x => x.QuizLession).Include(x => x.CorrectAnswers).FirstOrDefaultAsync(x => x.Id.Equals(questionLessionId));

            var correctAnswer = _context.CorrectAnswerLessions.Where(x => x.QuestionLessionId.Equals(questionLessionId)).ToList();

            List<string> correctAnswerLessions = new();

            List<string> answerLessions = new();

            foreach (var cr in correctAnswer)
            {
                correctAnswerLessions.Add(cr.CorrectAnswer);
            }
            foreach (var al in answer)
            {
                answerLessions.Add(al.OwnAnswer);
            }

            if (question != null)
            {
                if (answer.Count != 0)
                {
                    if (correctAnswer.Count > 0)
                    {
                        var diffDb = answerLessions.Where(x => !correctAnswerLessions.Contains(x)).ToList();

                        var dif = correctAnswerLessions.Where(x => !answerLessions.Contains(x)).ToList();


                        if (dif.Count == 0 && diffDb.Count == 0)
                        {
                            createHistorySubmitLessionDto.TotalScore += question.Point;
                        }


                        createHistorySubmitLessionDto.DateCompoleted = DateTime.Now;

                        createHistorySubmitLessionDto.AccountName = accountName;

                        createHistorySubmitLessionDto.Id = Guid.NewGuid();

                        createHistorySubmitLessionDto.QuizLessionId = question.QuizLessionId;

                        createHistorySubmitLessionDto.Submit = true;

                        HistorySubmitLession historySubmitLession = _mapper.Map<HistorySubmitLession>(createHistorySubmitLessionDto);


                        _context.HistorySubmitLessions.Add(historySubmitLession);

                        await _context.SaveChangesAsync();
                    }

                }
                return;

            }
        }

        public async Task ResetHistorySubmitLessionDto(Guid quizLessionId, string accountName)
        {
            var history = _context.HistorySubmitLessions.Where(x => x.QuizLessionId.Equals(quizLessionId) && x.AccountName.Equals(accountName));

            if (history != null)
            {
                _context.HistorySubmitLessions.RemoveRange(history);

                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateHistorySubmitLession(Guid questionId, Guid quizLessionId, string accountName, UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto)
        {
            var answer = await _answerService.GetAnswerById(questionId, accountName);

            var question = await _context.QuestionLessions.Include(x => x.QuizLession).FirstOrDefaultAsync(x => x.Id.Equals(questionId));

            HistorySubmitLession HistorySubmitLession = _mapper.Map(updateHistorySubmitLessionDto, await _context.HistorySubmitLessions.Include(x => x.QuizLessions).FirstOrDefaultAsync(x => x.QuizLessionId.Equals(quizLessionId) && x.AccountName == accountName));
            if (HistorySubmitLession != null)
            {
                updateHistorySubmitLessionDto.Submit = true;

                updateHistorySubmitLessionDto.DateCompoleted = DateTime.Now;

                if (question != null)
                {
                    //if (question.CorrectAnswer == answer.OwnAnswer)
                    //{
                    //    updateHistorySubmitLessionDto.TotalScore += question.Point;
                    //}
                }
                HistorySubmitLession.TotalScore = updateHistorySubmitLessionDto.TotalScore;

                _context.HistorySubmitLessions.Update(HistorySubmitLession);

                await _context.SaveChangesAsync();
            }
        }

    }
}
