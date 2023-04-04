using WebLearning.Contract.Dtos.CourseRole;

namespace WebLearning.Contract.Dtos.Role
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string RoleName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }

        public ICollection<CourseRoleDto> CourseRoleDtos { get; set; }
    }
}
