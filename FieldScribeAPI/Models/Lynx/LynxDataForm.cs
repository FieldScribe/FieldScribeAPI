using System.ComponentModel.DataAnnotations;

namespace FieldScribeAPI.Models
{
    public class LynxDataForm
    {
        [Required]
        [Display(Name = "meetId", Description = "The meet's assigned ID number")]
        public int meetId { get; set; }

        [Required]
        [Display(Name = "pplFileText", Description = "Contents of .ppl file")]
        public string PplFileText { get; set; }

        [Required]
        [Display(Name = "schFileText", Description = "Contents of .sch file")]
        public string SchFileText { get; set; }

        [Required]
        [Display(Name = "evtFileText", Description = "Contents of .evt file")]
        public string EvtFileText { get; set; }
    }
}
