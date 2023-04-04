using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.ReportScore
{
    public class ReportScoreLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuizLessionId { get; set; }

        public Guid LessionId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }

        public string IpAddress { get; set; }
        public virtual ICollection<QuizlessionDto> QuizlessionDtos { get; set; }
        public virtual ICollection<LessionDto> LessionDtos { get; set; }
    }
}
