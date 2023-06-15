using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IQuestionCourseService
    {
        Task<IEnumerable<QuestionCourseDto>> GetQuestionDtos();

        Task<QuestionCourseDto> GetQuestionById(Guid id, string accountName);

        Task UpdateConcerningQuestionCourseDto(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto);

        Task InsertConcerningQuestionCourseDto(CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto);

        Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto);

        Task DeleteQuestionDto(Guid id);

        Task<QuestionCourseDto> GetNameQuestion(string name);
        Task<QuestionCourseDto> CheckExist(Guid id, string name);

        Task<PagedViewModel<QuestionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<IEnumerable<QuestionCourseDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest);
        Task<QuestionCourseDto> GetCode(string code);
    }
    public class QuestionCourseService : IQuestionCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;


        public QuestionCourseService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task<QuestionCourseDto> CheckExist(Guid id, string name)
        {
            var account = await _context.QuestionFinals.FirstOrDefaultAsync(x => x.QuizCourseId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<QuestionCourseDto>(account);
        }

        public async Task DeleteQuestionDto(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var question = await _context.QuestionFinals.FindAsync(id);
            _context.QuestionFinals.Remove(question);

            await _context.SaveChangesAsync();


            //Update Submit false in userTest
            var historySubmit = _context.HistorySubmitFinals.Where(x => x.QuizCourseId.Equals(question.QuizCourseId));

            _context.HistorySubmitFinals.RemoveRange(historySubmit);
            await _context.SaveChangesAsync();



            var reportScore = _context.ReportUserScoreFinals.Where(x => x.QuizCourseId.Equals(question.QuizCourseId));
            _context.ReportUserScoreFinals.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<QuestionCourseDto> GetCode(string code)
        {
            var account = await _context.QuestionFinals.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<QuestionCourseDto>(account);
        }

        public async Task<QuestionCourseDto> GetNameQuestion(string name)
        {
            var account = await _context.QuestionFinals.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return _mapper.Map<QuestionCourseDto>(account);
        }

        public async Task<PagedViewModel<QuestionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:QuestionCourse"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.QuestionFinals.Count() / (double)pageResult);
            var query = _context.QuestionFinals.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.QuizCourse.Name.Contains(getListPagingRequest.Keyword) || x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new QuestionCourseDto()
                                    {
                                        Id = x.Id,
                                        QuizCourseId = x.QuizCourseId,
                                        Name = x.Name,
                                        Active = x.Active,
                                        Point = x.Point,
                                        Alias = x.Alias,
                                        Code = x.Code,
                                        QuizCourseDto = new QuizCourseDto()
                                        {
                                            ID = x.QuizCourse.ID,
                                            CourseId = x.QuizCourse.Course.Id,
                                            Name = x.QuizCourse.Name,
                                            Description = x.QuizCourse.Description,
                                            Active = x.QuizCourse.Active,
                                            DateCreated = x.QuizCourse.DateCreated,
                                            TimeToDo = x.QuizCourse.TimeToDo,
                                            ScorePass = x.QuizCourse.ScorePass,
                                            Code = x.QuizCourse.Code,
                                        },

                                    })
                                    .OrderByDescending(x => x.Code).ToListAsync();
            var questionCourseResponse = new PagedViewModel<QuestionCourseDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return questionCourseResponse;
        }

        public Task<IEnumerable<QuestionCourseDto>> GetQuestionByCategory(GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionCourseDto> GetQuestionById(Guid id, string accountName)
        {
            var question = await _context.QuestionFinals.Include(x => x.Options).Include(x => x.QuizCourse).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            var optionCourse = _context.OptionCourses.AsNoTracking().Where(x => x.QuestionFinalId.Equals(question.Id)).AsQueryable();

            var correctAnswer = _context.CorrectAnswerCourses.AsNoTracking().Where(x => x.QuestionCourseId.Equals(question.Id)).AsQueryable();

            var optionCourseDto = _mapper.Map<List<OptionCourseDto>>(optionCourse);


            var correctAnswerDto = _mapper.Map<List<CorrectAnswerCourseDto>>(correctAnswer);
            var questionDto = new QuestionCourseDto();

            questionDto.Id = question.Id;

            questionDto.QuizCourseId = question.QuizCourseId;

            questionDto.Name = question.Name;

            questionDto.Active = question.Active;

            questionDto.Point = question.Point;

            questionDto.Alias = question.Alias;

            questionDto.Code = question.Code;

            questionDto.QuizCourseDto = new QuizCourseDto()
            {
                ID = question.QuizCourse.ID,
                CourseId = question.QuizCourse.CourseId,
                Name = question.QuizCourse.Name,
                Description = question.QuizCourse.Description,
                Active = question.QuizCourse.Active,
                DateCreated = question.QuizCourse.DateCreated,
                TimeToDo = question.QuizCourse.TimeToDo,
                ScorePass = question.QuizCourse.ScorePass,
                Code = question.QuizCourse.Code,
            };
            questionDto.CorrectAnswerCourseDtos = new List<CorrectAnswerCourseDto>(correctAnswerDto);

            questionDto.OptionCourseDtos = new List<OptionCourseDto>(optionCourseDto);

            if (accountName != null)
            {

                var answerQuestionCourse = _context.AnswerCourses.Where(x => x.QuestionCourseId.Equals(id)).Where(x => x.AccountName == accountName).AsQueryable();

                if (answerQuestionCourse == null)
                {
                    answerQuestionCourse = _context.AnswerCourses.Where(x => x.QuestionCourseId.Equals(id)).AsQueryable();

                    var answerQuestionCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerQuestionCourse);

                    if (answerQuestionCourseDto != null)
                    {

                        questionDto.AnswerCourseDtos = new List<AnswerCourseDto>(answerQuestionCourseDto);

                        return questionDto;

                    }
                }
                else
                {

                    var answerQuestionCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerQuestionCourse);

                    if (answerQuestionCourseDto != null)
                    {
                        questionDto.AnswerCourseDtos = new List<AnswerCourseDto>(answerQuestionCourseDto);

                        return questionDto;

                    }
                }

            }
            else
            {
                var answerQuestionCourse = _context.AnswerCourses.Where(x => x.QuestionCourseId.Equals(id)).AsNoTracking().AsQueryable();

                var answerQuestionCourseDto = _mapper.Map<List<AnswerCourseDto>>(answerQuestionCourse);

                if (answerQuestionCourseDto != null)
                {

                    questionDto.AnswerCourseDtos = new List<AnswerCourseDto>(answerQuestionCourseDto);

                    return questionDto;

                }
            }

            return default;
        }

        public async Task<IEnumerable<QuestionCourseDto>> GetQuestionDtos()
        {
            var question = await _context.QuestionFinals.Include(x => x.QuizCourse).AsNoTracking().ToListAsync();
            var questionDto = _mapper.Map<List<QuestionCourseDto>>(question);
            return questionDto;
        }

        public async Task InsertConcerningQuestionCourseDto(CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Alias = Utilities.SEOUrl(createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Name);
            var code = _configuration.GetValue<string>("Code:QuestionCourse");
            var key = code + Utilities.GenerateStringDateTime();
            createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Code = key;
            createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.DateCreated = DateTime.Now;
            QuestionFinal question = _mapper.Map<QuestionFinal>(createAllConcerningQuestionCourseDto.CreateQuestionCourseDto);
            if (_context.QuestionFinals.Any(x => x.QuizCourseId.Equals(createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.QuizCourseId) && x.Name.Equals(createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Name)) == false)
            {
                _context.QuestionFinals.Add(question);
                await _context.SaveChangesAsync();
            }
            if (createAllConcerningQuestionCourseDto.OptionCourses != null)
            {
                for (int i = 0; i < createAllConcerningQuestionCourseDto.OptionCourses.Length; i++)
                {
                    for (int j = i; j < createAllConcerningQuestionCourseDto.CorrectAnswers.Length;)
                    {
                        createAllConcerningQuestionCourseDto.OptionAndCorrectCourses.Add(new OptionAndCorrectCourse
                        {
                            OptionCourses = createAllConcerningQuestionCourseDto.OptionCourses[i],
                            CorrectAnswers = createAllConcerningQuestionCourseDto.CorrectAnswers[j],
                        });
                        break;
                    }
                }

                foreach (var item in createAllConcerningQuestionCourseDto.OptionAndCorrectCourses)
                {
                    createAllConcerningQuestionCourseDto.CreateOptionCourseDto.Id = Guid.NewGuid();
                    createAllConcerningQuestionCourseDto.CreateOptionCourseDto.QuestionFinalId = createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Id;
                    createAllConcerningQuestionCourseDto.CreateOptionCourseDto.Name = item.OptionCourses;

                    OptionCourse optionCourseDto = _mapper.Map<OptionCourse>(createAllConcerningQuestionCourseDto.CreateOptionCourseDto);

                    _context.OptionCourses.Add(optionCourseDto);

                    await _context.SaveChangesAsync();

                    if (item.CorrectAnswers == true)
                    {
                        createAllConcerningQuestionCourseDto.CreateCorrectAnswerCourseDto.Id = Guid.NewGuid();
                        createAllConcerningQuestionCourseDto.CreateCorrectAnswerCourseDto.OptionCourseId = createAllConcerningQuestionCourseDto.CreateOptionCourseDto.Id;
                        createAllConcerningQuestionCourseDto.CreateCorrectAnswerCourseDto.QuestionCourseId = createAllConcerningQuestionCourseDto.CreateQuestionCourseDto.Id;
                        createAllConcerningQuestionCourseDto.CreateCorrectAnswerCourseDto.CorrectAnswer = createAllConcerningQuestionCourseDto.CreateOptionCourseDto.Name;
                        CorrectAnswerCourse correctAnswerCourse = _mapper.Map<CorrectAnswerCourse>(createAllConcerningQuestionCourseDto.CreateCorrectAnswerCourseDto);

                        _context.CorrectAnswerCourses.Add(correctAnswerCourse);

                        await _context.SaveChangesAsync();
                    }

                }
            }






            await transaction.CommitAsync();
        }

        public async Task NewOptionAndCorrectAnswerInUpdate(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            if (updateAllConcerningQuestionCourseDto.NewOptionCourses.Length > 0)
            {
                for (int i = 0; i < updateAllConcerningQuestionCourseDto.NewOptionCourses.Length; i++)
                {
                    for (int j = i; j < updateAllConcerningQuestionCourseDto.NewCorrectAnswers.Length;)
                    {
                        updateAllConcerningQuestionCourseDto.NewOptionAndCorrectCourseDtos.Add(new NewOptionAndCorrectCourseDto
                        {
                            NewOptionCourses = updateAllConcerningQuestionCourseDto.NewOptionCourses[i],
                            NewCorrectAnswers = updateAllConcerningQuestionCourseDto.NewCorrectAnswers[j],
                        });

                        break;
                    }
                }

                foreach (var item in updateAllConcerningQuestionCourseDto.NewOptionAndCorrectCourseDtos)
                {
                    CreateOptionCourseDto createOptionCourseDto = new();

                    var existOptionsDb = await _context.OptionCourses.Where(x => x.QuestionFinalId.Equals(id) && x.Name.Equals(item.NewOptionCourses) == true).ToListAsync();

                    if (existOptionsDb.Count == 0)
                    {
                        createOptionCourseDto.Id = Guid.NewGuid();


                        createOptionCourseDto.QuestionFinalId = id;

                        createOptionCourseDto.Name = item.NewOptionCourses;

                        OptionCourse optionCourseDto = _mapper.Map<OptionCourse>(createOptionCourseDto);

                        _context.OptionCourses.Add(optionCourseDto);

                        await _context.SaveChangesAsync();
                    }



                    if (item.NewCorrectAnswers == true)
                    {
                        var existCorrectsDb = await _context.CorrectAnswerCourses.Where(x => x.QuestionCourseId.Equals(id) && x.CorrectAnswer.Equals(item.NewCorrectAnswers) == true).ToListAsync();

                        if (existOptionsDb.Count == 0)
                        {
                            CreateCorrectAnswerCourseDto createCorrectAnswerCourseDto = new();

                            createCorrectAnswerCourseDto.Id = Guid.NewGuid();

                            createCorrectAnswerCourseDto.OptionCourseId = createOptionCourseDto.Id;

                            createCorrectAnswerCourseDto.QuestionCourseId = id;

                            createCorrectAnswerCourseDto.CorrectAnswer = createOptionCourseDto.Name;

                            CorrectAnswerCourse correctAnswerCourse = _mapper.Map<CorrectAnswerCourse>(createCorrectAnswerCourseDto);

                            _context.CorrectAnswerCourses.Add(correctAnswerCourse);

                            await _context.SaveChangesAsync();
                        }


                    }

                }
                await transaction.CommitAsync();

            }
        }

        public async Task UpdateConcerningQuestionCourseDto(Guid id, UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            for (int i = 0; i < updateAllConcerningQuestionCourseDto.OptionCourseId.Length; i++)
            {
                for (int j = i; j < updateAllConcerningQuestionCourseDto.OptionCourses.Length;)
                {
                    updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.OptionCourseDtos.Add(new OptionCourseDto
                    {
                        Id = updateAllConcerningQuestionCourseDto.OptionCourseId[i],
                        QuestionFinalId = id,
                        Name = updateAllConcerningQuestionCourseDto.OptionCourses[j],

                    });
                    for (int k = i; k < updateAllConcerningQuestionCourseDto.CorrectAnswerId.Length;)
                    {
                        for (int l = k; j < updateAllConcerningQuestionCourseDto.CorrectAnswers.Length;)
                        {

                            updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.CorrectAnswerCourseDtos.Add(new CorrectAnswerCourseDto
                            {
                                Id = updateAllConcerningQuestionCourseDto.CorrectAnswerId[k],
                                QuestionCourseId = id,

                                OptionCourseId = updateAllConcerningQuestionCourseDto.OptionCourseId[i],

                                CorrectAnswer = updateAllConcerningQuestionCourseDto.OptionCourses[j],

                                Correct = updateAllConcerningQuestionCourseDto.CorrectAnswers[l],
                            });
                            break;
                        }
                        break;
                    }
                    break;
                }
            }
            updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.Alias = Utilities.SEOUrl(updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.Name);
            updateAllConcerningQuestionCourseDto.UpdateQuestionCourse.DateCreated = DateTime.Now;
            QuestionFinal question = _mapper.Map(updateAllConcerningQuestionCourseDto.UpdateQuestionCourse, await _context.QuestionFinals.FirstOrDefaultAsync(x => x.Id.Equals(id)));

            _context.QuestionFinals.Update(question);

            await _context.SaveChangesAsync();

            var correctDb = _context.CorrectAnswerCourses.Where(x => x.QuestionCourseId.Equals(id)).AsNoTracking().AsQueryable();

            var optionDb = _context.OptionCourses.Where(x => x.QuestionFinalId.Equals(id)).AsNoTracking().AsQueryable();

            var correctTrue = updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.CorrectAnswerCourseDtos.Where(x => x.Correct == true).ToList();

            var correctFalse = updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.CorrectAnswerCourseDtos.Where(x => x.Correct == false).ToList();

            var correctAnswerCourseDto = _mapper.Map<List<CorrectAnswerCourseDto>>(correctDb);

            var optionCourseDto = _mapper.Map<List<OptionCourseDto>>(optionDb);

            var correctTrueDifference = correctTrue.Except(correctAnswerCourseDto).ToList();

            var correctFalseDifference = correctFalse.Except(correctAnswerCourseDto).ToList();

            var optionDifference = updateAllConcerningQuestionCourseDto.OptionAndCorrectCourseDto.OptionCourseDtos.Where(x => !optionCourseDto.Any(a => a.Name.Equals(x.Name) && a.Id.Equals(x.Id) == true)).ToList();


            var correctFalseExistDb = correctAnswerCourseDto.Where(x => correctFalse.Any(a => a.Id.Equals(x.Id))).ToList();

            if (correctFalseExistDb.Count > 0)
            {
                List<CorrectAnswerCourse> correctAnswerCourse = _mapper.Map<List<CorrectAnswerCourse>>(correctFalseExistDb);

                _context.CorrectAnswerCourses.RemoveRange(correctAnswerCourse);

                await _context.SaveChangesAsync();

            }

            if (optionDifference.Count > 0)
            {
                foreach (var item in optionDifference)
                {
                    if (optionDifference.Any(x => x.Id.Equals(item.Id)) == true)
                    {
                        OptionCourse optionCourse = _mapper.Map<OptionCourse>(item);

                        _context.OptionCourses.Update(optionCourse);

                        await _context.SaveChangesAsync();
                    }

                }
            }

            if (correctTrueDifference.Count > 0)
            {
                foreach (var correctAnswer in correctTrueDifference)
                {
                    if (correctAnswerCourseDto.Any(x => x.Id.Equals(correctAnswer.Id) && x.CorrectAnswer.Equals(correctAnswer.CorrectAnswer)) == false)
                    {

                        CorrectAnswerCourse correctAnswerCourse = _mapper.Map<CorrectAnswerCourse>(correctAnswer);

                        correctAnswer.Id = Guid.NewGuid();

                        _context.CorrectAnswerCourses.Add(correctAnswerCourse);

                        await _context.SaveChangesAsync();
                    }
                    else if (optionDifference.Count > 0)
                    {
                        foreach (var item in optionDifference)
                        {
                            if (correctAnswer.OptionCourseId.Equals(item.Id) == true)
                            {
                                correctAnswer.CorrectAnswer = item.Name;

                                CorrectAnswerCourse correctAnswerCourse = _mapper.Map<CorrectAnswerCourse>(correctAnswer);

                                _context.CorrectAnswerCourses.Update(correctAnswerCourse);

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
