using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.ImageCourse
{
    public class UpdateCourseImageDto
    {
        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
