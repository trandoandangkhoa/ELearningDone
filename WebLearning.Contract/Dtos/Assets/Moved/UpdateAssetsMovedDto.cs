namespace WebLearning.Contract.Dtos.Assets.Moved
{
    public class UpdateAssetsMovedDto
    {
        public Guid IdTemp { get; set; }

        public string OldAssestsId { get;set; }
        public string AssestsId { get; set; }

        public int AssetsMovedStatusId { get; set; }

        public Guid NewDepartmentId { get; set; }


        public string Receiver { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public DateTime? DateUsed { get; set; }

        public DateTime? DateMoved { get; set; }
    }
}
