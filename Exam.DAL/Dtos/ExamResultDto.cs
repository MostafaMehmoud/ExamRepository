using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class ExamResultDto
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public double Score { get; set; }
        public bool IsPassed { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int PassingPercentage { get; set; } = 60;
    }
}
