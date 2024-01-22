using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.Chart;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IChartService
    {
        Task<IEnumerable<TotalAsset>> TotalAssetsCategory();
        Task<IEnumerable<TotalAsset>> TotalAssetsStatus();
        Task<IEnumerable<TotalAsset>> TotalAssetsAvailable();

    }
    public class ChartService : IChartService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        public ChartService(WebLearningContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsAvailable()
        {
            var asset = await _context.Assests.AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsDto>>(asset);

            var total = new List<TotalAsset>();

            var disable = assetDto.Where(x => x.Active == false).ToList();
            var available = assetDto.Where(x => x.Active == true).ToList();

            total.Add(new TotalAsset
            {
                Name = "không hoạt động",
                Quantity = disable.Count,
            });
            total.Add(new TotalAsset
            {
                Name = "Hoạt động",
                Quantity = available.Count,
            });
            return total;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsCategory()
        {
            var cat = await _context.AssetsCategories.OrderByDescending(x => x.CatCode).AsNoTracking().ToListAsync();
            var asset = await _context.Assests.AsNoTracking().ToListAsync();

            var catDto = _mapper.Map<List<AssetsCategoryDto>>(cat);
            var assetDto = _mapper.Map<List<AssetsDto>>(asset);

            var total = new List<TotalAsset>();
            foreach (var item in catDto)
            {
                total.Add(new TotalAsset
                {
                    Name = item.Name,
                    Quantity = assetDto.Where(x => x.AssetsCategoryId.Equals(item.Id)).Count(),
                });
            }
            return total;
        }

        public async Task<IEnumerable<TotalAsset>> TotalAssetsStatus()
        {
            var stt = await _context.AssetsStatuses.AsNoTracking().ToListAsync();
            var asset = await _context.Assests.AsNoTracking().ToListAsync();
            var sttDto = _mapper.Map<List<AssetsStatusDto>>(stt);
            var assetDto = _mapper.Map<List<AssetsDto>>(asset);

            var total = new List<TotalAsset>();
            foreach (var item in sttDto)
            {
                total.Add(new TotalAsset
                {
                    Name = item.Name,
                    Quantity = assetDto.Where(x => x.AssetsStatusId == item.Id).Count(),
                });
            }
            return total;
        }
    }
}
