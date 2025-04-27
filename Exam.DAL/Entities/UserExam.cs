using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Entities
{
    public class UserExam
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime TakenAt { get; set; }
    }

}
