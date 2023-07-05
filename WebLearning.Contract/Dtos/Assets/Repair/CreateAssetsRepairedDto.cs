namespace WebLearning.Contract.Dtos.Assets.Repair
{
    public class CreateAssetsRepairedDto
    {
        public Guid Id { get; set; }

        public string AssestsId { get; set; }

        public string LocationRepaired { get; set; }

        public DateTime? DateRepaired { get; set; }

    }
}
