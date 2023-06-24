using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ICorrectAnswerCourseService
    {
        Task<IEnumerable<CorrectAnswerCourseDto>> GetcorrectAnswer();
        Task<CorrectAnswerCourseDto> GetcorrectAnswerById(Guid Id);
        Task InsertcorrectAnswer(CreateCorrectAnswerCourseDto createCorrectAnswerCourseDto);
        Task DeletecorrectAnswer(Guid Id);
        Task UpdatecorrectAnswer(UpdateCorrectAnswerCourseDto updateCorrectAnswerCourseDto, Guid Id);
        Task<PagedViewModel<CorrectAnswerCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class CorrectAnswerCourseService : ICorrectAnswerCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IOptionCourseService _optionCourseService;
        public CorrectAnswerCourseService(WebLearningContext context, IMapper mapper, IOptionCourseService optionCourseService)
        {
            _context = context;
            _mapper = mapper;
            _optionCourseService = optionCourseService;
        }
        public async Task DeletecorrectAnswer(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerCourses.FindAsync(Id);
            _context.CorrectAnswerCourses.Remove(correctAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedViewModel<CorrectAnswerCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = 10f;
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(pageResult);
            }
            else
            {
                pageResult = getListPagingRequest.PageSize;
            }

            var pageCount = Math.Ceiling(_context.CorrectAnswerCourses.Count() / (double)pageResult);
            var query = _context.CorrectAnswerCourses.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CorrectAnswer.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new CorrectAnswerCourseDto()
                                    {
                                        Id = x.Id,
                                        QuestionCourseId = x.QuestionCourseId,
                                        CorrectAnswer = x.CorrectAnswer,
                                    })
                                    .ToListAsync();
            var correctAnswerResponse = new PagedViewModel<CorrectAnswerCourseDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return correctAnswerResponse;
        }

        public async Task<IEnumerable<CorrectAnswerCourseDto>> GetcorrectAnswer()
        {
            var correctAnswer = await _context.CorrectAnswerCourses.OrderByDescending(x => x.CorrectAnswer).AsNoTracking().ToListAsync();
            var correctAnswerDto = _mapper.Map<List<CorrectAnswerCourseDto>>(correctAnswer);
            return correctAnswerDto;
        }

        public async Task<CorrectAnswerCourseDto> GetcorrectAnswerById(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerCourses.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<CorrectAnswerCourseDto>(correctAnswer);
        }

        public async Task InsertcorrectAnswer(CreateCorrectAnswerCourseDto createCorrectAnswerCourseDto)
        {
            createCorrectAnswerCourseDto.Id = Guid.NewGuid();

            var optionCourseName = await _optionCourseService.GetOptionCourseById(createCorrectAnswerCourseDto.OptionCourseId);

            createCorrectAnswerCourseDto.CorrectAnswer = optionCourseName.Name;

            CorrectAnswerCourse correctAnswer = _mapper.Map<CorrectAnswerCourse>(createCorrectAnswerCourseDto);

            _context.Add(correctAnswer);

            await _context.SaveChangesAsync();
        }

        public async Task UpdatecorrectAnswer(UpdateCorrectAnswerCourseDto updateCorrectAnswerCourseDto, Guid Id)
        {
            var item = await _context.CorrectAnswerCourses.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.CorrectAnswerCourses.Update(_mapper.Map(updateCorrectAnswerCourseDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
