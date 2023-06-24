namespace WebLearning.Contract.Dtos.Assets
{
    public class UpdateAssetsMovedDto
    {
        public Guid IdTemp { get; set; }
        public string AssetId { get; set; }

        public int MovedStatus { get; set; }

        public string NumBravo { get; set; }

        public string Receiver { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string Description { get; set; }

        public Guid AssetsDepartmentId { get; set; }

        public DateTime? DateUsed { get; set; }

        public DateTime? DateMoved { get; set; }
    }
}
