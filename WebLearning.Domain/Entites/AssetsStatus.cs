namespace WebLearning.Domain.Entites
{
    public class AssetsStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Assests> Assests { get; set; }
    }
}
