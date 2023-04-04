using FluentValidation;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;

namespace WebLearning.Application.Validation
{
    public class CreateQuestionMonthlyValidation : AbstractValidator<CreateAllConcerningQuestionMonthlyDto>
    {
        public CreateQuestionMonthlyValidation()
        {
            RuleFor(ls => ls.CreateQuestionMonthlyDto.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi!");
            RuleFor(ls => ls.CreateQuestionMonthlyDto.Point).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");
        }
    }
}
