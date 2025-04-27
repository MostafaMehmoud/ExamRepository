using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class QuestionUpdateModel
    {
        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Title { get; set; }

        [Range(1, 10)]
        public int Points { get; set; }
    }
}
