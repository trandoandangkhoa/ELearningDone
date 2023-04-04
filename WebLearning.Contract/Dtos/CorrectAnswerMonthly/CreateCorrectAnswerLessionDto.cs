namespace WebLearning.Contract.Dtos.CorrectAnswerMonthly
{
    public class CreateCorrectAnswerMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuestionMonthlyId { get; set; }

        public Guid OptionMonthlyId { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
