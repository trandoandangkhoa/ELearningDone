namespace WebLearning.Contract.Dtos.OptionLession
{
    public class CreateOptionLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuestionLessionId { get; set; }

        public string Name { get; set; }

    }
}
