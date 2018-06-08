using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class LynxEventFileForm
    {
        [Required]
        [Display(Name = "meetId", Description = "The meet's assigned ID number")]
        public int meetId { get; set; }

        [Required]
        [Display(Name = "schFileText", Description = "Contents of .sch file")]
        public string SchFileText { get; set; }

        [Required]
        [Display(Name = "evtFileText", Description = "Contents of .evt file")]
        public string EvtFileText { get; set; }
    }
}
