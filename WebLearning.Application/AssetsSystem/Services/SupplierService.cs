using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Supplier;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<AssetsSupplierDto>> GetAssetsSupplier();
        Task<AssetsSupplierDto> GetAssetsSupplierById(string id);
        Task InsertAssetsSupplier(CreateAssetsSupplierDto createAssetSupplierDto);
        Task<AssetsSupplierDto> GetName(string name);
        Task DeleteAssetsSupplier(string id);
        Task UpdateAssetsSupplier(UpdateAssetsSupplierDto updateAssetsSupplierDto, string id);
        Task<PagedViewModel<AssetsSupplierDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<AssetsSupplierDto> GetCode(string code);
    }
    public class SupplierService : ISupplierService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public SupplierService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsSupplier(string id)
        {

            var asset = await _context.AssetsSuppliers.FindAsync(id);
            _context.AssetsSuppliers.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsSupplierDto>> GetAssetsSupplier()
        {
            var asset = await _context.AssetsSuppliers.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsSupplierDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsSupplierDto> GetAssetsSupplierById(string id)
        {
            var asset = await _context.AssetsSuppliers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsSupplierDto>(asset);
        }

        public async Task<AssetsSupplierDto> GetCode(string code)
        {
            var asset = await _context.AssetsSuppliers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Contains(code));

            return _mapper.Map<AssetsSupplierDto>(asset);
        }

        public Task<AssetsSupplierDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsSupplierDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsSupplier");
            var pageCount = Math.Ceiling(_context.AssetsSuppliers.Count() / (double)pageResult);
            var query = _context.AssetsSuppliers.Include(x => x.Assests).AsNoTracking().OrderByDescending(x => x.Id).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CompanyName.Contains(getListPagingRequest.Keyword) || x.Id.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsSupplierDto()
                                    {
                                        Id = x.Id,
                                        CompanyName = x.CompanyName,
                                        Address = x.Address,
                                        Phone = x.Phone,
                                        CompanyTaxCode = x.CompanyTaxCode,
                                        Fax = x.Fax,
                                    }).AsNoTracking()
                                    .OrderByDescending(x => x.Id).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsSupplierDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsSupplier(CreateAssetsSupplierDto createAssetSupplierDto)
        {
            string s = _configuration.GetValue<string>("Code:AssetsSupplier");


            if (_context.AssetsSuppliers.Any(x => x.CompanyName.Equals(createAssetSupplierDto.CompanyName)) == false)
            {
                var list = await _context.AssetsSuppliers.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();

                if (list.Count == 0) { createAssetSupplierDto.Id = s + "00000001"; }

                else if (list.Count > 0)
                {
                    int k = Convert.ToInt32(list[0].Id.Substring(6, 8)) + 1;
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
                    createAssetSupplierDto.Id = s;
                }
                AssetSupplier asset = _mapper.Map<AssetSupplier>(createAssetSupplierDto);

                _context.Add(asset);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateAssetsSupplier(UpdateAssetsSupplierDto updateAssetsSupplierDto, string id)
        {
            var item = await _context.AssetsSuppliers.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.AssetsSuppliers.Update(_mapper.Map(updateAssetsSupplierDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
