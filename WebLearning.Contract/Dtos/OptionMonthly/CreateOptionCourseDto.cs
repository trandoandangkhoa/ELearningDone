namespace WebLearning.Contract.Dtos.OptionMonthly
{
    public class CreateOptionMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuestionMonthlyId { get; set; }

        public string Name { get; set; }
    }
}
