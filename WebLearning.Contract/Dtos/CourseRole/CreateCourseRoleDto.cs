namespace WebLearning.Contract.Dtos.CourseRole
{
    public class CreateCourseRoleDto
    {

        public Guid Id { get; set; }


        public Guid CourseId { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string Code { get; set; }

        public string CodeRole { get; set; }

        public string CodeCourse { get; set; }
    }
}
