using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IAnswerLessionService
    {
        Task<IEnumerable<AnswerLessionDto>> GetAnswerDtos();

        Task<List<AnswerLessionDto>> GetAnswerById(Guid id, string accountName);

        Task InsertAnswerDto(CreateAnswerLessionDto createAnswerLessionDto);

        Task DeleteAnswerDto(Guid id);

        Task ResetAllAnswer(Guid quizLessionId, string accountName);

        Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerLessionDto updateAnswerLessionDto);

    }
    public class AnswerLessionService : IAnswerLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        public AnswerLessionService(WebLearningContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteAnswerDto(Guid id)
        {
            var answer = await _context.AnswerLessions.FindAsync(id);
            if (answer == null)
            {
                return;
            }
            _context.Remove(answer);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AnswerLessionDto>> GetAnswerById(Guid id, string accountName)
        {
            var answer = await _context.AnswerLessions.Include(x => x.Question).AsNoTracking().Where(x => x.QuestionLessionId.Equals(id) && x.AccountName == accountName).ToListAsync();

            var answerDto = _mapper.Map<List<AnswerLessionDto>>(answer);

            return answerDto;
        }

        public async Task<IEnumerable<AnswerLessionDto>> GetAnswerDtos()
        {
            var answer = await _context.AnswerLessions.Include(x => x.Question).AsNoTracking().ToListAsync();

            if (answer != null)
            {
                var answerDto = _mapper.Map<List<AnswerLessionDto>>(answer);
                return answerDto;
            }
            return default;
        }

        public async Task InsertAnswerDto(CreateAnswerLessionDto createAnswerLessionDto)
        {
            createAnswerLessionDto.Id = Guid.NewGuid();

            if (createAnswerLessionDto != null)
            {
                if (createAnswerLessionDto.OwnAnswer != null)
                {
                    AnswerLession answer = _mapper.Map<AnswerLession>(createAnswerLessionDto);

                    _context.Add(answer);

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ResetAllAnswer(Guid quizLessionId, string accountName)
        {
            var answer = _context.AnswerLessions.Where(x => x.QuizLessionId.Equals(quizLessionId) && x.AccountName.Equals(accountName));

            if (answer != null)
            {
                _context.AnswerLessions.RemoveRange(answer);

                await _context.SaveChangesAsync();
            }

        }


        public async Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerLessionDto updateAnswerLessionDto)
        {
            AnswerLession answer = _mapper.Map(updateAnswerLessionDto, await _context.AnswerLessions.Include(x => x.Question).FirstOrDefaultAsync(x => x.QuestionLessionId.Equals(id) && x.AccountName == accountName));
            if (answer != null)
            {
                _context.AnswerLessions.Update(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
