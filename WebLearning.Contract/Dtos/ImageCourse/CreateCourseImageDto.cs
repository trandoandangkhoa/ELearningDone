using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.ImageCourse
{
    public class CreateCourseImageDto
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }

        public bool IsDefault { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
