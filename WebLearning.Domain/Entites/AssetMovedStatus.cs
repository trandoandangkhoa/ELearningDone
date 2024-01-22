namespace WebLearning.Domain.Entites
{
    public class AssetMovedStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AssetMoved> AssetsMoved { get; set; }
    }
}
