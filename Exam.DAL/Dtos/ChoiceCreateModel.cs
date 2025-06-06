﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Dtos
{
    public class ChoiceCreateModel
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
