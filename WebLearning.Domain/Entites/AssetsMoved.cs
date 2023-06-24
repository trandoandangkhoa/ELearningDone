using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class AssetsMoved
    {
        public Guid Id { get; set; }

        public string AssestsId { get; set; }

        [ForeignKey("AssetsMovedStatus")]

        public int MovedStatus { get; set; }

        [ForeignKey("AssetsDepartment")]
        public Guid AssetsDepartmentId { get; set; }

        public string NumBravo { get; set; }

        public string Receiver { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string Description { get; set; }


        public DateTime? DateUsed { get; set; }

        public DateTime? DateMoved { get; set; }


        public Assests Assests { get; set; }

        public AssetsMovedStatus AssetsMovedStatus { get; set; }

        public AssetsDepartment AssetsDepartment { get; set; }
    }
}
