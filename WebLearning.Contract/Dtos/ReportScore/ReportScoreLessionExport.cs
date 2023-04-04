namespace WebLearning.Contract.Dtos.ReportScore
{
    public class ReportScoreLessionExport
    {
        public Guid Id { get; set; }

        public Guid LessionId { get; set; }

        public Guid QuizLessionId { get; set; }
        public string LessionName { get; set; }

        public string QuizName { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }

        public string IpAddress { get; set; }
    }
}
