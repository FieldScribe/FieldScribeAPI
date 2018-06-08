using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EntryEntity
    {
        [Key]
        public int entryId { get; set; }

        public int eventId { get; set; }

        public int athleteId { get; set; }

        public int CompetePosition { get; set; }

        public int FlightPlace { get; set; }

        public int EventPlace { get; set; }
    }
}
