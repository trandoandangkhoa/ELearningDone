using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IAnswerCourseService
    {
        Task<IEnumerable<AnswerCourseDto>> GetAnswerDtos();

        Task<List<AnswerCourseDto>> GetAnswerById(Guid id, string accountName);

        Task InsertAnswerDto(CreateAnswerCourseDto createAnswerCourseDto);

        Task DeleteAnswerDto(Guid id);

        Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerCourseDto updateAnswerCourseDto);

    }
    public class AnswerCourseService : IAnswerCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        public AnswerCourseService(WebLearningContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteAnswerDto(Guid id)
        {
            var answer = await _context.AnswerCourses.FindAsync(id);
            if (answer == null)
            {
                return;
            }
            _context.Remove(answer);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AnswerCourseDto>> GetAnswerById(Guid id, string accountName)
        {
            var answer = await _context.AnswerCourses.Include(x => x.QuestionFinal).AsNoTracking().Where(x => x.QuestionCourseId.Equals(id) && x.AccountName == accountName).ToListAsync();
            return _mapper.Map<List<AnswerCourseDto>>(answer);
        }

        public async Task<IEnumerable<AnswerCourseDto>> GetAnswerDtos()
        {
            var answer = await _context.AnswerCourses.Include(x => x.QuestionFinal).AsNoTracking().ToListAsync();
            var answerDto = _mapper.Map<List<AnswerCourseDto>>(answer);
            return answerDto;
        }

        public Task<PagedViewModel<AnswerCourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAnswerDto(CreateAnswerCourseDto createAnswerCourseDto)
        {
            createAnswerCourseDto.Id = Guid.NewGuid();

            if (createAnswerCourseDto.OwnAnswer == null)
            {
                createAnswerCourseDto.OwnAnswer = " ";
            }

            AnswerCourse answer = _mapper.Map<AnswerCourse>(createAnswerCourseDto);

            _context.Add(answer);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerCourseDto updateAnswerCourseDto)
        {
            AnswerCourse answer = (_mapper.Map(updateAnswerCourseDto, await _context.AnswerCourses.Include(x => x.QuestionFinal).FirstOrDefaultAsync(x => x.QuestionCourseId.Equals(id) && x.AccountName == accountName)));
            if (answer != null)
            {
                _context.AnswerCourses.Update(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
