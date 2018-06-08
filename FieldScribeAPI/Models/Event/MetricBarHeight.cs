using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MetricBarHeight
    {
        [Required]
        [Display(Name = "HeightNum", Description = "Sequence of bar height")]
        public int HeightNum { get; set; }

        [Required]
        [Display(Name = "Meters", Description = "Height in meters")]
        public float Meters { get; set; }
    }
}
