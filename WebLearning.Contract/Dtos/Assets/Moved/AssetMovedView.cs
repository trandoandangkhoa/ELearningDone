namespace WebLearning.Contract.Dtos.Assets.Moved
{
    public class AssetMovedView
    {
        public List<CreateAssetsMovedDto> createAssetsMovedDtos = new List<CreateAssetsMovedDto>();
        public List<AssetMovedTicket> assetMovedTickets = new List<AssetMovedTicket>();
        public AssetMovedPrintView AssetMovedPrintView = new AssetMovedPrintView();
    }
}
