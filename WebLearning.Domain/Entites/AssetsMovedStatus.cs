namespace WebLearning.Domain.Entites
{
    public class AssetsMovedStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AssetsMoved> AssetsMoved { get; set; }
    }
}
