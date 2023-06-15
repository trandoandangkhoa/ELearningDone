using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IQuestionLessionService
    {
        Task<IEnumerable<QuestionLessionDto>> GetQuestionDtos();

        Task<QuestionLessionDto> GetQuestionById(Guid id, string accountName);

        Task UpdateConcerningQuestionLessionDto(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto);

        Task InsertConcerningQuestionLessionDto(CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto);

        Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto);
        Task<QuestionLessionDto> GetNameQuestion(string name);
        Task<QuestionLessionDto> CheckExist(Guid id, string name);
        Task DeleteQuestionDto(Guid id);

        Task<PagedViewModel<QuestionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<IEnumerable<QuestionLessionDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest);

        Task AddQuestion(QuestionLessionDto questionLessionDto);
        Task<QuestionLessionDto> GetCode(string code);
        Task AddQuestionWithQuizId(Guid QuizLessionId, CreateQuestionLessionDto createQuestionLessionDto);
    }
    public class QuestionLessionService : IQuestionLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        public QuestionLessionService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _environment = environment;

        }

        public Task AddQuestion(QuestionLessionDto questionLessionDto)
        {
            throw new NotImplementedException();
        }

        public async Task AddQuestionWithQuizId(Guid QuizLessionId, CreateQuestionLessionDto createQuestionLessionDto)
        {
            createQuestionLessionDto.Alias = Utilities.SEOUrl(createQuestionLessionDto.Name);

            QuestionLession questionLession = _mapper.Map<QuestionLession>(createQuestionLessionDto);



            //questionLession.Alias = createQuestionLessionDto.Alias;

            //questionLession.QuizLessionId = QuizLessionId;

            _context.Add(questionLession);

            await _context.SaveChangesAsync();
        }

        public async Task<QuestionLessionDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuestionLessions.FirstOrDefaultAsync(x => x.QuizLessionId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuestionLessionDto>(account);
        }

        public async Task DeleteQuestionDto(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var question = await _context.QuestionLessions.FindAsync(id);
            _context.QuestionLessions.Remove(question);

            await _context.SaveChangesAsync();


            //Update Submit false in userTest
            var historySubmit = _context.HistorySubmitLessions.Where(x => x.QuizLessionId.Equals(question.QuizLessionId));

            _context.HistorySubmitLessions.RemoveRange(historySubmit);
            await _context.SaveChangesAsync();



            var reportScore = _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(question.QuizLessionId));
            _context.ReportUsersScore.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<QuestionLessionDto> GetCode(string code)
        {
            var account = await _context.QuestionLessions.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuestionLessionDto>(account);
        }

        public async Task<QuestionLessionDto> GetNameQuestion(string name)
        {
            var account = await _context.QuestionLessions.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return _mapper.Map<QuestionLessionDto>(account);
        }

        public async Task<PagedViewModel<QuestionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:QuestionLession"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.QuestionLessions.Count() / (double)pageResult);
            var query = _context.QuestionLessions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.QuizLession.Name.Contains(getListPagingRequest.Keyword) || x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();


            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new QuestionLessionDto()
                                    {
                                        Id = x.Id,
                                        QuizLessionId = x.QuizLessionId,
                                        Name = x.Name,
                                        Active = x.Active,
                                        Point = x.Point,
                                        Alias = x.Alias,
                                        Code = x.Code,
                                        QuizlessionDto = new QuizlessionDto()
                                        {
                                            ID = x.QuizLession.ID,
                                            LessionId = x.QuizLession.LessionId,
                                            Name = x.QuizLession.Name,
                                            Description = x.QuizLession.Description,
                                            Active = x.QuizLession.Active,
                                            DateCreated = x.QuizLession.DateCreated,
                                            TimeToDo = x.QuizLession.TimeToDo,
                                            ScorePass = x.QuizLession.ScorePass,
                                            Code = x.QuizLession.Code,
                                        },

                                    })
                                    .OrderBy(x => x.QuizlessionDto.DateCreated).ToListAsync();
            var roleResponse = new PagedViewModel<QuestionLessionDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return roleResponse;
        }

        public Task<IEnumerable<QuestionLessionDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionLessionDto> GetQuestionById(Guid id, string accountName)
        {
            var question = await _context.QuestionLessions.Include(x => x.QuizLession).Include(x => x.Options).Include(x => x.CorrectAnswers).OrderBy(x => x.Id).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            //var answerQuestionLession = _context.AnswerLessions.Where(x => x.QuestionLessionId.Equals(id)).AsNoTracking().AsQueryable();

            if (question == null) return default;

            var questionDto = _mapper.Map<QuestionLessionDto>(question);

            return questionDto;

            //var allAnswerDto = _mapper.Map<List<AnswerLessionDto>>(answerQuestionLession);

            //if (answerQuestionLession == null) return default;

            //if (accountName == null) return questionDto;

            //var answer = answerQuestionLession.Where(x => x.AccountName.Equals(accountName)).AsNoTracking().AsQueryable();

            //if (answer == null) return questionDto;

            //var userAnswerDto = _mapper.Map<List<AnswerLessionDto>>(answer);

            //questionDto.AnswerLessionDtos = userAnswerDto;

            //return questionDto;

        }


        public async Task<IEnumerable<QuestionLessionDto>> GetQuestionDtos()
        {
            var question = await _context.QuestionLessions.Include(x => x.QuizLession).Where(x => x.Active == true).AsNoTracking().ToListAsync();
            var questionDto = _mapper.Map<List<QuestionLessionDto>>(question);
            return questionDto;
        }


        public async Task InsertConcerningQuestionLessionDto(CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Alias = Utilities.SEOUrl(createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Name);
            var code = _configuration.GetValue<string>("Code:QuestionLession");

            var key = code + Utilities.GenerateStringDateTime();

            createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Code = key;

            createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.DateCreated = DateTime.Now;

            QuestionLession question = _mapper.Map<QuestionLession>(createAllConcerningQuestionLessionDto.CreateQuestionLessionDto);

            var exist = _context.QuestionLessions.Any(x => x.QuizLessionId.Equals(createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.QuizLessionId) && x.Name.Equals(createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Name));

            if (exist == false)
            {
                _context.QuestionLessions.Add(question);

                await _context.SaveChangesAsync();
            }


            if (createAllConcerningQuestionLessionDto.OptionLessions != null)
            {

                for (int i = 0; i < createAllConcerningQuestionLessionDto.OptionLessions.Length; i++)
                {
                    for (int j = i; j < createAllConcerningQuestionLessionDto.CorrectAnswers.Length;)
                    {
                        createAllConcerningQuestionLessionDto.OptionAndCorrectLessions.Add(new OptionAndCorrectLession
                        {
                            OptionLessions = createAllConcerningQuestionLessionDto.OptionLessions[i],
                            CorrectAnswers = createAllConcerningQuestionLessionDto.CorrectAnswers[j],
                        });

                        break;
                    }
                }

                foreach (var item in createAllConcerningQuestionLessionDto.OptionAndCorrectLessions)
                {
                    createAllConcerningQuestionLessionDto.CreateOptionLessionDto.Id = Guid.NewGuid();

                    createAllConcerningQuestionLessionDto.CreateOptionLessionDto.QuestionLessionId = createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Id;

                    createAllConcerningQuestionLessionDto.CreateOptionLessionDto.Name = item.OptionLessions;

                    OptionLession optionLessionDto = _mapper.Map<OptionLession>(createAllConcerningQuestionLessionDto.CreateOptionLessionDto);

                    if (_context.OptionLessions.Any(x => x.QuestionLessionId.Equals(createAllConcerningQuestionLessionDto.CreateOptionLessionDto.QuestionLessionId) && x.Name.Equals(createAllConcerningQuestionLessionDto.CreateOptionLessionDto.Name)) == false)
                    {
                        _context.OptionLessions.Add(optionLessionDto);

                        await _context.SaveChangesAsync();
                    }


                    if (item.CorrectAnswers == true)
                    {
                        createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.Id = Guid.NewGuid();

                        createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.OptionLessionId = createAllConcerningQuestionLessionDto.CreateOptionLessionDto.Id;

                        createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.QuestionLessionId = createAllConcerningQuestionLessionDto.CreateQuestionLessionDto.Id;

                        createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.CorrectAnswer = createAllConcerningQuestionLessionDto.CreateOptionLessionDto.Name;

                        CorrectAnswerLession correctAnswerLession = _mapper.Map<CorrectAnswerLession>(createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto);

                        if (_context.CorrectAnswerLessions.Any(x => x.QuestionLessionId.Equals(createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.QuestionLessionId) && x.CorrectAnswer.Equals(createAllConcerningQuestionLessionDto.CreateCorrectAnswerLessionDto.CorrectAnswer)) == false)
                        {
                            _context.CorrectAnswerLessions.Add(correctAnswerLession);

                            await _context.SaveChangesAsync();
                        }

                    }

                }
            }




            await transaction.CommitAsync();

        }

        public async Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            if (updateAllConcerningQuestionLesstionDto.NewOptionLessions.Length > 0)
            {
                for (int i = 0; i < updateAllConcerningQuestionLesstionDto.NewOptionLessions.Length; i++)
                {
                    for (int j = i; j < updateAllConcerningQuestionLesstionDto.NewCorrectAnswers.Length;)
                    {
                        updateAllConcerningQuestionLesstionDto.NewOptionAndCorrectLessionDtos.Add(new NewOptionAndCorrectLessionDto
                        {
                            NewOptionLessions = updateAllConcerningQuestionLesstionDto.NewOptionLessions[i],
                            NewCorrectAnswers = updateAllConcerningQuestionLesstionDto.NewCorrectAnswers[j],
                        });

                        break;
                    }
                }

                foreach (var item in updateAllConcerningQuestionLesstionDto.NewOptionAndCorrectLessionDtos)
                {
                    CreateOptionLessionDto createOptionLessionDto = new();

                    var existOptionsDb = _context.OptionLessions.Any(x => x.QuestionLessionId.Equals(id) && x.Name.Equals(item.NewOptionLessions) == true);

                    if (existOptionsDb == false)
                    {
                        createOptionLessionDto.Id = Guid.NewGuid();

                        createOptionLessionDto.QuestionLessionId = id;

                        createOptionLessionDto.Name = item.NewOptionLessions;

                        OptionLession optionLessionDto = _mapper.Map<OptionLession>(createOptionLessionDto);

                        if (_context.OptionLessions.Any(x => x.QuestionLessionId.Equals(id) && x.Name.Equals(createOptionLessionDto.Name) == false))
                        {
                            _context.OptionLessions.Add(optionLessionDto);

                            await _context.SaveChangesAsync();
                        }

                    }



                    if (item.NewCorrectAnswers == true)
                    {
                        var existCorrectsDb = _context.CorrectAnswerLessions.Any(x => x.QuestionLessionId.Equals(id) && x.CorrectAnswer.Equals(item.NewCorrectAnswers) == true);

                        if (existOptionsDb == false)
                        {
                            CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto = new();

                            createCorrectAnswerLessionDto.Id = Guid.NewGuid();

                            createCorrectAnswerLessionDto.OptionLessionId = createOptionLessionDto.Id;

                            createCorrectAnswerLessionDto.QuestionLessionId = id;

                            createCorrectAnswerLessionDto.CorrectAnswer = createOptionLessionDto.Name;

                            CorrectAnswerLession correctAnswerLession = _mapper.Map<CorrectAnswerLession>(createCorrectAnswerLessionDto);
                            if (_context.CorrectAnswerLessions.Any(x => x.QuestionLessionId.Equals(id) && x.CorrectAnswer.Equals(createCorrectAnswerLessionDto.CorrectAnswer) == false))
                            {
                                _context.CorrectAnswerLessions.Add(correctAnswerLession);

                                await _context.SaveChangesAsync();
                            }

                        }


                    }

                }
                await transaction.CommitAsync();

            }
        }

        public async Task UpdateConcerningQuestionLessionDto(Guid id, UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            for (int i = 0; i < updateAllConcerningQuestionLesstionDto.OptionLessionId.Length; i++)
            {
                for (int j = i; j < updateAllConcerningQuestionLesstionDto.OptionLessions.Length;)
                {
                    updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.OptionLessionDtos.Add(new OptionLessionDto
                    {
                        Id = updateAllConcerningQuestionLesstionDto.OptionLessionId[i],
                        QuestionLessionId = id,
                        Name = updateAllConcerningQuestionLesstionDto.OptionLessions[j],

                    });
                    for (int k = i; k < updateAllConcerningQuestionLesstionDto.CorrectAnswerId.Length;)
                    {
                        for (int l = k; j < updateAllConcerningQuestionLesstionDto.CorrectAnswers.Length;)
                        {

                            updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.CorrectAnswerLessionDtos.Add(new CorrectAnswerLessionDto
                            {
                                Id = updateAllConcerningQuestionLesstionDto.CorrectAnswerId[k],

                                QuestionLessionId = id,

                                OptionLessionId = updateAllConcerningQuestionLesstionDto.OptionLessionId[i],

                                CorrectAnswer = updateAllConcerningQuestionLesstionDto.OptionLessions[j],

                                Correct = updateAllConcerningQuestionLesstionDto.CorrectAnswers[l],
                            });

                            break;
                        }
                        break;
                    }
                    break;
                }

            }

            updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.Alias = Utilities.SEOUrl(updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.Name);
            updateAllConcerningQuestionLesstionDto.UpdateQuestionLession.DateCreated = DateTime.Now;

            QuestionLession question = _mapper.Map(updateAllConcerningQuestionLesstionDto.UpdateQuestionLession, await _context.QuestionLessions.FirstOrDefaultAsync(x => x.Id.Equals(id)));

            _context.QuestionLessions.Update(question);

            await _context.SaveChangesAsync();

            var correctDb = _context.CorrectAnswerLessions.Where(x => x.QuestionLessionId.Equals(id)).AsNoTracking().AsQueryable();

            var optionDb = _context.OptionLessions.Where(x => x.QuestionLessionId.Equals(id)).AsNoTracking().AsQueryable();

            var correctTrue = updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.CorrectAnswerLessionDtos.Where(x => x.Correct == true).ToList();

            var correctFalse = updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.CorrectAnswerLessionDtos.Where(x => x.Correct == false).ToList();

            var correctAnswerLessionDto = _mapper.Map<List<CorrectAnswerLessionDto>>(correctDb);

            var optionLessionDto = _mapper.Map<List<OptionLessionDto>>(optionDb);

            var correctTrueDifference = correctTrue.Except(correctAnswerLessionDto).ToList();

            var correctFalseDifference = correctFalse.Except(correctAnswerLessionDto).ToList();

            var optionDifference = updateAllConcerningQuestionLesstionDto.OptionAndCorrectLessionDto.OptionLessionDtos.Where(x => !optionLessionDto.Any(a => a.Name.Equals(x.Name) && a.Id.Equals(x.Id) == true)).ToList();


            var correctFalseExistDb = correctAnswerLessionDto.Where(x => correctFalse.Any(a => a.Id.Equals(x.Id))).ToList();

            if (correctFalseExistDb.Count > 0)
            {
                List<CorrectAnswerLession> correctAnswerLession = _mapper.Map<List<CorrectAnswerLession>>(correctFalseExistDb);

                _context.CorrectAnswerLessions.RemoveRange(correctAnswerLession);

                await _context.SaveChangesAsync();

            }

            if (optionDifference.Count > 0)
            {
                foreach (var item in optionDifference)
                {
                    if (optionDifference.Any(x => x.Id.Equals(item.Id)) == true)
                    {
                        OptionLession optionLession = _mapper.Map<OptionLession>(item);

                        _context.OptionLessions.Update(optionLession);

                        await _context.SaveChangesAsync();
                    }

                }
            }

            if (correctTrueDifference.Count > 0)
            {
                foreach (var correctAnswer in correctTrueDifference)
                {
                    if (correctAnswerLessionDto.Any(x => x.Id.Equals(correctAnswer.Id) && x.CorrectAnswer.Equals(correctAnswer.CorrectAnswer)) == false)
                    {

                        CorrectAnswerLession correctAnswerLession = _mapper.Map<CorrectAnswerLession>(correctAnswer);

                        correctAnswer.Id = Guid.NewGuid();

                        _context.CorrectAnswerLessions.Add(correctAnswerLession);

                        await _context.SaveChangesAsync();
                    }
                    else if (optionDifference.Count > 0)
                    {
                        foreach (var item in optionDifference)
                        {
                            if (correctAnswer.OptionLessionId.Equals(item.Id) == true)
                            {
                                correctAnswer.CorrectAnswer = item.Name;

                                CorrectAnswerLession correctAnswerLession = _mapper.Map<CorrectAnswerLession>(correctAnswer);

                                _context.CorrectAnswerLessions.Update(correctAnswerLession);

                                await _context.SaveChangesAsync();
                            }

                        }
                    }
                }
            }

            await transaction.CommitAsync();

        }



    }
}
