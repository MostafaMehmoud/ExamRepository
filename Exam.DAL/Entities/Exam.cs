using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Enumies;

namespace Exam.DAL.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ExamStatus Status { get; set; } = ExamStatus.Draft;
        public ICollection<Question> Questions { get; set; }
        public int DurationMinutes { get; set; }
    }

}
