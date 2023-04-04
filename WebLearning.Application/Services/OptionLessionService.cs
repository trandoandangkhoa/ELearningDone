using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IOptionLessionService
    {
        Task<IEnumerable<OptionLessionDto>> GetOptionLession();
        Task<OptionLessionDto> GetOptionLessionById(Guid Id);
        Task InsertOptionLession(CreateOptionLessionDto createOptionLessionDto);
        Task DeleteOptionLession(Guid id, Guid questionLessionId);
        Task UpdateOptionLession(UpdateOptionLessionDto updateOptionLessionDto, Guid Id);
        Task<PagedViewModel<OptionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class OptionLessionService : IOptionLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public OptionLessionService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteOptionLession(Guid id, Guid questionLessionId)
        {
            var transaction = _context.Database.BeginTransaction();

            var optionLession = await _context.OptionLessions.FirstOrDefaultAsync(x => x.QuestionLessionId.Equals(questionLessionId) && x.Id.Equals(id));

            _context.OptionLessions.Remove(optionLession);

            await _context.SaveChangesAsync();

            var correctLession = await _context.CorrectAnswerLessions.FirstOrDefaultAsync(x => x.QuestionLessionId.Equals(questionLessionId) && x.OptionLessionId.Equals(optionLession.Id));

            if (correctLession != null)
            {
                _context.CorrectAnswerLessions.Remove(correctLession);

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task<PagedViewModel<OptionLessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = 10f;
            var pageCount = Math.Ceiling(_context.OptionLessions.Count() / (double)pageResult);
            var query = _context.OptionLessions.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new OptionLessionDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        QuestionLessionId = x.QuestionLessionId,
                                    })
                                    .ToListAsync();
            var OptionLessionResponse = new PagedViewModel<OptionLessionDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return OptionLessionResponse;
        }

        public async Task<IEnumerable<OptionLessionDto>> GetOptionLession()
        {
            var optionLession = await _context.OptionLessions.OrderByDescending(x => x.Name).AsNoTracking().ToListAsync();
            var optionLessionDto = _mapper.Map<List<OptionLessionDto>>(optionLession);
            return optionLessionDto;
        }

        public async Task<OptionLessionDto> GetOptionLessionById(Guid Id)
        {
            var optionLession = await _context.OptionLessions.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<OptionLessionDto>(optionLession);
        }

        public async Task InsertOptionLession(CreateOptionLessionDto createOptionLessionDto)
        {
            createOptionLessionDto.Id = Guid.NewGuid();
            OptionLession optionLession = _mapper.Map<OptionLession>(createOptionLessionDto);


            _context.Add(optionLession);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOptionLession(UpdateOptionLessionDto updateOptionLessionDto, Guid Id)
        {
            var item = await _context.OptionLessions.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {
                _context.OptionLessions.Update(_mapper.Map(updateOptionLessionDto, item));

                await _context.SaveChangesAsync();
            }
        }

    }
}
