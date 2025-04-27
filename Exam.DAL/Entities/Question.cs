using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        [Range(1, 10)]
        public int Points { get; set; } = 1; // Default to 1 point
        public ICollection<Choice> Choices { get; set; }
    }

}
