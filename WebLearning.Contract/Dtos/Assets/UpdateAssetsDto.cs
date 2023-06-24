namespace WebLearning.Contract.Dtos.Assets
{
    public class UpdateAssetsDto
    {
        public string AssetId { get; set; }

        public string AssetName { get; set; }
        public string SeriNumber { get; set; }

        public string Price { get; set; }

        public string Supplier { get; set; }

        public string OrderNumber { get; set; }
        public Guid AssetsCategoryId { get; set; }

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

        public DateTime? DateRepaired { get; set; }

        public string RepairLocation { get; set; }
        public bool Active { get; set; }

        public DateTime DateChecked { get; set; }

        public string Spec { get; set; }

        public string Note { get; set; }
    }
}
