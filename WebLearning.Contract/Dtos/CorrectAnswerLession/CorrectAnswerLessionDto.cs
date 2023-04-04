namespace WebLearning.Contract.Dtos.CorrectAnswerLession
{
    public class CorrectAnswerLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuestionLessionId { get; set; }

        public Guid OptionLessionId { get; set; }

        public string CorrectAnswer { get; set; }

        public bool Correct { get; set; }
    }
}
