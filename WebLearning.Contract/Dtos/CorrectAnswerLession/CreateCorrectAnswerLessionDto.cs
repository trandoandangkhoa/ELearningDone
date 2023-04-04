namespace WebLearning.Contract.Dtos.CorrectAnswerLession
{
    public class CreateCorrectAnswerLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuestionLessionId { get; set; }

        public Guid OptionLessionId { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
