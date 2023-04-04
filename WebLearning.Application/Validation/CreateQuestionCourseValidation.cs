using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;

namespace WebLearning.Application.Validation
{
    public class CreateQuestionCourseValidation : AbstractValidator<CreateAllConcerningQuestionCourseDto>
    {
        public CreateQuestionCourseValidation()
        {
            RuleFor(ls => ls.CreateQuestionCourseDto.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi!");
            RuleFor(ls => ls.CreateQuestionCourseDto.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
        }
    }
}
