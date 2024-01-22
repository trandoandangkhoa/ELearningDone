namespace WebLearning.Domain.Entites
{
    public class AssetDepartment
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }

        public string Level { get; set; }

        public ICollection<Asset> Assests { get; set; }


    }
}
