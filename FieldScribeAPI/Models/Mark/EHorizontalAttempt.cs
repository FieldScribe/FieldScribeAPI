using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EHorizontalAttempt
    {
        [Required]
        [Display(Name = "AttemptNum", Description = "Sequence of attempt")]
        public int AttemptNum { get; set; }

        [Required]
        [Display(Name = "Feet", Description = "Distance in feet")]
        public int Feet { get; set; }

        [Required]
        [Display(Name = "Inches", Description = "Inches component of distance")]
        public float Inches { get; set; }

        [Display(Name = "Wind", Description = "Wind reading, if applicable")]
        public float? Wind { get; set; }
    }
}
