namespace WebLearning.Contract.Dtos.AnswerLession
{
    public class UpdateAnswerLessionDto
    {

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }
    }
}
