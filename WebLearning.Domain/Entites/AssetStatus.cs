namespace WebLearning.Domain.Entites
{
    public class AssetStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Asset> Assests { get; set; }
    }
}
