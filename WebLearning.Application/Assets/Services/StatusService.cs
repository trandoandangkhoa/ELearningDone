using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<AssetsStatusDto>> GetAssetsStatus();
        Task<AssetsStatusDto> GetAssetsStatusById(int id);
        Task InsertAssetsStatus(CreateAssetsStatusDto createAssetsStatusDto);
        Task<AssetsStatusDto> GetName(string name);
        Task DeleteAssetsStatus(int id);
        Task UpdateAssetsStatus(UpdateAssetsStatusDto updateAssetsStatusDto, int id);
        Task<PagedViewModel<AssetsStatusDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class StatusService : IStatusService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public StatusService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsStatus(int id)
        {

            var asset = await _context.AssetsStatuses.FindAsync(id);
            _context.AssetsStatuses.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsStatusDto>> GetAssetsStatus()
        {
            var asset = await _context.AssetsStatuses.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsStatusDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsStatusDto> GetAssetsStatusById(int id)
        {
            var asset = await _context.AssetsStatuses.Include(x => x.Assests).FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsStatusDto>(asset);
        }


        public Task<AssetsStatusDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsStatusDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsStatus");
            var pageCount = Math.Ceiling(_context.AssetsStatuses.Count() / (double)pageResult);
            var query = _context.AssetsStatuses.OrderByDescending(x => x.Id).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsStatusDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                    })
                                    .OrderByDescending(x => x.Id).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsStatusDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsStatus(CreateAssetsStatusDto createAssetsStatusDto)
        {

            if (_context.AssetsStatuses.Any(x => x.Name.Equals(createAssetsStatusDto.Name)) == false)
            {

                AssetsStatus asset = _mapper.Map<AssetsStatus>(createAssetsStatusDto);

                _context.Add(asset);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateAssetsStatus(UpdateAssetsStatusDto updateAssetsStatusDto, int id)
        {
            var item = await _context.AssetsStatuses.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.AssetsStatuses.Update(_mapper.Map(updateAssetsStatusDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
