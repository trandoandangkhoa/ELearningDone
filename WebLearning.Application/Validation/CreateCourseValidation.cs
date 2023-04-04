using FluentValidation;
using WebLearning.Contract.Dtos.Course;

namespace WebLearning.Application.Validation
{
    public class CreateCourseValidation : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseValidation()
        {
            // Check name is not null, empty and is between 1 and 250 characters
            RuleFor(course => course.Name).NotEmpty().WithMessage("Vui lòng điền tên khóa học!");

            RuleFor(course => course.Description).NotEmpty().WithMessage("Vui lòng điền tóm tắt khóa học!");
            RuleFor(course => course.Image).NotEmpty().WithMessage("Vui lòng chọn ảnh khóa học!");

        }
    }
}
