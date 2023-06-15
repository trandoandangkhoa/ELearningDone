using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IQuizLessionService
    {
        Task<IEnumerable<QuizlessionDto>> GetQuizLessionDtos();

        Task<QuizlessionDto> GetNameQuiz(Guid id);

        Task<QuizlessionDto> GetQuizLessionById(Guid id, string accountName);

        Task<QuizlessionDto> AdminGetQuizLessionById(Guid id);
        Task<QuizlessionDto> CheckExist(Guid id, string name);
        Task<int> FindIndex(Guid quizLessionId, Guid lessionId);

        Task<int> CheckQuizPassed(Guid quizLessionId, string accountName, Guid lessionId);

        Task InsertQuizLessionDto(CreateQuizLessionDto createQuizLessionDto);

        Task DeleteQuizLessionDto(Guid id);

        Task UpdateQuizLessionDto(Guid id, UpdateQuizLessionDto updateQuizLessionDto);

        Task<PagedViewModel<QuizlessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<QuizlessionDto> GetCode(string code);

    }
    public class QuizLessionService : IQuizLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public QuizLessionService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }
        public async Task DeleteQuizLessionDto(Guid id)
        {
            var quiz = await _context.QuizLessions.FindAsync(id);
            if (quiz != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.QuizLessions.Remove(quiz);

                await _context.SaveChangesAsync();

                var historySubmit = _context.HistorySubmitLessions.Where(x => x.QuizLessionId.Equals(quiz.ID));

                _context.HistorySubmitLessions.RemoveRange(historySubmit);

                await _context.SaveChangesAsync();



                var reportScore = _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(quiz.ID));
                _context.ReportUsersScore.RemoveRange(reportScore);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }

        public async Task<PagedViewModel<QuizlessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:QuizLession"));
            }
            var pageResult = getListPagingRequest.PageSize;

            var pageCount = Math.Ceiling(_context.QuizLessions.Count() / (double)pageResult);

            var query = _context.QuizLessions.AsNoTracking().AsQueryable();

            var questionLession = _context.QuestionLessions.AsNoTracking().AsQueryable();

            var historySubmitLession = _context.HistorySubmitLessions.AsNoTracking().AsQueryable();

            var answerLession = _context.AnswerLessions.AsNoTracking().AsQueryable();

            var reportScoreLession = _context.ReportUsersScore.AsNoTracking().AsQueryable();

            var questionDto = _mapper.Map<List<QuestionLessionDto>>(questionLession);

            var historySubmitLessionDto = _mapper.Map<List<HistorySubmitLessionDto>>(historySubmitLession);

            var answerLessionDto = _mapper.Map<List<AnswerLessionDto>>(answerLession);

            var reportScoreLessionDto = _mapper.Map<List<ReportScoreLessionDto>>(reportScoreLession);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Lession.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(quizLession => new QuizlessionDto()
                                    {
                                        ID = quizLession.ID,
                                        LessionId = quizLession.LessionId,
                                        Name = quizLession.Name,
                                        Description = quizLession.Description,
                                        Active = quizLession.Active,
                                        DateCreated = quizLession.DateCreated,
                                        TimeToDo = quizLession.TimeToDo,
                                        ScorePass = quizLession.ScorePass,
                                        Alias = quizLession.Alias,
                                        Code = quizLession.Code,
                                        LessionDto = new LessionDto()
                                        {
                                            Id = quizLession.Lession.Id,
                                            CourseId = quizLession.Lession.CourseId,
                                            Name = quizLession.Lession.Name,
                                            ShortDesc = quizLession.Lession.ShortDesc,
                                            Active = quizLession.Lession.Active,
                                            DateCreated = quizLession.Lession.DateCreated,
                                            Alias = quizLession.Lession.Alias,
                                            Author = quizLession.Lession.Author,
                                            Code = quizLession.Lession.Code,
                                        },
                                        QuestionLessionDtos = new List<QuestionLessionDto>(questionDto),
                                        HistorySubmitLessionDtos = new List<HistorySubmitLessionDto>(historySubmitLessionDto),
                                        AnswerLessionDtos = new List<AnswerLessionDto>(answerLessionDto),
                                        ReportScoreLessionDtos = new List<ReportScoreLessionDto>(reportScoreLessionDto),

                                    }).OrderByDescending(x => x.DateCreated).ToListAsync();
            var quizLessionResponse = new PagedViewModel<QuizlessionDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return quizLessionResponse;
        }
        public async Task<QuizlessionDto> GetQuizLessionById(Guid id, string accountName)
        {
            var quizLession = await _context.QuizLessions.Include(x => x.QuestionLessions).ThenInclude(x => x.Options).Include(x => x.Lession).ThenInclude(x => x.Courses).OrderBy(X => X.DateCreated).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (quizLession == null)
            {
                return default;
            }
            else
            {
                var historySubmitLession = await _context.HistorySubmitLessions.Where(x => x.QuizLessionId.Equals(quizLession.ID) && x.AccountName.Equals(accountName)).ToListAsync();

                var answerLession = await _context.AnswerLessions.Where(x => x.QuizLessionId.Equals(quizLession.ID) && x.AccountName.Equals(accountName)).ToListAsync();

                var reportScoreLession = await _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(quizLession.ID) && x.UserName.Equals(accountName)).ToListAsync();


                var questionDto = _mapper.Map<List<QuestionLessionDto>>(quizLession.QuestionLessions);

                var historySubmitLessionDto = _mapper.Map<List<HistorySubmitLessionDto>>(historySubmitLession);

                var answerLessionDto = _mapper.Map<List<AnswerLessionDto>>(answerLession);

                var reportScoreLessionDto = _mapper.Map<List<ReportScoreLessionDto>>(reportScoreLession);

                var courseDto = _mapper.Map<CourseDto>(quizLession.Lession.Courses);
                QuizlessionDto quizlessionDto = new()
                {
                    ID = quizLession.ID,
                    LessionId = quizLession.LessionId,
                    Name = quizLession.Name,
                    Description = quizLession.Description,
                    Active = quizLession.Active,
                    DateCreated = quizLession.DateCreated,
                    TimeToDo = quizLession.TimeToDo,
                    ScorePass = quizLession.ScorePass,
                    Alias = quizLession.Alias,
                    Code = quizLession.Code,
                    LessionDto = new LessionDto()
                    {
                        Name = quizLession.Lession.Name,
                        Alias = quizLession.Lession.Alias,
                        Id = quizLession.Lession.Id,
                        Code = quizLession.Lession.Code,
                        CourseId = quizLession.Lession.CourseId,
                        CourseDto = courseDto,
                    },
                    SortItem = quizLession.SortItem,
                    QuestionLessionDtos = new List<QuestionLessionDto>(questionDto),
                    HistorySubmitLessionDtos = new List<HistorySubmitLessionDto>(historySubmitLessionDto),
                    AnswerLessionDtos = new List<AnswerLessionDto>(answerLessionDto),
                    ReportScoreLessionDtos = new List<ReportScoreLessionDto>(reportScoreLessionDto),

                };

                return quizlessionDto;
            }
        }

        public async Task<IEnumerable<QuizlessionDto>> GetQuizLessionDtos()
        {
            var quizLession = await _context.QuizLessions.Include(x => x.Lession).Include(x => x.QuestionLessions).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            var quizLessionDto = _mapper.Map<List<QuizlessionDto>>(quizLession);
            return quizLessionDto;
        }

        public async Task InsertQuizLessionDto(CreateQuizLessionDto createQuizLessionDto)
        {
            var count = _context.QuizLessions.Where(x => x.LessionId.Equals(createQuizLessionDto.LessionId)).AsNoTracking().AsQueryable().Count();

            if (createQuizLessionDto != null)
            {
                var code = _configuration.GetValue<string>("Code:QuizLession");

                var key = code + Utilities.GenerateStringDateTime();

                createQuizLessionDto.Code = key;

                createQuizLessionDto.ID = Guid.NewGuid();

                createQuizLessionDto.SortItem = count;

                createQuizLessionDto.DateCreated = DateTime.Now;

                createQuizLessionDto.Alias = Utilities.SEOUrl(createQuizLessionDto.Name);

            }

            QuizLession quizLession = _mapper.Map<QuizLession>(createQuizLessionDto);

            quizLession.SortItem = createQuizLessionDto.SortItem;
            if (_context.QuizLessions.Any(x => x.LessionId.Equals(createQuizLessionDto.LessionId) && x.Name.Equals(createQuizLessionDto.Name)) == false)
            {
                _context.Add(quizLession);

                await _context.SaveChangesAsync();
            }

        }

        public async Task UpdateQuizLessionDto(Guid id, UpdateQuizLessionDto updateQuizLessionDto)
        {
            updateQuizLessionDto.DateCreated = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();

            QuizLession quiz = _mapper.Map(updateQuizLessionDto, await _context.QuizLessions.Include(x => x.Lession).Include(x => x.QuestionLessions).FirstOrDefaultAsync(x => x.ID.Equals(id)));

            if (quiz != null)
            {
                _context.QuizLessions.Update(quiz);

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
        public async Task<QuizlessionDto> GetNameQuiz(Guid id)
        {
            var name = await _context.QuizLessions.AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));

            var quizDto = _mapper.Map<QuizlessionDto>(name);

            return quizDto;
        }
        public async Task<QuizlessionDto> AdminGetQuizLessionById(Guid id)
        {
            var quizLession = await _context.QuizLessions.Include(x => x.QuestionLessions).Include(x => x.Lession).ThenInclude(x => x.Courses).OrderBy(X => X.DateCreated).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (quizLession == null)
            {
                return default;
            }
            else
            {
                var questionLession = _context.QuestionLessions.Include(x => x.CorrectAnswers).Where(x => x.QuizLessionId.Equals(quizLession.ID)).AsNoTracking().AsQueryable();

                var historySubmitLession = _context.HistorySubmitLessions.Where(x => x.QuizLessionId.Equals(quizLession.ID)).AsNoTracking().AsQueryable();

                var answerLession = _context.AnswerLessions.Where(x => x.QuizLessionId.Equals(quizLession.ID)).AsNoTracking().AsQueryable();

                var reportScoreLession = _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(quizLession.ID)).AsNoTracking().AsQueryable();

                var questionDto = _mapper.Map<List<QuestionLessionDto>>(questionLession);


                QuizlessionDto quizlessionDto = new()
                {
                    ID = quizLession.ID,
                    LessionId = quizLession.LessionId,
                    Name = quizLession.Name,
                    Description = quizLession.Description,
                    Active = quizLession.Active,
                    DateCreated = quizLession.DateCreated,
                    TimeToDo = quizLession.TimeToDo,
                    ScorePass = quizLession.ScorePass,
                    Alias = quizLession.Alias,
                    Code = quizLession.Code,
                    LessionDto = new LessionDto()
                    {
                        Id = quizLession.Lession.Id,
                        CourseId = quizLession.Lession.CourseId,
                        Name = quizLession.Lession.Name,
                        ShortDesc = quizLession.Lession.ShortDesc,
                        Active = quizLession.Lession.Active,
                        DateCreated = quizLession.Lession.DateCreated,
                        Alias = quizLession.Lession.Alias,
                        Author = quizLession.Lession.Author,
                        Code = quizLession.Lession.Code,
                        CourseDto = new CourseDto()
                        {
                            CreatedBy = quizLession.Lession.Courses.CreatedBy,
                        },
                    },
                    SortItem = quizLession.SortItem,
                    QuestionLessionDtos = new List<QuestionLessionDto>(questionDto),

                    //HistorySubmitLessionDtos = new List<HistorySubmitLessionDto>(historySubmitLessionDto),
                    //AnswerLessionDtos = new List<AnswerLessionDto>(answerLessionDto),
                    //ReportScoreLessionDtos = new List<ReportScoreLessionDto>(reportScoreLessionDto),

                };
                return quizlessionDto;
            }
        }

        public async Task<int> FindIndex(Guid quizLessionId, Guid lessionId)
        {
            var quiz = await _context.QuizLessions.Where(x => x.LessionId.Equals(lessionId)).AsNoTracking().OrderBy(x => x.DateCreated).ToListAsync();

            var quizDto = _mapper.Map<List<QuizlessionDto>>(quiz);

            List<QuizlessionDto> reportScoreLessionDtos = new List<QuizlessionDto>(quizDto);

            int index = reportScoreLessionDtos.FindIndex(a => a.ID.ToString().Contains(quizLessionId.ToString()));


            return index;
        }

        public async Task<int> CheckQuizPassed(Guid quizLessionId, string accountName, Guid lessionId)
        {
            var currentQuiz = FindIndex(quizLessionId, lessionId);

            var ints = new List<int>();

            var totalPass = 0;

            var bools = new List<bool>();

            for (int i = 0; i < Convert.ToInt32(currentQuiz.Result); i++)
            {

                ints.Add(i);

                var quiz = _context.QuizLessions.AsNoTracking().OrderBy(x => x.DateCreated).ToList();

                var quizDto = _mapper.Map<List<QuizlessionDto>>(quiz);

                var checkPass = await _context.QuizLessions.FirstOrDefaultAsync(x => x.SortItem.Equals(i) && x.LessionId.Equals(lessionId));

                var reportScoreLession = _context.ReportUsersScore.AsNoTracking().AsQueryable();

                if (reportScoreLession.Any(item => item.QuizLessionId.Equals(checkPass.ID) && item.UserName.Equals(accountName) && item.Passed == true))
                {
                    totalPass++;

                }
            }
            return totalPass;
        }

        public async Task<QuizlessionDto> GetCode(string code)
        {
            var account = await _context.QuizLessions.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuizlessionDto>(account);
        }

        public async Task<QuizlessionDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuizLessions.FirstOrDefaultAsync(x => x.LessionId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuizlessionDto>(account);
        }
    }
}
