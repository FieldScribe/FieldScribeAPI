using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class VerticalAttempt
    {
        [Required]
        [Display(Name = "AttemptNum", Description = "Sequence of attempt")]
        public int AttemptNum { get; set; }

        [Required]
        [Display(Name = "Mark", Description = "Mark: X, O, P, or blank")]
        public string Mark { get; set; }
    }
}
