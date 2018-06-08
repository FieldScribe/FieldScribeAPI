using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MeetForm
    {

        [Required]
        [Display(Name = "meetName", Description = "The name of the meet")]
        public string MeetName { get; set; }

        [Required]
        [Display(Name = "meetDate", Description = "The date of the event")]
        public DateTime MeetDate { get; set; }

        [Required]
        [Display(Name = "meetLocation", Description = "The name of the place the meet is held")]
        public string MeetLocation { get; set; }

        [Required]
        [Display(Name = "measurementType", Description = "The measurement type, metric false, american true")]
        public string MeasurementType { get; set; }
    }
}
