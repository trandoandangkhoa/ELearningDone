namespace WebLearning.Domain.Entites
{
    public class AssetsSupplier
    {
        public string Id { get; set; }
        public string CompanyTaxCode { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public ICollection<Assests> Assests { get; set; }
    }
}
