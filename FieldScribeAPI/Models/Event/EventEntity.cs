using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventEntity
    {
        [Key]
        public int eventId { get; set; }

        public int meetId { get; set; }

        public int EventNum { get; set; }

        public int RoundNum { get; set; }

        public int FlightNum { get; set; }

        public string EventName { get; set; }
    }
}
