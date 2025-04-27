using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class ExamSubmissionDto
    {
        public int ExamId { get; set; }
        public string UserId { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }
    }
}
