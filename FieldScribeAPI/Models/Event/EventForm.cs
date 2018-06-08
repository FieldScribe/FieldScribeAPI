using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FieldScribeAPI.Models
{
    public class EventForm
    {
        [Required]
        [Display(Name = "meetId", Description = "The unique ID of the meet in which the event is taking place")]
        public int meetId { get; set; }

        [Required]
        [Display(Name = "eventNum", Description = "The event's unique identifier")]
        public int EventNum { get; set; }

        [Required]
        [Display(Name = "roundNum", Description = "The round number of the event")]
        public int RoundNum { get; set; }

        [Required]
        [Display(Name = "flightNum", Description = "The flight number of the event")]
        public int FlightNum { get; set; }

        [Required]
        [Display(Name = "eventName", Description = "The name of the event")]
        public string EventName { get; set; }
    }
}
