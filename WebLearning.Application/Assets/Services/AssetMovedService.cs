using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{

    public interface IAssetMovedService
    {
        Task<IEnumerable<AssetsMovedDto>> GetAssetsMoved();
        Task<AssetsMovedDto> GetAssetsMovedById(Guid id);
        Task InsertAssetsMoved(CreateAssetsMovedDto createAssetsMovedDto);
        Task<AssetsMovedDto> GetName(string name);
        Task DeleteAssetsMoved(Guid id);
        Task UpdateAssetsMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id);
    }

    public class AssetMovedService : IAssetMovedService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAssetService _assetService;

        public AssetMovedService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IAssetService assetService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _assetService = assetService;
        }
        public async Task DeleteAssetsMoved(Guid id)
        {

            var asset = await _context.AssetsMoveds.FindAsync(id);

            _context.AssetsMoveds.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsMovedDto>> GetAssetsMoved()
        {
            var asset = await _context.AssetsMoveds.Include(x => x.AssetsMovedStatus).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsMovedDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsMovedDto> GetAssetsMovedById(Guid id)
        {
            var asset = await _context.AssetsMoveds.Include(x => x.Assests).FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsMovedDto>(asset);
        }


        public Task<AssetsMovedDto> GetName(string name)
        {
            throw new NotImplementedException();
        }



        public async Task InsertAssetsMoved(CreateAssetsMovedDto createAssetsMovedDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            createAssetsMovedDto.Id = Guid.NewGuid();


            createAssetsMovedDto.MovedStatus = 1;


            createAssetsMovedDto.DateMoved = DateTime.Now;

            if (createAssetsMovedDto.AssestsId != null)
            {
                var asset = await _assetService.GetAssetById(createAssetsMovedDto.AssestsId);

                if (asset == null) return;

                createAssetsMovedDto.NumBravo = asset.AssetId;

                asset.DateMoved = createAssetsMovedDto.DateMoved;

                asset.Customer = createAssetsMovedDto.Receiver;

                asset.AssetsDepartmentId = createAssetsMovedDto.AssetsDepartmentId;

                UpdateAssetsDto updateAssetsDto = new()
                {
                    AssetId = asset.AssetId,
                    Active = asset.Active,
                    AssetName = asset.AssetName,
                    AssetsCategoryId = asset.AssetsCategoryId,
                    AssetsDepartmentId = createAssetsMovedDto.AssetsDepartmentId,
                    AssetsStatusId = asset.AssetsStatusId,
                    AssetSubCategory = asset.AssetSubCategory,
                    Customer = createAssetsMovedDto.Receiver,
                    DateBuyed = asset.DateBuyed,
                    DateChecked = asset.DateChecked,
                    DateCreated = asset.DateCreated,
                    DateExpired = asset.DateExpired,
                    DateMoved = createAssetsMovedDto.DateMoved,
                    DateRepaired = asset.DateRepaired,
                    DateUsed = createAssetsMovedDto.DateUsed,
                    ExpireDay = asset.ExpireDay,
                    Manager = asset.Manager,
                    Note = asset.Note,
                    OrderNumber = asset.OrderNumber,
                    Price = asset.Price,
                    Quantity = asset.Quantity,
                    RepairLocation = asset.RepairLocation,
                    SeriNumber = asset.SeriNumber,
                    Spec = asset.Spec,
                    Supplier = asset.Supplier,
                };
                await _assetService.UpdateAsset(updateAssetsDto, createAssetsMovedDto.AssestsId);

                await _context.SaveChangesAsync();

            }


            AssetsMoved assetMoved = _mapper.Map<AssetsMoved>(createAssetsMovedDto);


            _context.Add(assetMoved);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return;
        }

        public async Task UpdateAssetsMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id)
        {
            var item = await _context.AssetsMoveds.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {
                updateAssetsMovedDto.DateMoved = DateTime.Now;
                using var transaction = _context.Database.BeginTransaction();

                var asset = await _assetService.GetAssetById(updateAssetsMovedDto.AssetId);

                if (asset == null) return;

                updateAssetsMovedDto.MovedStatus = 1;

                updateAssetsMovedDto.NumBravo = asset.AssetId;

                asset.DateMoved = updateAssetsMovedDto.DateMoved;

                asset.Customer = updateAssetsMovedDto.Receiver;

                asset.AssetsDepartmentId = updateAssetsMovedDto.AssetsDepartmentId;

                UpdateAssetsDto updateAssetsDto = new()
                {
                    AssetId = asset.AssetId,
                    Active = asset.Active,
                    AssetName = asset.AssetName,
                    AssetsCategoryId = asset.AssetsCategoryId,
                    AssetsDepartmentId = updateAssetsMovedDto.AssetsDepartmentId,
                    AssetsStatusId = asset.AssetsStatusId,
                    AssetSubCategory = asset.AssetSubCategory,
                    Customer = updateAssetsMovedDto.Receiver,
                    DateBuyed = asset.DateBuyed,
                    DateChecked = asset.DateChecked,
                    DateCreated = asset.DateCreated,
                    DateExpired = asset.DateExpired,
                    DateMoved = updateAssetsMovedDto.DateMoved,
                    DateRepaired = asset.DateRepaired,
                    DateUsed = updateAssetsMovedDto.DateUsed,
                    ExpireDay = asset.ExpireDay,
                    Manager = asset.Manager,
                    Note = asset.Note,
                    OrderNumber = asset.OrderNumber,
                    Price = asset.Price,
                    Quantity = asset.Quantity,
                    RepairLocation = asset.RepairLocation,
                    SeriNumber = asset.SeriNumber,
                    Spec = asset.Spec,
                    Supplier = asset.Supplier,
                };
                await _assetService.UpdateAsset(updateAssetsDto, updateAssetsMovedDto.AssetId);

                await _context.SaveChangesAsync();

                _context.AssetsMoveds.Update(_mapper.Map(updateAssetsMovedDto, item));

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

    }
}
