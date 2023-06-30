using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<AssetsDepartmentDto>> GetAssetsDepartment();
        Task<AssetsDepartmentDto> GetAssetsDepartmentById(Guid id);
        Task InsertAssetsDepartment(CreateAssetsDepartmentDto createAssetsDepartmentDto);
        Task<AssetsDepartmentDto> GetName(string name);
        Task DeleteAssetsDepartment(Guid id);
        Task UpdateAssetsDepartment(UpdateAssetsDepartmentDto updateAssetsDepartmentDto, Guid id);
        Task<PagedViewModel<AssetsDepartmentDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<AssetsDepartmentDto> GetCode(string code);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public DepartmentService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsDepartment(Guid id)
        {

            var asset = await _context.AssetsDepartments.FindAsync(id);
            _context.AssetsDepartments.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsDepartmentDto>> GetAssetsDepartment()
        {
            var asset = await _context.AssetsDepartments.OrderByDescending(x => x.Code).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsDepartmentDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsDepartmentDto> GetAssetsDepartmentById(Guid id)
        {
            var asset = await _context.AssetsDepartments.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsDepartmentDto>(asset);
        }

        public async Task<AssetsDepartmentDto> GetCode(string code)
        {
            var asset = await _context.AssetsDepartments.Include(x => x.Assests).FirstOrDefaultAsync(x => x.Code.Contains(code));

            return _mapper.Map<AssetsDepartmentDto>(asset);
        }

        public async Task<AssetsDepartmentDto> GetName(string name)
        {
            var asset = await _context.AssetsDepartments.Include(x => x.Assests).FirstOrDefaultAsync(x => x.Name.Contains(name));

            return _mapper.Map<AssetsDepartmentDto>(asset);
        }

        public async Task<PagedViewModel<AssetsDepartmentDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsDepartment");
            var pageCount = Math.Ceiling(_context.AssetsDepartments.Count() / (double)pageResult);
            var query = _context.AssetsDepartments.OrderByDescending(x => x.Code).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsDepartmentDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Code = x.Code,
                                    })
                                    .OrderByDescending(x => x.Code).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsDepartmentDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsDepartment(CreateAssetsDepartmentDto createAssetsDepartmentDto)
        {
            string s = _configuration.GetValue<string>("Code:AssetsDepartment");


            if (_context.AssetsDepartments.Any(x => x.Name.Equals(createAssetsDepartmentDto.Name)) == false)
            {
                var list = await _context.AssetsDepartments.AsNoTracking().OrderByDescending(x => x.Code).ToListAsync();

                if (list.Count == 0) { createAssetsDepartmentDto.Code = s + "00000001"; }

                else if (list.Count > 0)
                {
                    int k = Convert.ToInt32(list[0].Code.Substring(6, 8)) + 1;
                    if (k < 10) s += "0000000";
                    else if (k < 100)
                        s += "000000";
                    else if (k < 1000)
                        s += "00000";
                    else if (k < 10000)
                        s += "0000";
                    else if (k < 100000)
                        s += "000";
                    else if (k < 1000000)
                        s += "00";
                    else if (k < 10000000)
                        s += "0";
                    s += k.ToString();
                    createAssetsDepartmentDto.Code = s;
                }
                AssetsDepartment asset = _mapper.Map<AssetsDepartment>(createAssetsDepartmentDto);

                _context.Add(asset);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateAssetsDepartment(UpdateAssetsDepartmentDto updateAssetsDepartmentDto, Guid id)
        {
            var item = await _context.AssetsDepartments.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.AssetsDepartments.Update(_mapper.Map(updateAssetsDepartmentDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
