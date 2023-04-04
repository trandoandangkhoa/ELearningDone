using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IQuestionMonthlyService
    {
        Task<IEnumerable<QuestionMonthlyDto>> GetQuestionDtos();

        Task<QuestionMonthlyDto> GetQuestionById(Guid id, string accountName);

        Task UpdateConcerningQuestionMonthlyDto(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto);

        Task InsertConcerningQuestionMonthlyDto(CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto);

        Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto);
        Task<QuestionMonthlyDto> CheckExist(Guid id, string name);

        Task DeleteQuestionDto(Guid id);
        Task<QuestionMonthlyDto> GetNameQuestion(string name);
        Task<PagedViewModel<QuestionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<QuestionMonthlyDto> GetCode(string code);
        Task<IEnumerable<QuestionMonthlyDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest);
    }
    public class QuestionMonthlyService : IQuestionMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public QuestionMonthlyService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task<QuestionMonthlyDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuestionMonthlies.FirstOrDefaultAsync(x => x.QuizMonthlyId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuestionMonthlyDto>(account);
        }

        public async Task DeleteQuestionDto(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var question = await _context.QuestionMonthlies.FindAsync(id);
            _context.QuestionMonthlies.Remove(question);

            await _context.SaveChangesAsync();


            //Update Submit false in userTest
            var historySubmit = _context.HistorySubmitMonthlies.Where(x => x.QuizMonthlyId.Equals(question.QuizMonthlyId));

            _context.HistorySubmitMonthlies.RemoveRange(historySubmit);
            await _context.SaveChangesAsync();



            var reportScore = _context.ReportUserScoreMonthlies.Where(x => x.QuizMonthlyId.Equals(question.QuizMonthlyId));
            _context.ReportUserScoreMonthlies.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<QuestionMonthlyDto> GetCode(string code)
        {
            var account = await _context.QuestionMonthlies.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuestionMonthlyDto>(account);
        }
        public async Task<QuestionMonthlyDto> GetNameQuestion(string name)
        {
            var account = await _context.QuestionMonthlies.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return _mapper.Map<QuestionMonthlyDto>(account);
        }
        public async Task<PagedViewModel<QuestionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:QuestionMonthly");
            var pageCount = Math.Ceiling(_context.QuestionMonthlies.Count() / (double)pageResult);
            var query = _context.QuestionMonthlies.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.QuizMonthly.Name.Contains(getListPagingRequest.Keyword) || x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new QuestionMonthlyDto()
                                    {
                                        Id = x.Id,
                                        QuizMonthlyId = x.QuizMonthlyId,
                                        Name = x.Name,
                                        Active = x.Active,
                                        Point = x.Point,
                                        Alias = x.Alias,
                                        Code = x.Code,
                                        QuizMonthlyDto = new QuizMonthlyDto()
                                        {
                                            ID = x.QuizMonthly.ID,
                                            RoleId = x.QuizMonthly.Role.Id,
                                            Name = x.QuizMonthly.Name,
                                            Description = x.QuizMonthly.Description,
                                            Active = x.QuizMonthly.Active,
                                            DateCreated = x.QuizMonthly.DateCreated,
                                            TimeToDo = x.QuizMonthly.TimeToDo,
                                            ScorePass = x.QuizMonthly.ScorePass,
                                            Code = x.QuizMonthly.Code,
                                        },

                                    })
                                    .OrderByDescending(x => x.Code).ToListAsync();
            var questionMonthlyResponse = new PagedViewModel<QuestionMonthlyDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return questionMonthlyResponse;
        }

        public Task<IEnumerable<QuestionMonthlyDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionMonthlyDto> GetQuestionById(Guid id, string accountName)
        {
            var question = await _context.QuestionMonthlies.Include(x => x.Options).Include(x => x.CorrectAnswers).Include(x => x.QuizMonthly).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            var optionLession = await _context.OptionMonthlies.AsNoTracking().Where(x => x.QuestionMonthlyId.Equals(question.Id)).ToListAsync();

            var correctAnswer = await _context.CorrectAnswerMonthlies.AsNoTracking().Where(x => x.QuestionMonthlyId.Equals(question.Id)).ToListAsync();

            var optionMonthlyDto = _mapper.Map<List<OptionMonthlyDto>>(optionLession);

            var correctAnswerDto = _mapper.Map<List<CorrectAnswerMonthlyDto>>(correctAnswer);

            var questionDto = new QuestionMonthlyDto();

            questionDto.Id = question.Id;

            questionDto.QuizMonthlyId = question.QuizMonthlyId;

            questionDto.Name = question.Name;

            questionDto.Active = question.Active;

            questionDto.Point = question.Point;

            questionDto.Alias = question.Alias;

            questionDto.Code = question.Code;
            questionDto.QuizMonthlyDto = new QuizMonthlyDto()
            {
                ID = question.QuizMonthly.ID,
                RoleId = question.QuizMonthly.RoleId,
                Name = question.QuizMonthly.Name,
                Description = question.QuizMonthly.Description,
                Active = question.QuizMonthly.Active,
                DateCreated = question.QuizMonthly.DateCreated,
                TimeToDo = question.QuizMonthly.TimeToDo,
                ScorePass = question.QuizMonthly.ScorePass,
                Code = question.QuizMonthly.Code,
            };

            questionDto.CorrectAnswerMonthlyDtos = new List<CorrectAnswerMonthlyDto>(correctAnswerDto);

            questionDto.OptionMonthlyDtos = new List<OptionMonthlyDto>(optionMonthlyDto);
            if (accountName != null)
            {

                var answerQuestionMonthly = _context.AnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(id)).Where(x => x.AccountName == accountName).AsQueryable();

                if (answerQuestionMonthly == null)
                {
                    answerQuestionMonthly = _context.AnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(id)).AsNoTracking().AsQueryable();

                    var answerQuestionMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerQuestionMonthly);

                    if (answerQuestionMonthlyDto != null)
                    {

                        questionDto.AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerQuestionMonthlyDto);

                        return questionDto;

                    }
                }
                else
                {

                    var answerQuestionMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerQuestionMonthly);

                    if (answerQuestionMonthlyDto != null)
                    {
                        questionDto.AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerQuestionMonthlyDto);

                        return questionDto;

                    }
                }

            }
            else
            {
                var answerQuestionMonthly = _context.AnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(id)).AsNoTracking().AsQueryable();

                var answerQuestionMonthlyDto = _mapper.Map<List<AnswerMonthlyDto>>(answerQuestionMonthly);

                if (answerQuestionMonthlyDto != null)
                {

                    questionDto.AnswerMonthlyDtos = new List<AnswerMonthlyDto>(answerQuestionMonthlyDto);

                    return questionDto;

                }
            }

            return default;
        }

        public async Task<IEnumerable<QuestionMonthlyDto>> GetQuestionDtos()
        {
            var question = await _context.QuestionMonthlies.Include(x => x.QuizMonthly).AsNoTracking().ToListAsync();
            var questionDto = _mapper.Map<List<QuestionMonthlyDto>>(question);
            return questionDto;
        }

        public async Task InsertConcerningQuestionMonthlyDto(CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Alias = Utilities.SEOUrl(createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Name);
            var code = _configuration.GetValue<string>("Code:QuestionMonthly");

            var key = code + Utilities.GenerateStringDateTime();
            createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Code = key;
            createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.DateCreated = DateTime.Now;

            QuestionMonthly question = _mapper.Map<QuestionMonthly>(createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto);
            if (_context.QuestionMonthlies.Any(x => x.QuizMonthlyId.Equals(createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.QuizMonthlyId) && x.Name.Equals(createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Name)) == false)
            {
                _context.QuestionMonthlies.Add(question);
                await _context.SaveChangesAsync();
            }


            if (createAllConcerningQuestionMonthlyDto.OptionMonthlys != null)
            {

                for (int i = 0; i < createAllConcerningQuestionMonthlyDto.OptionMonthlys.Length; i++)
                {
                    for (int j = i; j < createAllConcerningQuestionMonthlyDto.CorrectAnswers.Length;)
                    {
                        createAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlys.Add(new OptionAndCorrectMonthly
                        {
                            OptionMonthlys = createAllConcerningQuestionMonthlyDto.OptionMonthlys[i],
                            CorrectAnswers = createAllConcerningQuestionMonthlyDto.CorrectAnswers[j],
                        });
                        break;
                    }
                }

                foreach (var item in createAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlys)
                {
                    createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto.Id = Guid.NewGuid();
                    createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto.QuestionMonthlyId = createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Id;
                    createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto.Name = item.OptionMonthlys;

                    OptionMonthly optionMonthlyDto = _mapper.Map<OptionMonthly>(createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto);

                    _context.OptionMonthlies.Add(optionMonthlyDto);

                    await _context.SaveChangesAsync();

                    if (item.CorrectAnswers == true)
                    {
                        createAllConcerningQuestionMonthlyDto.CreateCorrectAnswerMonthlyDto.Id = Guid.NewGuid();
                        createAllConcerningQuestionMonthlyDto.CreateCorrectAnswerMonthlyDto.OptionMonthlyId = createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto.Id;
                        createAllConcerningQuestionMonthlyDto.CreateCorrectAnswerMonthlyDto.QuestionMonthlyId = createAllConcerningQuestionMonthlyDto.CreateQuestionMonthlyDto.Id;
                        createAllConcerningQuestionMonthlyDto.CreateCorrectAnswerMonthlyDto.CorrectAnswer = createAllConcerningQuestionMonthlyDto.CreateOptionMonthlyDto.Name;

                        CorrectAnswerMonthly correctAnswerMonthly = _mapper.Map<CorrectAnswerMonthly>(createAllConcerningQuestionMonthlyDto.CreateCorrectAnswerMonthlyDto);

                        _context.CorrectAnswerMonthlies.Add(correctAnswerMonthly);

                        await _context.SaveChangesAsync();
                    }

                }
            }



            await transaction.CommitAsync();
        }

        public async Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            if (updateAllConcerningQuestionMonthlyDto.NewOptionMonthlys.Length > 0)
            {
                for (int i = 0; i < updateAllConcerningQuestionMonthlyDto.NewOptionMonthlys.Length; i++)
                {
                    for (int j = i; j < updateAllConcerningQuestionMonthlyDto.NewCorrectAnswers.Length;)
                    {
                        updateAllConcerningQuestionMonthlyDto.NewOptionAndCorrectMonthlyDtos.Add(new NewOptionAndCorrectMonthlyDto
                        {
                            NewOptionMonthlys = updateAllConcerningQuestionMonthlyDto.NewOptionMonthlys[i],
                            NewCorrectAnswers = updateAllConcerningQuestionMonthlyDto.NewCorrectAnswers[j],
                        });

                        break;
                    }
                }

                foreach (var item in updateAllConcerningQuestionMonthlyDto.NewOptionAndCorrectMonthlyDtos)
                {
                    CreateOptionMonthlyDto createOptionMonthlyDto = new();

                    var existOptionsDb = await _context.OptionMonthlies.Where(x => x.QuestionMonthlyId.Equals(id) && x.Name.Equals(item.NewOptionMonthlys) == true).ToListAsync();

                    if (existOptionsDb.Count == 0)
                    {
                        createOptionMonthlyDto.Id = Guid.NewGuid();

                        createOptionMonthlyDto.QuestionMonthlyId = id;

                        createOptionMonthlyDto.Name = item.NewOptionMonthlys;

                        OptionMonthly optionMonthlyDto = _mapper.Map<OptionMonthly>(createOptionMonthlyDto);

                        _context.OptionMonthlies.Add(optionMonthlyDto);

                        await _context.SaveChangesAsync();
                    }



                    if (item.NewCorrectAnswers == true)
                    {
                        var existCorrectsDb = await _context.CorrectAnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(id) && x.CorrectAnswer.Equals(item.NewCorrectAnswers) == true).ToListAsync();

                        if (existOptionsDb.Count == 0)
                        {
                            CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto = new();

                            createCorrectAnswerMonthlyDto.Id = Guid.NewGuid();

                            createCorrectAnswerMonthlyDto.OptionMonthlyId = createOptionMonthlyDto.Id;

                            createCorrectAnswerMonthlyDto.QuestionMonthlyId = id;

                            createCorrectAnswerMonthlyDto.CorrectAnswer = createOptionMonthlyDto.Name;

                            CorrectAnswerMonthly correctAnswerMonthly = _mapper.Map<CorrectAnswerMonthly>(createCorrectAnswerMonthlyDto);

                            _context.CorrectAnswerMonthlies.Add(correctAnswerMonthly);

                            await _context.SaveChangesAsync();
                        }


                    }

                }
                await transaction.CommitAsync();

            }
        }

        public async Task UpdateConcerningQuestionMonthlyDto(Guid id, UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            for (int i = 0; i < updateAllConcerningQuestionMonthlyDto.OptionMonthlyId.Length; i++)
            {
                for (int j = i; j < updateAllConcerningQuestionMonthlyDto.OptionMonthlys.Length;)
                {
                    updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.OptionMonthlyDtos.Add(new OptionMonthlyDto
                    {
                        Id = updateAllConcerningQuestionMonthlyDto.OptionMonthlyId[i],
                        QuestionMonthlyId = id,
                        Name = updateAllConcerningQuestionMonthlyDto.OptionMonthlys[j],

                    });
                    for (int k = i; k < updateAllConcerningQuestionMonthlyDto.CorrectAnswerId.Length;)
                    {
                        for (int l = k; j < updateAllConcerningQuestionMonthlyDto.CorrectAnswers.Length;)
                        {

                            updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.CorrectAnswerMonthlyDtos.Add(new CorrectAnswerMonthlyDto
                            {
                                Id = updateAllConcerningQuestionMonthlyDto.CorrectAnswerId[k],
                                QuestionMonthlyId = id,

                                OptionMonthlyId = updateAllConcerningQuestionMonthlyDto.OptionMonthlyId[i],

                                CorrectAnswer = updateAllConcerningQuestionMonthlyDto.OptionMonthlys[j],

                                Correct = updateAllConcerningQuestionMonthlyDto.CorrectAnswers[l],
                            });
                            break;
                        }
                        break;
                    }
                    break;
                }
            }
            updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.Alias = Utilities.SEOUrl(updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.Name);

            updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly.DateCreated = DateTime.Now;

            QuestionMonthly question = (_mapper.Map(updateAllConcerningQuestionMonthlyDto.UpdateQuestionMonthly, await _context.QuestionMonthlies.FirstOrDefaultAsync(x => x.Id.Equals(id))));

            _context.QuestionMonthlies.Update(question);

            await _context.SaveChangesAsync();

            var correctDb = _context.CorrectAnswerMonthlies.Where(x => x.QuestionMonthlyId.Equals(id)).AsNoTracking().AsQueryable();
            var optionDb = _context.OptionMonthlies.Where(x => x.QuestionMonthlyId.Equals(id)).AsNoTracking().AsQueryable();
            var correctTrue = updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.CorrectAnswerMonthlyDtos.Where(x => x.Correct == true).ToList();

            var correctFalse = updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.CorrectAnswerMonthlyDtos.Where(x => x.Correct == false).ToList();

            var correctAnswerMonthlyDto = _mapper.Map<List<CorrectAnswerMonthlyDto>>(correctDb);

            var optionMonthlyDto = _mapper.Map<List<OptionMonthlyDto>>(optionDb);

            var correctTrueDifference = correctTrue.Except(correctAnswerMonthlyDto).ToList();
            var correctFalseDifference = correctFalse.Except(correctAnswerMonthlyDto).ToList();

            var optionDifference = updateAllConcerningQuestionMonthlyDto.OptionAndCorrectMonthlyDto.OptionMonthlyDtos.Where(x => !optionMonthlyDto.Any(a => a.Name.Equals(x.Name) && a.Id.Equals(x.Id) == true)).ToList();


            var correctFalseExistDb = correctAnswerMonthlyDto.Where(x => correctFalse.Any(a => a.Id.Equals(x.Id))).ToList();

            if (correctFalseExistDb.Count > 0)
            {
                List<CorrectAnswerMonthly> correctAnswerMonthly = _mapper.Map<List<CorrectAnswerMonthly>>(correctFalseExistDb);

                _context.CorrectAnswerMonthlies.RemoveRange(correctAnswerMonthly);

                await _context.SaveChangesAsync();

            }

            if (optionDifference.Count > 0)
            {
                foreach (var item in optionDifference)
                {
                    if (optionDifference.Any(x => x.Id.Equals(item.Id)) == true)
                    {
                        OptionMonthly optionMonthly = _mapper.Map<OptionMonthly>(item);

                        _context.OptionMonthlies.Update(optionMonthly);

                        await _context.SaveChangesAsync();
                    }

                }
            }

            if (correctTrueDifference.Count > 0)
            {
                foreach (var correctAnswer in correctTrueDifference)
                {
                    if (correctAnswerMonthlyDto.Any(x => x.Id.Equals(correctAnswer.Id) && x.CorrectAnswer.Equals(correctAnswer.CorrectAnswer)) == false)
                    {

                        CorrectAnswerMonthly correctAnswerMonthly = _mapper.Map<CorrectAnswerMonthly>(correctAnswer);

                        correctAnswer.Id = Guid.NewGuid();

                        _context.CorrectAnswerMonthlies.Add(correctAnswerMonthly);

                        await _context.SaveChangesAsync();
                    }
                    else if (optionDifference.Count > 0)
                    {
                        foreach (var item in optionDifference)
                        {
                            if (correctAnswer.OptionMonthlyId.Equals(item.Id) == true)
                            {
                                correctAnswer.CorrectAnswer = item.Name;

                                CorrectAnswerMonthly correctAnswerMonthly = _mapper.Map<CorrectAnswerMonthly>(correctAnswer);

                                _context.CorrectAnswerMonthlies.Update(correctAnswerMonthly);

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
