using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ICorrectAnswerMonthlyService
    {
        Task<IEnumerable<CorrectAnswerMonthlyDto>> GetcorrectAnswer();
        Task<CorrectAnswerMonthlyDto> GetcorrectAnswerById(Guid Id);
        Task InsertcorrectAnswer(CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto);
        Task DeletecorrectAnswer(Guid Id);
        Task UpdatecorrectAnswer(UpdateCorrectAnswerMonthlyDto updateCorrectAnswerMonthlyDto, Guid Id);
        Task<PagedViewModel<CorrectAnswerMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class CorrectAnswerMonthlyService : ICorrectAnswerMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IOptionMonthlyService _optionMonthlyService;
        public CorrectAnswerMonthlyService(WebLearningContext context, IMapper mapper, IOptionMonthlyService optionMonthlyService)
        {
            _context = context;
            _mapper = mapper;
            _optionMonthlyService = optionMonthlyService;
        }
        public async Task DeletecorrectAnswer(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerMonthlies.FindAsync(Id);
            _context.CorrectAnswerMonthlies.Remove(correctAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedViewModel<CorrectAnswerMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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

            var pageCount = Math.Ceiling(_context.CorrectAnswerMonthlies.Count() / (double)pageResult);
            var query = _context.CorrectAnswerMonthlies.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CorrectAnswer.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new CorrectAnswerMonthlyDto()
                                    {
                                        Id = x.Id,
                                        QuestionMonthlyId = x.QuestionMonthlyId,
                                        CorrectAnswer = x.CorrectAnswer,
                                    })
                                    .ToListAsync();
            var correctAnswerResponse = new PagedViewModel<CorrectAnswerMonthlyDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return correctAnswerResponse;
        }

        public async Task<IEnumerable<CorrectAnswerMonthlyDto>> GetcorrectAnswer()
        {
            var correctAnswer = await _context.CorrectAnswerMonthlies.OrderByDescending(x => x.CorrectAnswer).AsNoTracking().ToListAsync();
            var correctAnswerDto = _mapper.Map<List<CorrectAnswerMonthlyDto>>(correctAnswer);
            return correctAnswerDto;
        }

        public async Task<CorrectAnswerMonthlyDto> GetcorrectAnswerById(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerMonthlies.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<CorrectAnswerMonthlyDto>(correctAnswer);
        }

        public async Task InsertcorrectAnswer(CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto)
        {
            createCorrectAnswerMonthlyDto.Id = Guid.NewGuid();

            var optionMonthlyName = await _optionMonthlyService.GetOptionMonthlyById(createCorrectAnswerMonthlyDto.OptionMonthlyId);

            createCorrectAnswerMonthlyDto.CorrectAnswer = optionMonthlyName.Name;

            CorrectAnswerMonthly correctAnswer = _mapper.Map<CorrectAnswerMonthly>(createCorrectAnswerMonthlyDto);

            _context.Add(correctAnswer);

            await _context.SaveChangesAsync();
        }

        public async Task UpdatecorrectAnswer(UpdateCorrectAnswerMonthlyDto updateCorrectAnswerMonthlyDto, Guid Id)
        {
            var item = await _context.CorrectAnswerMonthlies.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.CorrectAnswerMonthlies.Update(_mapper.Map(updateCorrectAnswerMonthlyDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
