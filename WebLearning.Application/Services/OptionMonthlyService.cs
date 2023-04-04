using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IOptionMonthlyService
    {
        Task<IEnumerable<OptionMonthlyDto>> GetOptionMonthly();
        Task<OptionMonthlyDto> GetOptionMonthlyById(Guid Id);
        Task InsertOptionMonthly(CreateOptionMonthlyDto createOptionMonthlyDto);
        Task DeleteOptionMonthly(Guid id, Guid questionMonthlyId);
        Task UpdateOptionMonthly(UpdateOptionMonthlyDto updateOptionMonthlyDto, Guid Id);
        Task<PagedViewModel<OptionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class OptionMonthlyService : IOptionMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public OptionMonthlyService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteOptionMonthly(Guid id, Guid questionMonthlyId)
        {
            var transaction = _context.Database.BeginTransaction();

            var optionMonthly = await _context.OptionMonthlies.FirstOrDefaultAsync(x => x.QuestionMonthlyId.Equals(questionMonthlyId) && x.Id.Equals(id));

            _context.OptionMonthlies.Remove(optionMonthly);

            await _context.SaveChangesAsync();

            var correctMonthly = await _context.CorrectAnswerMonthlies.FirstOrDefaultAsync(x => x.QuestionMonthlyId.Equals(questionMonthlyId) && x.OptionMonthlyId.Equals(optionMonthly.Id));

            if (correctMonthly != null)
            {
                _context.CorrectAnswerMonthlies.Remove(correctMonthly);

                await _context.SaveChangesAsync();
            }
            await transaction.CommitAsync();
        }

        public async Task<PagedViewModel<OptionMonthlyDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = 10f;
            var pageCount = Math.Ceiling(_context.OptionMonthlies.Count() / (double)pageResult);
            var query = _context.OptionMonthlies.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new OptionMonthlyDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        QuestionMonthlyId = x.QuestionMonthlyId,
                                    })
                                    .ToListAsync();
            var OptionMonthlyResponse = new PagedViewModel<OptionMonthlyDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return OptionMonthlyResponse;
        }

        public async Task<IEnumerable<OptionMonthlyDto>> GetOptionMonthly()
        {
            var optionMonthly = await _context.OptionMonthlies.OrderByDescending(x => x.Name).AsNoTracking().ToListAsync();
            var optionMonthlyDto = _mapper.Map<List<OptionMonthlyDto>>(optionMonthly);
            return optionMonthlyDto;
        }

        public async Task<OptionMonthlyDto> GetOptionMonthlyById(Guid Id)
        {
            var optionMonthly = await _context.OptionMonthlies.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<OptionMonthlyDto>(optionMonthly);
        }

        public async Task InsertOptionMonthly(CreateOptionMonthlyDto createOptionMonthlyDto)
        {
            createOptionMonthlyDto.Id = Guid.NewGuid();


            OptionMonthly optionMonthly = _mapper.Map<OptionMonthly>(createOptionMonthlyDto);


            _context.Add(optionMonthly);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOptionMonthly(UpdateOptionMonthlyDto updateOptionMonthlyDto, Guid Id)
        {
            var item = await _context.OptionMonthlies.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.OptionMonthlies.Update(_mapper.Map(updateOptionMonthlyDto, item));

                await _context.SaveChangesAsync();
            }
        }

    }
}
