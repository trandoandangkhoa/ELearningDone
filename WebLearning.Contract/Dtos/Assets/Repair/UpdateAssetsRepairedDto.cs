namespace WebLearning.Contract.Dtos.Assets.Repair
{
    public class UpdateAssetsRepairedDto
    {
        public string AssetsId { get; set; }
        public string LocationRepaired { get; set; }

        public DateTime? DateRepaired { get; set; }
    }
}
