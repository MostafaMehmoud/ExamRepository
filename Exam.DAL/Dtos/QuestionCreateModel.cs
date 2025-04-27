using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class QuestionCreateModel
    {
        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Range(1, 10)]
        public int Points { get; set; } = 1;

        [Required]
        [MinLength(2, ErrorMessage = "يجب إضافة خيارين على الأقل")]
        [MaxLength(4, ErrorMessage = "لا يمكن إضافة أكثر من 4 خيارات")]
        public List<ChoiceCreateModel> Choices { get; set; }
    }
}
