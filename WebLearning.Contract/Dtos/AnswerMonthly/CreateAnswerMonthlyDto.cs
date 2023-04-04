namespace WebLearning.Contract.Dtos.AnswerMonthly
{
    public class CreateAnswerMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuestionMonthlyId { get; set; }

        public Guid QuizMonthlyId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }

    }
}
