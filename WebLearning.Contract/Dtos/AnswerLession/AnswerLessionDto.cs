using WebLearning.Contract.Dtos.Question;

namespace WebLearning.Contract.Dtos.AnswerLession
{
    public class AnswerLessionDto
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public Guid QuizLessionId { get; set; }

        public string AccountName { get; set; }

        public string OwnAnswer { get; set; }

        public int CheckBoxId { get; set; }

        public bool Checked { get; set; }
        public virtual QuestionLessionDto Question { get; set; }
    }
}
