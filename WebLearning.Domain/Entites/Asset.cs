using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class Asset
    {
        [Key]
        public string Id { get; set; }

        public string OldAssetId { get; set; }

        public string AssetId { get; set; }

        public string AssetName { get; set; }

        public bool Active { get; set; }
        public string SeriNumber { get; set; }

        public string Price { get; set; }

        public string OrderNumber { get; set; }

        public Guid AssetsCategoryId { get; set; }

        public string AssetsSupplierId { get; set; }

        public string AssetSubCategory { get; set; }

        public int Quantity { get; set; }

        public Guid AssetsDepartmentId { get; set; }

        public string Customer { get; set; }
        public string Manager { get; set; }
        public int AssetsStatusId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateBuyed { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateUsed { get; set; }
        public int ExpireDay { get; set; }
        public DateTime? DateMoved { get; set; }

        public DateTime DateChecked { get; set; }

        public DateTime? DateRepaired { get; set; }

        public string RepairLocation { get; set; }

        public string Spec { get; set; }

        public string Note { get; set; }

        public string Region { get; set; }

        public string BusinessModel { get; set; }

        public AssetCategory AssetsCategory { get; set; }

        public AssetDepartment AssetsDepartment { get; set; }

        public AssetStatus AssetsStatus { get; set; }


        public ICollection<AssetMoved> AssetsMoveds { get; set; }

        public AssetSupplier AssetsSupplier { get; set; }

        public ICollection<AssetRepaired> AssetsRepaireds { get; set; }
    }
}
