using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class ExamWithQuestionsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ChoiceDto> Choices { get; set; }
    }

    public class ChoiceDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
