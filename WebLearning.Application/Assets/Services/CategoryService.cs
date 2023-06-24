using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<AssetsCategoryDto>> GetAssetsCategory();
        Task<AssetsCategoryDto> GetAssetsCategoryById(Guid id);
        Task InsertAssetsCategory(CreateAssetsCategoryDto createAssetCategoryDto);
        Task<AssetsCategoryDto> GetName(string name);
        Task DeleteAssetsCategory(Guid id);
        Task UpdateAssetsCategory(UpdateAssetsCategoryDto updateAssetsCategoryDto, Guid id);
        Task<PagedViewModel<AssetsCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<AssetsCategoryDto> GetCode(string code);
    }
    public class CategoryService : ICategoryService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public CategoryService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsCategory(Guid id)
        {

            var asset = await _context.AssetsCategories.FindAsync(id);
            _context.AssetsCategories.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsCategoryDto>> GetAssetsCategory()
        {
            var asset = await _context.AssetsCategories.OrderByDescending(x => x.Name).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsCategoryDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsCategoryDto> GetAssetsCategoryById(Guid id)
        {
            var asset = await _context.AssetsCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsCategoryDto>(asset);
        }

        public async Task<AssetsCategoryDto> GetCode(string code)
        {
            var asset = await _context.AssetsCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CatCode.Contains(code));

            return _mapper.Map<AssetsCategoryDto>(asset);
        }

        public Task<AssetsCategoryDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsCategory");
            var pageCount = Math.Ceiling(_context.AssetsCategories.Count() / (double)pageResult);
            var query = _context.AssetsCategories.Include(x => x.Assests).AsNoTracking().OrderByDescending(x => x.CatCode).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword) || x.CatCode.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsCategoryDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        CatCode = x.CatCode,
                                    }).AsNoTracking()
                                    .OrderByDescending(x => x.CatCode).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsCategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsCategory(CreateAssetsCategoryDto createAssetCategoryDto)
        {
            createAssetCategoryDto.Id = Guid.NewGuid();
            string s = _configuration.GetValue<string>("Code:AssetsCategory");


            if (_context.AssetsCategories.Any(x => x.Name.Equals(createAssetCategoryDto.Name)) == false)
            {
                var list = await _context.AssetsCategories.AsNoTracking().OrderByDescending(x => x.CatCode).ToListAsync();

                if (list.Count == 0) { createAssetCategoryDto.CatCode = s + "00000001"; }

                else if (list.Count > 0)
                {
                    int k = Convert.ToInt32(list[0].CatCode.Substring(6, 8)) + 1;
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
                    createAssetCategoryDto.CatCode = s;
                }
                AssetsCategory asset = _mapper.Map<AssetsCategory>(createAssetCategoryDto);

                _context.Add(asset);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateAssetsCategory(UpdateAssetsCategoryDto updateAssetsCategoryDto, Guid id)
        {
            var item = await _context.AssetsCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.AssetsCategories.Update(_mapper.Map(updateAssetsCategoryDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
