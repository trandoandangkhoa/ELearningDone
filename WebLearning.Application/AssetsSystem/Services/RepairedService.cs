using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Repair;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IRepairedService
    {
        Task<IEnumerable<AssetsRepairedDto>> GetAssetsRepaired();
        Task<AssetsRepairedDto> GetAssetsRepairedById(Guid id);
        Task InsertAssetsRepaired(CreateAssetsRepairedDto createAssetRepairedDto);
        Task<AssetsRepairedDto> GetName(string name);
        Task DeleteAssetsRepaired(Guid id);
        Task UpdateAssetsRepaired(UpdateAssetsRepairedDto updateAssetsRepairedDto, Guid id);
        Task<PagedViewModel<AssetsRepairedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class RepairedService : IRepairedService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RepairedService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsRepaired(Guid id)
        {

            var asset = await _context.AssetsRepaireds.FindAsync(id);
            _context.AssetsRepaireds.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsRepairedDto>> GetAssetsRepaired()
        {
            var asset = await _context.AssetsRepaireds.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsRepairedDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsRepairedDto> GetAssetsRepairedById(Guid id)
        {
            var asset = await _context.AssetsRepaireds.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsRepairedDto>(asset);
        }

        public Task<AssetsRepairedDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsRepairedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsRepaired");
            var pageCount = Math.Ceiling(_context.AssetsRepaireds.Count() / (double)pageResult);
            var query = _context.AssetsRepaireds.Include(x => x.Assests).AsNoTracking().OrderByDescending(x => x.Id).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.LocationRepaired.Contains(getListPagingRequest.Keyword) || x.AssestsId.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsRepairedDto()
                                    {
                                        Id = x.Id,
                                        AssestsId = x.AssestsId,
                                        DateRepaired = x.DateRepaired,
                                        LocationRepaired = x.LocationRepaired,
                                    }).AsNoTracking()
                                    .OrderByDescending(x => x.Id).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsRepairedDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsRepaired(CreateAssetsRepairedDto createAssetRepairedDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            if (createAssetRepairedDto.AssestsId != null)
            {
                createAssetRepairedDto.Id = Guid.NewGuid();

                var asset = await _context.Assests.FirstOrDefaultAsync(x => x.Id == createAssetRepairedDto.AssestsId);
                if (asset == null) return;
                //var asset = _mapper.Map<AssetsDto>(assetDb);


                UpdateAssetsDto updateAssetsDto = new()
                {
                    AssetId = asset.AssetId,
                    Active = asset.Active,
                    AssetName = asset.AssetName,
                    AssetsCategoryId = asset.AssetsCategoryId,
                    AssetsDepartmentId = asset.AssetsDepartmentId,
                    AssetsStatusId = asset.AssetsStatusId,
                    AssetSubCategory = asset.AssetSubCategory,
                    Customer = asset.Customer,
                    DateBuyed = asset.DateBuyed,
                    DateChecked = asset.DateChecked,
                    DateCreated = asset.DateCreated,
                    DateExpired = asset.DateRepaired,
                    DateMoved = asset.DateMoved,
                    DateRepaired = createAssetRepairedDto.DateRepaired,
                    DateUsed = asset.DateUsed,
                    ExpireDay = asset.ExpireDay,
                    Manager = asset.Manager,
                    Note = asset.Note,
                    OrderNumber = asset.OrderNumber,
                    Price = asset.Price,
                    Quantity = asset.Quantity,
                    RepairLocation = createAssetRepairedDto.LocationRepaired,
                    SeriNumber = asset.SeriNumber,
                    Spec = asset.Spec,
                    AssetsSupplierId = asset.AssetsSupplierId,
                    Region = asset.Region,
                    BusinessModel = asset.BusinessModel,

                };
                _context.Assests.Update(_mapper.Map(updateAssetsDto, asset));


                await _context.SaveChangesAsync();

            }
            AssetRepaired assetRepair = _mapper.Map<AssetRepaired>(createAssetRepairedDto);

            _context.Add(assetRepair);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

        }

        public async Task UpdateAssetsRepaired(UpdateAssetsRepairedDto updateAssetsRepairedDto, Guid id)
        {
            var item = await _context.AssetsRepaireds.FirstOrDefaultAsync(x => x.Id.Equals(id));

            var firstItem = await _context.AssetsRepaireds.AsNoTracking().OrderByDescending(x => x.DateRepaired).Select(x => x.Id).FirstAsync();

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                var asset = await _context.Assests.FirstOrDefaultAsync(x => x.Id == updateAssetsRepairedDto.AssetsId);

                if (asset == null) return;

                _context.AssetsRepaireds.Update(_mapper.Map(updateAssetsRepairedDto, item));

                await _context.SaveChangesAsync();

                var firstItemAfterUpdate = await _context.AssetsRepaireds.AsNoTracking().OrderByDescending(x => x.DateRepaired).Select(x => x.Id).FirstAsync();

                if (item.Id.Equals(firstItem) || item.Id.Equals(firstItemAfterUpdate))
                {
                    UpdateAssetsDto updateAssetsDto = new()
                    {
                        AssetId = asset.AssetId,
                        Active = asset.Active,
                        AssetName = asset.AssetName,
                        AssetsCategoryId = asset.AssetsCategoryId,
                        AssetsDepartmentId = asset.AssetsDepartmentId,
                        AssetsStatusId = asset.AssetsStatusId,
                        AssetSubCategory = asset.AssetSubCategory,
                        Customer = asset.Customer,
                        DateBuyed = asset.DateBuyed,
                        DateChecked = asset.DateChecked,
                        DateCreated = asset.DateCreated,
                        DateExpired = asset.DateExpired,
                        DateMoved = asset.DateMoved,
                        DateRepaired = updateAssetsRepairedDto.DateRepaired,
                        DateUsed = asset.DateUsed,
                        ExpireDay = asset.ExpireDay,
                        Manager = asset.Manager,
                        Note = asset.Note,
                        OrderNumber = asset.OrderNumber,
                        Price = asset.Price,
                        Quantity = asset.Quantity,
                        RepairLocation = updateAssetsRepairedDto.LocationRepaired,
                        SeriNumber = asset.SeriNumber,
                        Spec = asset.Spec,
                        AssetsSupplierId = asset.AssetsSupplierId,
                        BusinessModel = asset.BusinessModel,
                        Region = asset.Region,
                    };
                    _context.Assests.Update(_mapper.Map(updateAssetsDto, asset));

                    await _context.SaveChangesAsync();
                }




                await transaction.CommitAsync();
            }
        }
    }
}
