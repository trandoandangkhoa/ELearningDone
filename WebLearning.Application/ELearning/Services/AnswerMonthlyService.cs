using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IAnswerMonthlyService
    {
        Task<IEnumerable<AnswerMonthlyDto>> GetAnswerDtos();

        Task<List<AnswerMonthlyDto>> GetAnswerById(Guid id, string accountName);

        Task InsertAnswerDto(CreateAnswerMonthlyDto createAnswerMonthlyDto);

        Task DeleteAnswerDto(Guid id);

        Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerMonthlyDto updateAnswerMonthlyDto);

    }
    public class AnswerMonthlyService : IAnswerMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        public AnswerMonthlyService(WebLearningContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteAnswerDto(Guid id)
        {
            var answer = await _context.AnswerMonthlies.FindAsync(id);
            if (answer == null)
            {
                return;
            }
            _context.Remove(answer);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AnswerMonthlyDto>> GetAnswerById(Guid id, string accountName)
        {
            var answer = await _context.AnswerMonthlies.Include(x => x.QuestionMonthly).AsNoTracking().Where(x => x.QuestionMonthlyId.Equals(id) && x.AccountName == accountName).ToListAsync();
            return _mapper.Map<List<AnswerMonthlyDto>>(answer);
        }

        public async Task<IEnumerable<AnswerMonthlyDto>> GetAnswerDtos()
        {
            var answer = await _context.AnswerMonthlies.Include(x => x.QuestionMonthly).AsNoTracking().ToListAsync();

            var answerDto = _mapper.Map<List<AnswerMonthlyDto>>(answer);
            return answerDto;
        }


        public async Task InsertAnswerDto(CreateAnswerMonthlyDto createAnswerMonthlyDto)
        {
            createAnswerMonthlyDto.Id = Guid.NewGuid();

            if (createAnswerMonthlyDto.OwnAnswer == null)
            {
                createAnswerMonthlyDto.OwnAnswer = " ";
            }

            AnswerMonthly answer = _mapper.Map<AnswerMonthly>(createAnswerMonthlyDto);

            _context.Add(answer);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerDto(Guid id, string accountName, UpdateAnswerMonthlyDto updateAnswerMonthlyDto)
        {
            AnswerMonthly answer = _mapper.Map(updateAnswerMonthlyDto, await _context.AnswerMonthlies.Include(x => x.QuestionMonthly).FirstOrDefaultAsync(x => x.QuestionMonthlyId.Equals(id) && x.AccountName == accountName));
            if (answer != null)
            {
                _context.AnswerMonthlies.Update(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
