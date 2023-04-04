namespace WebLearning.Contract.Dtos.Certificate
{
    public class CertificateMonthly
    {
        public string NameQuiz { get; set; }

        public string FullName { get; set; }

        public string RoleName { get; set; }

        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }

    }
}
