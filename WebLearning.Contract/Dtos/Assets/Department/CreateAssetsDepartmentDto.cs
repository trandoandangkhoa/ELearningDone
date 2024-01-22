namespace WebLearning.Contract.Dtos.Assets.Department
{
    public class CreateAssetsDepartmentDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public bool Location { get; set; }
        public string ParentCode { get; set; }

        public string Level { get; set; }
    }
}
