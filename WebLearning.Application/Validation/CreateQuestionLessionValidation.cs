using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;

namespace WebLearning.Application.Validation
{
    public class CreateQuestionLessionValidation : AbstractValidator<CreateAllConcerningQuestionLessionDto>
    {
        public CreateQuestionLessionValidation()
        {
            RuleFor(ls => ls.CreateQuestionLessionDto.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi!");
            RuleFor(ls => ls.CreateQuestionLessionDto.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
        }
    }
}
