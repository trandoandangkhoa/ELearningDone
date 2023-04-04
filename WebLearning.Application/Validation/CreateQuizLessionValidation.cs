using FluentValidation;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Application.Validation
{
    public class CreateQuizLessionValidation : AbstractValidator<CreateQuizLessionDto>
    {
        public CreateQuizLessionValidation()
        {
            RuleFor(ls => ls.Name).NotEmpty().WithMessage("Vui lòng điền nội dung câu hỏi!");
            RuleFor(ls => ls.ScorePass).InclusiveBetween(1, 1000).WithMessage("Điểm câu hỏi phải từ 1 - 1000")
                                                    .NotNull().NotEmpty().WithMessage("Vui lòng điền số điểm!");

            RuleFor(ls => ls.LessionId).NotNull().NotEmpty().WithMessage("Vui lòng chọn bài học kiểm tra!");

            RuleFor(ls => ls.TimeToDo).NotEmpty().WithMessage("Vui lòng điền thời gian hoàn thành!");

            RuleFor(ls => ls.Description).NotEmpty().WithMessage("Vui lòng điền nội dung bài kiểm tra!");
        }
    }
}
