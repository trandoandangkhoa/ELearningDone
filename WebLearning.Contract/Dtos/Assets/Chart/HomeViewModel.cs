using WebLearning.Contract.Dtos.Assets.Moved;

namespace WebLearning.Contract.Dtos.Assets.Chart
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public List<AssetsMovedDto> AssetsMovedDtos { get; set; }
        public List<TotalAsset> LsData { get; set; }
    }
}
