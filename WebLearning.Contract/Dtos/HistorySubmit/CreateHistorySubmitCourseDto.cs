namespace WebLearning.Contract.Dtos.HistorySubmit
{
    public class CreateHistorySubmitCourseDto
    {
        public Guid Id { get; set; }

        public Guid QuizCourseId { get; set; }

        public string AccountName { get; set; }

        public DateTime DateCompoleted { get; set; }

        public bool Submit { get; set; }

        public int TotalScore { get; set; }

    }
}
