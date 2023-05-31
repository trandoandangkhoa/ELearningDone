using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IOptionCourseService
    {
        Task<IEnumerable<OptionCourseDto>> GetOptionCourse();
        Task<OptionCourseDto> GetOptionCourseById(Guid Id);
        Task InsertOptionCourse(CreateOptionCourseDto createOptionCourseDto);
        Task DeleteOptionCourse(Guid id, Guid questionCourseId);
        Task UpdateOptionCourse(UpdateOptionCourseDto updateOptionCourseDto, Guid Id);
        Task<PagedViewModel<OptionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class OptionCourseService : IOptionCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public OptionCourseService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteOptionCourse(Guid id, Guid questionCourseId)
        {
            var transaction = _context.Database.BeginTransaction();

            var optionCourse = await _context.OptionCourses.FirstOrDefaultAsync(x => x.QuestionFinalId.Equals(questionCourseId) && x.Id.Equals(id));

            _context.OptionCourses.Remove(optionCourse);

            await _context.SaveChangesAsync();

            var correctCourse = await _context.CorrectAnswerCourses.FirstOrDefaultAsync(x => x.QuestionCourseId.Equals(questionCourseId) && x.OptionCourseId.Equals(optionCourse.Id));

            if (correctCourse != null)
            {
                _context.CorrectAnswerCourses.Remove(correctCourse);

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task<PagedViewModel<OptionCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = 10f;
            var pageCount = Math.Ceiling(_context.OptionCourses.Count() / (double)pageResult);
            var query = _context.OptionCourses.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new OptionCourseDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        QuestionFinalId = x.QuestionFinalId,
                                    })
                                    .ToListAsync();
            var OptionCourseResponse = new PagedViewModel<OptionCourseDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return OptionCourseResponse;
        }

        public async Task<IEnumerable<OptionCourseDto>> GetOptionCourse()
        {
            var optionCourse = await _context.OptionCourses.OrderByDescending(x => x.Name).AsNoTracking().ToListAsync();
            var optionCourseDto = _mapper.Map<List<OptionCourseDto>>(optionCourse);
            return optionCourseDto;
        }

        public async Task<OptionCourseDto> GetOptionCourseById(Guid Id)
        {
            var optionCourse = await _context.OptionCourses.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<OptionCourseDto>(optionCourse);
        }

        public async Task InsertOptionCourse(CreateOptionCourseDto createOptionCourseDto)
        {
            createOptionCourseDto.Id = Guid.NewGuid();

            OptionCourse optionCourse = _mapper.Map<OptionCourse>(createOptionCourseDto);


            _context.Add(optionCourse);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOptionCourse(UpdateOptionCourseDto updateOptionCourseDto, Guid Id)
        {
            var item = await _context.OptionCourses.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.OptionCourses.Update(_mapper.Map(updateOptionCourseDto, item));

                await _context.SaveChangesAsync();
            }
        }


    }
}
