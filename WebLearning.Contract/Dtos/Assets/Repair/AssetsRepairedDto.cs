namespace WebLearning.Contract.Dtos.Assets.Repair
{
    public class AssetsRepairedDto
    {
        public Guid Id { get; set; }

        public string AssestsId { get; set; }

        public string LocationRepaired { get; set; }

        public DateTime? DateRepaired { get; set; }

        public AssetsDto AssetsDto { get; set; }
    }
}
