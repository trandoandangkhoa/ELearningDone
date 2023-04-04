namespace WebLearning.Contract.Dtos.AnswerMonthly
{
    public class UpdateAnswerMonthlyDto
    {
        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }
    }
}
