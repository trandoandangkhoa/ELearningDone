namespace WebLearning.Contract.Dtos.Quiz
{
    public class CreateQuizMonthlyDto
    {

        public Guid ID { get; set; }

        public Guid RoleId { get; set; }
        public string Code { get; set; }
        public string CodeRole { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public int TimeToDo { get; set; }

        public int ScorePass { get; set; }

        public bool Passed { get; set; }

        public string Alias { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }
        public Guid CreatedBy { get; set; }


    }
}
