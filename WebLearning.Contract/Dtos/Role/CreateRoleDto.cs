namespace WebLearning.Contract.Dtos.Role
{
    public class CreateRoleDto
    {
        public Guid Id = Guid.NewGuid();
        public string Code { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

    }
}
