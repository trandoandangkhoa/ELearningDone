namespace WebLearning.Domain.Entites
{
    public class AssetMoved
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string OldAssestsId { get; set; }

        public string AssestsId { get; set; }

        public string NumBravo { get; set; }


        public int AssetsMovedStatusId { get; set; }

        public Guid OldDepartmentId { get; set; }

        public Guid NewDepartmentId { get; set; }

        public string Unit { get; set; }

        public string Receiver { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string Description { get; set; }
        public string AssetNote { get; set; }

        public string AssetStatus { get; set; }

        public int Quantity { get; set; }

        public DateTime? DateUsed { get; set; }

        public DateTime? DateMoved { get; set; }


        public Asset Assests { get; set; }
        public AssetMovedHistory AssetMovedHistory { get; set; }

        public AssetMovedStatus AssetsMovedStatus { get; set; }

    }
}
