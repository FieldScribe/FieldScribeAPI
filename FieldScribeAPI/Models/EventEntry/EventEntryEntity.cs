using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventEntryEntity
    {
        [Key]
        public int entryId { get; set; }

        public int eventId { get; set; }

        public int athleteId { get; set; }

        public int CompNum { get; set; }

        public int CompetePosition { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string TeamName { get; set; }
    }
}
