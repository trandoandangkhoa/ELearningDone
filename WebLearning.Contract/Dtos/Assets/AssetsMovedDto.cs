using WebLearning.Contract.Dtos.Assets.Department;

namespace WebLearning.Contract.Dtos.Assets
{
    public class AssetsMovedDto
    {
        public Guid Id { get; set; }

        public string AssetsId { get; set; }

        public int MovedStatus { get; set; }

        public string NumBravo { get; set; }

        public string Receiver { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string Description { get; set; }

        public Guid AssetsDepartmentId { get; set; }

        public DateTime? DateUsed { get; set; }

        public DateTime? DateMoved { get; set; }


        public AssetsDto AssetsDto { get; set; }

        public AssetsMovedStatusDto AssetsMovedStatusDto { get; set; }

        public AssetsDepartmentDto AssetsDepartmentDto { get; set; }

    }
}
