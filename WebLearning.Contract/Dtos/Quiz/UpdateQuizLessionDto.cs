namespace WebLearning.Contract.Dtos.Quiz
{
    public class UpdateQuizLessionDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public int TimeToDo { get; set; }

        public int ScorePass { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortItem { get; set; }

        public string Alias { get; set; }

        public bool Notify { get; set; }

        public string DescNotify { get; set; }

    }
}
