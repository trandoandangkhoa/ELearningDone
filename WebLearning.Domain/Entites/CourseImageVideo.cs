using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class CourseImageVideo
    {
        public Guid Id { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateCreated { get; set; }
        public long FileSize { get; set; }
        public Course Course { get; set; }

    }
}
