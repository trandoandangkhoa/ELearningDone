namespace WebLearning.Contract.Dtos.Question
{
    public class UpdateQuestionLessionDto
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Alias { get; set; }
        public int Point { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
