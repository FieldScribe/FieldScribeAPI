using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class LynxAthleteFileForm
    {
        [Required]
        [Display(Name = "meetId", Description = "The meet's assigned ID number")]
        public int meetId { get; set; }

        [Required]
        [Display(Name = "fileText", Description = "Contents of .ppl file")]
        public string FileText { get; set; }
    }
}
