using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class AthleteEntity
    {
        [Key]
        public int athleteId { get; set; }

        public int meetId { get; set; }

        public int CompNum { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TeamName { get; set; }

        public string Gender { get; set; }
    }
}
