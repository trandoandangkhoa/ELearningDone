﻿using WebLearning.Contract.Dtos.Quiz;

namespace WebLearning.Contract.Dtos.ReportScore
{
    public class ReportScoreMonthlyDto
    {
        public Guid Id { get; set; }

        public Guid QuizMonthlyId { get; set; }

        public Guid RoleId { get; set; }


        public string UserName { get; set; }

        public string FullName { get; set; }


        public DateTime CompletedDate { get; set; }

        public int TotalScore { get; set; }

        public bool Passed { get; set; }
        public string IpAddress { get; set; }

        public virtual ICollection<QuizMonthlyDto> QuizMonthlyDtos { get; set; }
    }
}
