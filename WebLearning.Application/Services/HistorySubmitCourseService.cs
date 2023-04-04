using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IHistorySubmitCourseService
    {
        Task<HistorySubmitCourseDto> GetHistorySubmitCourseById(Guid quizCourseId, string accountName);

        Task InsertHistorySubmitCourseDto(Guid questionCourseId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto);

    }

    public class HistorySubmitCourseService : IHistorySubmitCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IAnswerCourseService _answerService;
        public HistorySubmitCourseService(WebLearningContext context, IMapper mapper, IAnswerCourseService answerService)
        {
            _context = context;
            _mapper = mapper;
            _answerService = answerService;
        }

        public async Task<HistorySubmitCourseDto> GetHistorySubmitCourseById(Guid quizCourseId, string accountName)
        {
            var historySubmitCourse = await _context.HistorySubmitFinals.Include(x => x.QuizCourses).AsNoTracking().FirstOrDefaultAsync(x => x.QuizCourseId.Equals(quizCourseId) && x.AccountName == accountName);
            return _mapper.Map<HistorySubmitCourseDto>(historySubmitCourse);
        }


        public async Task InsertHistorySubmitCourseDto(Guid questionCourseId, string accountName, CreateHistorySubmitCourseDto createHistorySubmitCourseDto)
        {
            var answer = await _answerService.GetAnswerById(questionCourseId, accountName);

            var question = await _context.QuestionFinals.Include(x => x.QuizCourse).Include(x => x.CorrectAnswers).FirstOrDefaultAsync(x => x.Id.Equals(questionCourseId));

            var correctAnswer = _context.CorrectAnswerCourses.Where(x => x.QuestionCourseId.Equals(questionCourseId)).ToList();

            List<string> correctAnswerCourses = new();

            List<string> answerCourses = new();

            foreach (var cr in correctAnswer)
            {
                correctAnswerCourses.Add(cr.CorrectAnswer);
            }
            foreach (var al in answer)
            {
                answerCourses.Add(al.OwnAnswer);
            }


            if (question != null)
            {
                if (answer != null)
                {
                    if(correctAnswer.Count > 0)
                    {
                        var diffDb = answerCourses.Where(x => !correctAnswerCourses.Contains(x)).ToList();

                        var dif = correctAnswerCourses.Where(x => !answerCourses.Contains(x)).ToList();



                        if (dif.Count == 0 && diffDb.Count == 0)
                        {
                            createHistorySubmitCourseDto.TotalScore += question.Point;
                        }

                        createHistorySubmitCourseDto.DateCompoleted = DateTime.Now;

                        createHistorySubmitCourseDto.AccountName = accountName;

                        createHistorySubmitCourseDto.Id = Guid.NewGuid();

                        createHistorySubmitCourseDto.QuizCourseId = question.QuizCourseId;

                        createHistorySubmitCourseDto.Submit = true;

                        HistorySubmitFinal historySubmitCourse = _mapper.Map<HistorySubmitFinal>(createHistorySubmitCourseDto);

                        _context.HistorySubmitFinals.Add(historySubmitCourse);

                        await _context.SaveChangesAsync();
                    }

                }
                return;

            }
        }
    }
}
