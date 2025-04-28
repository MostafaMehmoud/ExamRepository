using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class ExamCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1, 240)]
        public int DurationMinutes { get; set; } = 30;

        public List<QuestionCreateDto> Questions { get; set; } = new();
       

    }
    public class QuestionCreateDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Range(1, 10)]
        public int Points { get; set; } = 1;

        public List<ChoiceCreateDto> Choices { get; set; } = new();
    }
    public class ChoiceCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
