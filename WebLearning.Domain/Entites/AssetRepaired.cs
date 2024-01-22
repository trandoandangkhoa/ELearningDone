namespace WebLearning.Domain.Entites
{
    public class AssetRepaired
    {
        public Guid Id { get; set; }

        public string AssestsId { get; set; }

        public string LocationRepaired { get; set; }

        public DateTime? DateRepaired { get; set; }

        public Asset Assests { get; set; }

    }
}
