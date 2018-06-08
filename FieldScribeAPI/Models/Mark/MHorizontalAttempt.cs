using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MHorizontalAttempt
    {
        [Required]
        [Display(Name = "AttemptNum", Description = "Sequence of attempt")]
        public int AttemptNum { get; set; }

        [Required]
        [Display(Name = "Meters", Description = "Distance in meters")]
        public float Meters { get; set; }

        [Display(Name = "Wind", Description = "Wind reading, if applicable")]
        public float? Wind { get; set; }
    }
}
