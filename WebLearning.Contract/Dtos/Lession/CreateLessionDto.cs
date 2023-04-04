namespace WebLearning.Contract.Dtos.Lession
{
    public class CreateLessionDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string Alias { get; set; }
        public string Author { get; set; }

        public bool Notify { get; set; }
        public string CodeCourse { get; set; }
        public string DescNotify { get; set; }

    }
}
