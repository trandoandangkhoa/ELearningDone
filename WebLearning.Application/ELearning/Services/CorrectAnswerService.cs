using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ICorrectAnswerService
    {
        Task<IEnumerable<CorrectAnswerLessionDto>> GetcorrectAnswer();
        Task<CorrectAnswerLessionDto> GetcorrectAnswerById(Guid Id);
        Task InsertcorrectAnswer(CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto);
        Task DeletecorrectAnswer(Guid Id);
        Task UpdatecorrectAnswer(UpdateCorrectAnswerLessionDto updateCorrectAnswerLessionDto, Guid Id);
        Task<PagedViewModel<CorrectAnswerLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class CorrectAnswerService : ICorrectAnswerService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IOptionLessionService _optionLessionService;
        public CorrectAnswerService(WebLearningContext context, IMapper mapper, IOptionLessionService optionLessionService)
        {
            _context = context;
            _mapper = mapper;
            _optionLessionService = optionLessionService;
        }
        public async Task DeletecorrectAnswer(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerLessions.FindAsync(Id);
            _context.CorrectAnswerLessions.Remove(correctAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedViewModel<CorrectAnswerLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
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
            
            var pageCount = Math.Ceiling(_context.CorrectAnswerLessions.Count() / (double)pageResult);
            var query = _context.CorrectAnswerLessions.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CorrectAnswer.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new CorrectAnswerLessionDto()
                                    {
                                        Id = x.Id,
                                        QuestionLessionId = x.QuestionLessionId,
                                        CorrectAnswer = x.CorrectAnswer,
                                    })
                                    .ToListAsync();
            var correctAnswerResponse = new PagedViewModel<CorrectAnswerLessionDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return correctAnswerResponse;
        }

        public async Task<IEnumerable<CorrectAnswerLessionDto>> GetcorrectAnswer()
        {
            var correctAnswer = await _context.CorrectAnswerLessions.OrderByDescending(x => x.CorrectAnswer).AsNoTracking().ToListAsync();
            var correctAnswerDto = _mapper.Map<List<CorrectAnswerLessionDto>>(correctAnswer);
            return correctAnswerDto;
        }

        public async Task<CorrectAnswerLessionDto> GetcorrectAnswerById(Guid Id)
        {
            var correctAnswer = await _context.CorrectAnswerLessions.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<CorrectAnswerLessionDto>(correctAnswer);
        }

        public async Task InsertcorrectAnswer(CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto)
        {
            createCorrectAnswerLessionDto.Id = Guid.NewGuid();

            var optionLessionName = await _optionLessionService.GetOptionLessionById(createCorrectAnswerLessionDto.OptionLessionId);

            createCorrectAnswerLessionDto.CorrectAnswer = optionLessionName.Name;

            CorrectAnswerLession correctAnswer = _mapper.Map<CorrectAnswerLession>(createCorrectAnswerLessionDto);

            _context.Add(correctAnswer);

            await _context.SaveChangesAsync();
        }

        public async Task UpdatecorrectAnswer(UpdateCorrectAnswerLessionDto updateCorrectAnswerLessionDto, Guid Id)
        {
            var item = await _context.CorrectAnswerLessions.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.CorrectAnswerLessions.Update(_mapper.Map(updateCorrectAnswerLessionDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
