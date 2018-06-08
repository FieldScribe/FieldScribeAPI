using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EnglishBarHeight
    {
        [Required]
        [Display(Name = "HeightNum", Description = "Sequence of bar height")]
        public int HeightNum { get; set; }

        [Required]
        [Display(Name = "Feet", Description = "Height in feet")]
        public int Feet { get; set; }

        [Required]
        [Display(Name = "Inches", Description = "Inches component of height")]
        public float Inches { get; set; }
    }
}
