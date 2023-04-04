namespace WebLearning.Contract.Dtos.Course
{
    public class UpdateCourseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public string Alias { get; set; }
        public bool Notify { get; set; }

        public int TotalWatched { get; set; }

        public string DescNotify { get; set; }
    }
}
