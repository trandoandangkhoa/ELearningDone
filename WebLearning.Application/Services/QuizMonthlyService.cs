using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IQuizMonthlyService
    {
        Task<IEnumerable<QuizMonthlyDto>> GetQuizDtos();

        Task<QuizMonthlyDto> GetQuizById(Guid id, string accountName);

        Task<QuizMonthlyDto> AdminGetQuizMonthlyById(Guid id);
        Task<QuizMonthlyDto> GetNameQuiz(Guid id);

        Task<IEnumerable<QuizMonthlyDto>> GetOwnQuizDtos(Guid roleId);

        Task InsertQuizDto(CreateQuizMonthlyDto createQuizMonthlyDto);

        Task DeleteQuizDto(Guid id);

        Task UpdateQuizDto(Guid id, UpdateQuizMonthlyDto updateQuizMonthlyDto);
        Task<QuizMonthlyDto> GetCode(string code);
        Task<PagedViewModel<QuizMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<QuizMonthlyDto> CheckExist(Guid id, string name);

        Task<IEnumerable<QuizMonthlyDto>> GetQuizByCategory(GetListPagingRequest getListPagingRequest);
    }
    public class QuizMonthlyService : IQuizMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public QuizMonthlyService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }
        public async Task<QuizMonthlyDto> AdminGetQuizMonthlyById(Guid id)
        {
            var quizMonthly = await _context.QuizMonthlies.Include(x => x.QuestionMonthlies).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));

            var correct = await _context.CorrectAnswerMonthlies.ToListAsync();


            var correctDto = _mapper.Map<List<CorrectAnswerMonthlyDto>>(correct);

            var quizMonthlyDto = _mapper.Map<QuizMonthlyDto>(quizMonthly);

            foreach (var cr in quizMonthlyDto.QuestionMonthlyDtos)
            {
                foreach (var cre in correctDto.Where(x => x.QuestionMonthlyId.Equals(cr.Id)))
                {
                    cr.CorrectAnswerMonthlyDtos.Add(cre);
                }
                continue;
            };
            return quizMonthlyDto;
        }

        public async Task<QuizMonthlyDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuizMonthlies.FirstOrDefaultAsync(x => x.RoleId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuizMonthlyDto>(account);
        }

        public async Task DeleteQuizDto(Guid id)
        {
            var quizMonthly = await _context.QuizMonthlies.FindAsync(id);
            if (quizMonthly != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.QuizMonthlies.Remove(quizMonthly);


                await _context.SaveChangesAsync();

                var historySubmit = _context.HistorySubmitMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID));

                _context.HistorySubmitMonthlies.RemoveRange(historySubmit);

                await _context.SaveChangesAsync();



                var reportScore = _context.ReportUserScoreMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID));
                _context.ReportUserScoreMonthlies.RemoveRange(reportScore);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }

        public async Task<QuizMonthlyDto> GetCode(string code)
        {
            var account = await _context.QuizMonthlies.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuizMonthlyDto>(account);
        }

        public async Task<QuizMonthlyDto> GetNameQuiz(Guid id)
        {
            var name = await _context.QuizMonthlies.AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));

            var quizDto = _mapper.Map<QuizMonthlyDto>(name);

            return quizDto;
        }

        public async Task<IEnumerable<QuizMonthlyDto>> GetOwnQuizDtos(Guid roleId)
        {
            var quizMonthly = await _context.QuizMonthlies.Include(x => x.Role).Include(x => x.QuestionMonthlies).ThenInclude(x => x.Options).OrderByDescending(x => x.DateCreated).Where(x => x.RoleId.Equals(roleId)).AsNoTracking().ToListAsync();
            var quizMonthlyDto = _mapper.Map<List<QuizMonthlyDto>>(quizMonthly);
            return quizMonthlyDto;
        }

        public async Task<PagedViewModel<QuizMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:QuizMonthly");

            var pageCount = Math.Ceiling(_context.QuizMonthlies.Count() / (double)pageResult);

            var query = _context.QuizMonthlies.Include(x => x.Role).AsNoTracking().AsQueryable();

            var questionMonthly = _context.QuestionMonthlies.AsNoTracking().AsQueryable();

            var historySubmitMonthly = _context.HistorySubmitMonthlies.AsNoTracking().AsQueryable();

            var answerMonthly = _context.AnswerMonthlies.AsNoTracking().AsQueryable();

            var reportScoreMonthly = _context.ReportUserScoreMonthlies.AsNoTracking().AsQueryable();

            var questionDto = _mapper.Map<List<QuestionMonthlyDto>>(questionMonthly);

            var historySubmitMonthlyDto = _mapper.Map<List<HistorySubmitMonthlyDto>>(historySubmitMonthly);

            var answerMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerMonthly);

            var reportScoreMonthlyDto = _mapper.Map<List<ReportScoreMonthlyDto>>(reportScoreMonthly);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Role.RoleName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(quizMonthly => new QuizMonthlyDto()
                                    {
                                        ID = quizMonthly.ID,
                                        RoleId = quizMonthly.RoleId,
                                        Name = quizMonthly.Name,
                                        Description = quizMonthly.Description,
                                        Active = quizMonthly.Active,
                                        DateCreated = quizMonthly.DateCreated,
                                        TimeToDo = quizMonthly.TimeToDo,
                                        ScorePass = quizMonthly.ScorePass,
                                        Alias = quizMonthly.Alias,
                                        Code = quizMonthly.Code,
                                        RoleDto = new RoleDto()
                                        {
                                            Id = quizMonthly.Role.Id,
                                            RoleName = quizMonthly.Role.RoleName,
                                            Description = quizMonthly.Role.Description,
                                            Code = quizMonthly.Code,

                                        },
                                        QuestionMonthlyDtos = new List<QuestionMonthlyDto>(questionDto),
                                        HistorySubmitMonthlyDtos = new List<HistorySubmitMonthlyDto>(historySubmitMonthlyDto),
                                        AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerMonthlyDto),
                                        ReportScoreMonthlyDtos = new List<ReportScoreMonthlyDto>(reportScoreMonthlyDto),

                                    }).OrderByDescending(x => x.DateCreated).ToListAsync();
            var quizMonthlyResponse = new PagedViewModel<QuizMonthlyDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return quizMonthlyResponse;
        }

        public Task<IEnumerable<QuizMonthlyDto>> GetQuizByCategory(GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<QuizMonthlyDto> GetQuizById(Guid id, string accountName)
        {
            var quizMonthly = await _context.QuizMonthlies.Include(x => x.QuestionMonthlies).ThenInclude(x => x.Options).Include(x => x.Role).OrderBy(x => x.DateCreated).AsNoTracking().FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (quizMonthly == null)
            {
                return default;
            }
            else
            {
                if (accountName != null)
                {
                    var questionMonthly = quizMonthly.QuestionMonthlies.ToList();

                    var historySubmitMonthly = await _context.HistorySubmitMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID) && x.AccountName.Equals(accountName)).ToListAsync();

                    var answerMonthly = await _context.AnswerMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID) && x.AccountName.Equals(accountName)).ToListAsync();

                    var reportScoreMonthly = await _context.ReportUserScoreMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID) && x.UserName.Equals(accountName)).ToListAsync();

                    var questionDto = _mapper.Map<List<QuestionMonthlyDto>>(questionMonthly);

                    var historySubmitMonthlyDto = _mapper.Map<List<HistorySubmitMonthlyDto>>(historySubmitMonthly);

                    var answerMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerMonthly);

                    var reportScoreMonthlyDto = _mapper.Map<List<ReportScoreMonthlyDto>>(reportScoreMonthly);

                    QuizMonthlyDto quizMonthlyDto = new QuizMonthlyDto()
                    {
                        ID = quizMonthly.ID,
                        RoleId = quizMonthly.RoleId,
                        Name = quizMonthly.Name,
                        Description = quizMonthly.Description,
                        Active = quizMonthly.Active,
                        DateCreated = quizMonthly.DateCreated,
                        TimeToDo = quizMonthly.TimeToDo,
                        ScorePass = quizMonthly.ScorePass,
                        Alias = quizMonthly.Alias,
                        Code = quizMonthly.Code,
                        RoleDto = new RoleDto()
                        {
                            Id = quizMonthly.Role.Id,
                            RoleName = quizMonthly.Role.RoleName,
                            Description = quizMonthly.Role.Description,
                            Code = quizMonthly.Role.Code
                        },
                        QuestionMonthlyDtos = new List<QuestionMonthlyDto>(questionDto),
                        HistorySubmitMonthlyDtos = new List<HistorySubmitMonthlyDto>(historySubmitMonthlyDto),
                        AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerMonthlyDto),
                        ReportScoreMonthlyDtos = new List<ReportScoreMonthlyDto>(reportScoreMonthlyDto),

                    };
                    return quizMonthlyDto;
                }
                else
                {
                    var questionMonthly = _context.QuestionMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID)).OrderBy(x => Guid.NewGuid()).AsNoTracking().AsQueryable();

                    var historySubmitMonthly = _context.HistorySubmitMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID)).AsNoTracking().AsQueryable();

                    var answerMonthly = _context.AnswerMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID)).AsNoTracking().AsQueryable();

                    var reportScoreMonthly = _context.ReportUserScoreMonthlies.Where(x => x.QuizMonthlyId.Equals(quizMonthly.ID)).AsNoTracking().AsQueryable();

                    var questionDto = _mapper.Map<List<QuestionMonthlyDto>>(questionMonthly);

                    var historySubmitMonthlyDto = _mapper.Map<List<HistorySubmitMonthlyDto>>(historySubmitMonthly);

                    var answerMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerMonthly);

                    var reportScoreMonthlyDto = _mapper.Map<List<ReportScoreMonthlyDto>>(reportScoreMonthly);

                    QuizMonthlyDto quizMonthlyDto = new()
                    {
                        ID = quizMonthly.ID,
                        RoleId = quizMonthly.RoleId,
                        Name = quizMonthly.Name,
                        Description = quizMonthly.Description,
                        Active = quizMonthly.Active,
                        DateCreated = quizMonthly.DateCreated,
                        TimeToDo = quizMonthly.TimeToDo,
                        ScorePass = quizMonthly.ScorePass,
                        Alias = quizMonthly.Alias,
                        Code = quizMonthly.Code,
                        RoleDto = new RoleDto()
                        {
                            Id = quizMonthly.Role.Id,
                            RoleName = quizMonthly.Role.RoleName,
                            Description = quizMonthly.Role.Description,
                            Code = quizMonthly.Role.Code,
                        },
                        QuestionMonthlyDtos = new List<QuestionMonthlyDto>(questionDto),
                        HistorySubmitMonthlyDtos = new List<HistorySubmitMonthlyDto>(historySubmitMonthlyDto),
                        AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerMonthlyDto),
                        ReportScoreMonthlyDtos = new List<ReportScoreMonthlyDto>(reportScoreMonthlyDto),

                    };
                    return quizMonthlyDto;
                }

            }
        }

        public async Task<IEnumerable<QuizMonthlyDto>> GetQuizDtos()
        {
            var quizMonthly = await _context.QuizMonthlies.Include(x => x.Role).Include(x => x.QuestionMonthlies).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            var quizMonthlyDto = _mapper.Map<List<QuizMonthlyDto>>(quizMonthly);
            return quizMonthlyDto;
        }

        public async Task InsertQuizDto(CreateQuizMonthlyDto createQuizMonthlyDto)
        {
            if (createQuizMonthlyDto != null)
            {
                createQuizMonthlyDto.ID = Guid.NewGuid();
                var code = _configuration.GetValue<string>("Code:QuizMonthly");


                var key = code + Utilities.GenerateStringDateTime();

                createQuizMonthlyDto.Code = key;

                createQuizMonthlyDto.DateCreated = DateTime.Now;

                createQuizMonthlyDto.Alias = Utilities.SEOUrl(createQuizMonthlyDto.Name);

            }
            QuizMonthly quizMonthly = _mapper.Map<QuizMonthly>(createQuizMonthlyDto);

            if (_context.QuizMonthlies.Any(x => x.RoleId.Equals(createQuizMonthlyDto.RoleId) && x.Name.Equals(createQuizMonthlyDto.Name)) == false)
            {
                _context.Add(quizMonthly);
                await _context.SaveChangesAsync();
            }



        }

        public async Task UpdateQuizDto(Guid id, UpdateQuizMonthlyDto updateQuizMonthlyDto)
        {
            updateQuizMonthlyDto.DateCreated = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();

            QuizMonthly quiz = (_mapper.Map(updateQuizMonthlyDto, await _context.QuizMonthlies.Include(x => x.Role).Include(x => x.QuestionMonthlies).FirstOrDefaultAsync(x => x.ID.Equals(id))));

            if (quiz != null)
            {
                updateQuizMonthlyDto.Alias = Utilities.SEOUrl(updateQuizMonthlyDto.Name);
                _context.QuizMonthlies.Update(quiz);

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


                    NotificationResponse notificationResponseDto = (_mapper.Map(updateNotificationResponseDto, item));

                    _context.NotificationResponses.Update(notificationResponseDto);

                    await _context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
        }
    }
}
