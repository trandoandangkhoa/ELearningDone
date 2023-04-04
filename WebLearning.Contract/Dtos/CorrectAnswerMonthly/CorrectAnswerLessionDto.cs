namespace WebLearning.Contract.Dtos.CorrectAnswerMonthly
{
    public class CorrectAnswerMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuestionMonthlyId { get; set; }

        public Guid OptionMonthlyId { get; set; }

        public string CorrectAnswer { get; set; }

        public bool Correct { get; set; }
    }
}
