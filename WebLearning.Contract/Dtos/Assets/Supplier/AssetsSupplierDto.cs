namespace WebLearning.Contract.Dtos.Assets.Supplier
{
    public class AssetsSupplierDto
    {
        public string Id { get; set; }
        public string CompanyTaxCode { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public ICollection<AssetsDto> AssetsDtos { get; set; }
    }
}
