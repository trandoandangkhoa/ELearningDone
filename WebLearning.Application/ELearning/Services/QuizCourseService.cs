using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IQuizCourseService
    {
        Task<IEnumerable<QuizCourseDto>> GetQuizDtos();

        Task<QuizCourseDto> GetQuizById(Guid id, string accountName);

        Task<QuizCourseDto> AdminGetQuizCourseById(Guid id);

        Task<QuizCourseDto> GetNameQuiz(Guid id);

        Task<QuizCourseDto> CheckExist(Guid id, string name);

        Task InsertQuizDto(CreateQuizCourseDto createQuizCourseDto);

        Task DeleteQuizDto(Guid id);

        Task ResetQuizCourse(Guid quizCourseId, string accountName);

        Task UpdateQuizDto(Guid id, UpdateQuizCourseDto updateQuizCourseDto);

        Task<PagedViewModel<QuizCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<IEnumerable<QuizCourseDto>> GetQuizByCategory(GetListPagingRequest getListPagingRequest);

        Task<QuizCourseDto> GetCode(string code);
    }
    public class QuizCourseService : IQuizCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public QuizCourseService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task<QuizCourseDto> AdminGetQuizCourseById(Guid id)
        {
            var quizCourse = await _context.QuizCourses.Include(x => x.QuestionFinals).Include(x => x.Course).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));

            var correct = await _context.CorrectAnswerCourses.ToListAsync();


            var correctDto = _mapper.Map<List<CorrectAnswerCourseDto>>(correct);

            var quizCourseDto = _mapper.Map<QuizCourseDto>(quizCourse);
            foreach (var cr in quizCourseDto.QuestionCourseDtos)
            {
                foreach (var cre in correctDto.Where(x => x.QuestionCourseId.Equals(cr.Id)))
                {
                    cr.CorrectAnswerCourseDtos.Add(cre);
                }
                continue;
            };
            return quizCourseDto;
        }

        public async Task<QuizCourseDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuizCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuizCourseDto>(account);
        }

        public async Task DeleteQuizDto(Guid id)
        {
            var quizCourse = await _context.QuizCourses.FindAsync(id);
            if (quizCourse != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.QuizCourses.Remove(quizCourse);
                await _context.SaveChangesAsync();

                var historySubmit = _context.HistorySubmitFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID));

                _context.HistorySubmitFinals.RemoveRange(historySubmit);

                await _context.SaveChangesAsync();



                var reportScore = _context.ReportUserScoreFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID));
                _context.ReportUserScoreFinals.RemoveRange(reportScore);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }

        public async Task<QuizCourseDto> GetCode(string code)
        {
            var account = await _context.QuizCourses.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuizCourseDto>(account);
        }

        public async Task<QuizCourseDto> GetNameQuiz(Guid id)
        {
            var name = await _context.QuizCourses.AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));

            var quizDto = _mapper.Map<QuizCourseDto>(name);

            return quizDto;
        }

        public async Task<PagedViewModel<QuizCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:QuizCourse"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.QuizCourses.Count() / (double)pageResult);

            var query = _context.QuizCourses.Include(x => x.Course).AsQueryable();

            var questionCourse = _context.QuestionFinals.AsQueryable();

            var historySubmitCourse = _context.HistorySubmitFinals.AsQueryable();

            var answerCourse = _context.AnswerCourses.AsQueryable();

            var reportScoreCourse = _context.ReportUserScoreFinals.AsQueryable();

            var questionDto = _mapper.Map<List<QuestionCourseDto>>(questionCourse);

            var historySubmitCourseDto = _mapper.Map<List<HistorySubmitCourseDto>>(historySubmitCourse);

            var answerCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerCourse);

            var reportScoreCourseDto = _mapper.Map<List<ReportScoreCourseDto>>(reportScoreCourse);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Course.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(quizCourse => new QuizCourseDto()
                                    {
                                        ID = quizCourse.ID,
                                        CourseId = quizCourse.CourseId,
                                        Name = quizCourse.Name,
                                        Description = quizCourse.Description,
                                        Active = quizCourse.Active,
                                        DateCreated = quizCourse.DateCreated,
                                        TimeToDo = quizCourse.TimeToDo,
                                        ScorePass = quizCourse.ScorePass,
                                        Alias = quizCourse.Alias,
                                        Code = quizCourse.Code,
                                        CourseDto = new CourseDto()
                                        {
                                            Id = quizCourse.Course.Id,
                                            Name = quizCourse.Course.Name,
                                            Description = quizCourse.Course.Description,
                                            Active = quizCourse.Course.Active,
                                            DateCreated = quizCourse.Course.DateCreated,
                                            Alias = quizCourse.Course.Alias,
                                            Code = quizCourse.Course.Code,
                                        },
                                        QuestionCourseDtos = new List<QuestionCourseDto>(questionDto),
                                        HistorySubmitCourseDtos = new List<HistorySubmitCourseDto>(historySubmitCourseDto),
                                        AnswerCourseDtos = new List<AnswerCourseDto>(answerCourseDto),
                                        ReportScoreCourseDtos = new List<ReportScoreCourseDto>(reportScoreCourseDto),

                                    }).OrderByDescending(x => x.DateCreated).ToListAsync();
            var quizCourseResponse = new PagedViewModel<QuizCourseDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return quizCourseResponse;
        }

        public Task<IEnumerable<QuizCourseDto>> GetQuizByCategory(GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<QuizCourseDto> GetQuizById(Guid id, string accountName)
        {
            var quizCourse = await _context.QuizCourses.Include(x => x.QuestionFinals).ThenInclude(x => x.Options).Include(x => x.Course).OrderBy(X => X.DateCreated).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (quizCourse == null)
            {
                return default;
            }
            else
            {
                if (accountName != null)
                {
                    var questionCourse = quizCourse.QuestionFinals.ToList();

                    var historySubmitCourse = _context.HistorySubmitFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID) && x.AccountName.Equals(accountName)).AsNoTracking().AsQueryable();

                    var answerCourse = _context.AnswerCourses.Where(x => x.QuizCourseId.Equals(quizCourse.ID) && x.AccountName.Equals(accountName)).AsNoTracking().AsQueryable();

                    var reportScoreCourse = _context.ReportUserScoreFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID) && x.UserName.Equals(accountName)).AsNoTracking().AsQueryable();

                    var questionDto = _mapper.Map<List<QuestionCourseDto>>(questionCourse);

                    var historySubmitCourseDto = _mapper.Map<List<HistorySubmitCourseDto>>(historySubmitCourse);

                    var answerCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerCourse);

                    var reportScoreCourseDto = _mapper.Map<List<ReportScoreCourseDto>>(reportScoreCourse);

                    QuizCourseDto quizCourseDto = new QuizCourseDto()
                    {
                        ID = quizCourse.ID,
                        CourseId = quizCourse.CourseId,
                        Name = quizCourse.Name,
                        Description = quizCourse.Description,
                        Active = quizCourse.Active,
                        DateCreated = quizCourse.DateCreated,
                        TimeToDo = quizCourse.TimeToDo,
                        ScorePass = quizCourse.ScorePass,
                        Alias = quizCourse.Alias,
                        Code = quizCourse.Code,
                        CourseDto = new CourseDto()
                        {
                            Id = quizCourse.Course.Id,
                            Name = quizCourse.Course.Name,
                            Description = quizCourse.Course.Description,
                            Active = quizCourse.Course.Active,
                            DateCreated = quizCourse.Course.DateCreated,
                            Alias = quizCourse.Course.Alias,
                            Code = quizCourse.Course.Code,
                        },
                        QuestionCourseDtos = new List<QuestionCourseDto>(questionDto),
                        HistorySubmitCourseDtos = new List<HistorySubmitCourseDto>(historySubmitCourseDto),
                        AnswerCourseDtos = new List<AnswerCourseDto>(answerCourseDto),
                        ReportScoreCourseDtos = new List<ReportScoreCourseDto>(reportScoreCourseDto),

                    };
                    return quizCourseDto;
                }
                else
                {
                    var questionCourse = quizCourse.QuestionFinals.ToList();

                    var historySubmitCourse = _context.HistorySubmitFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID)).ToList();

                    var answerCourse = _context.AnswerCourses.Where(x => x.QuizCourseId.Equals(quizCourse.ID)).ToList();

                    var reportScoreCourse = _context.ReportUserScoreFinals.Where(x => x.QuizCourseId.Equals(quizCourse.ID)).ToList();

                    var questionDto = _mapper.Map<List<QuestionCourseDto>>(questionCourse);

                    var historySubmitCourseDto = _mapper.Map<List<HistorySubmitCourseDto>>(historySubmitCourse);

                    var answerCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerCourse);

                    var reportScoreCourseDto = _mapper.Map<List<ReportScoreCourseDto>>(reportScoreCourse);

                    QuizCourseDto quizCourseDto = new QuizCourseDto()
                    {
                        ID = quizCourse.ID,
                        CourseId = quizCourse.CourseId,
                        Name = quizCourse.Name,
                        Description = quizCourse.Description,
                        Active = quizCourse.Active,
                        DateCreated = quizCourse.DateCreated,
                        TimeToDo = quizCourse.TimeToDo,
                        ScorePass = quizCourse.ScorePass,
                        Alias = quizCourse.Alias,
                        Code = quizCourse.Code,
                        CourseDto = new CourseDto()
                        {
                            Id = quizCourse.Course.Id,
                            Name = quizCourse.Course.Name,
                            Description = quizCourse.Course.Description,
                            Active = quizCourse.Course.Active,
                            DateCreated = quizCourse.Course.DateCreated,
                            Alias = quizCourse.Course.Alias,
                            Code = quizCourse.Course.Code,
                        },
                        QuestionCourseDtos = new List<QuestionCourseDto>(questionDto),
                        HistorySubmitCourseDtos = new List<HistorySubmitCourseDto>(historySubmitCourseDto),
                        AnswerCourseDtos = new List<AnswerCourseDto>(answerCourseDto),
                        ReportScoreCourseDtos = new List<ReportScoreCourseDto>(reportScoreCourseDto),

                    };
                    return quizCourseDto;
                }

            }
        }

        public async Task<IEnumerable<QuizCourseDto>> GetQuizDtos()
        {
            var quizCourse = await _context.QuizCourses.Include(x => x.Course).Include(x => x.QuestionFinals).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            var quizCourseDto = _mapper.Map<List<QuizCourseDto>>(quizCourse);
            return quizCourseDto;
        }

        public async Task InsertQuizDto(CreateQuizCourseDto createQuizCourseDto)
        {
            if (createQuizCourseDto != null)
            {
                var code = _configuration.GetValue<string>("Code:QuizCourse");

                var key = code + Utilities.GenerateStringDateTime();
                createQuizCourseDto.Code = key;

                createQuizCourseDto.ID = Guid.NewGuid();

                createQuizCourseDto.DateCreated = DateTime.Now;

                createQuizCourseDto.Alias = Utilities.SEOUrl(createQuizCourseDto.Name);

            }
            QuizCourse quizCourse = _mapper.Map<QuizCourse>(createQuizCourseDto);

            if (_context.QuizCourses.Any(x => x.CourseId.Equals(createQuizCourseDto.CourseId) && x.Name.Equals(createQuizCourseDto.Name)) == false)
            {
                _context.Add(quizCourse);
                await _context.SaveChangesAsync();
            }


        }

        public async Task ResetQuizCourse(Guid quizCourseId, string accountName)
        {
            var quizCourse = await _context.AnswerCourses.Where(x => x.QuizCourseId.Equals(quizCourseId) && x.AccountName == accountName).ToListAsync();
            if (quizCourse != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.AnswerCourses.RemoveRange(quizCourse);
                await _context.SaveChangesAsync();

                var historySubmit = _context.HistorySubmitFinals.Where(x => x.QuizCourseId.Equals(quizCourseId) && x.AccountName == accountName);

                _context.HistorySubmitFinals.RemoveRange(historySubmit);

                await _context.SaveChangesAsync();



                var reportScore = _context.ReportUserScoreFinals.Where(x => x.QuizCourseId.Equals(quizCourseId) && x.UserName == accountName);
                _context.ReportUserScoreFinals.RemoveRange(reportScore);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }

        public async Task UpdateQuizDto(Guid id, UpdateQuizCourseDto updateQuizCourseDto)
        {
            updateQuizCourseDto.DateCreated = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();

            QuizCourse quiz = _mapper.Map(updateQuizCourseDto, await _context.QuizCourses.Include(x => x.Course).Include(x => x.QuestionFinals).FirstOrDefaultAsync(x => x.ID.Equals(id)));

            if (quiz != null)
            {
                _context.QuizCourses.Update(quiz);

                await _context.SaveChangesAsync();
            }
            var notificationResponeDb = _context.NotificationResponses.Where(x => x.TargetNotificationId.Equals(id));

            foreach (var item in notificationResponeDb)
            {
                if (item.Notify == false)
                {
                    UpdateNotificationResponseDto updateNotificationResponseDto = new();

                    updateNotificationResponseDto.Notify = true;
                    updateNotificationResponseDto.DateCreated = DateTime.Now;


                    NotificationResponse notificationResponseDto = _mapper.Map(updateNotificationResponseDto, item);

                    _context.NotificationResponses.Update(notificationResponseDto);

                    await _context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
        }
    }
}
