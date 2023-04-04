using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.Course
{
    public class CreateCourseDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public DateTime DateCreated { get; set; }
        public string Alias { get; set; }
        public IFormFile Image { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
