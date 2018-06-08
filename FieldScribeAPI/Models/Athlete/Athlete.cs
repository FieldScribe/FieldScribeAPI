using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldScribeAPI.Infrastructure;

namespace FieldScribeAPI.Models
{
    public class Athlete : Resource
    {
        public Link Meet { get; set; }

        public Link AthleteEvents { get; set; }

        // Not sure whether to use int or string for some of these values.
        [Sortable(Default = true)]
        [SearchableInt]
        public int athleteId { get; set; }

        [Sortable]
        [SearchableInt]
        public int meetId { get; set; }

        [Sortable]
        [SearchableInt]
        public int CompNum { get; set; }

        [Sortable]
        [Searchable]
        public string FirstName { get; set; }

        [Sortable]
        [Searchable]
        public string LastName { get; set; }

        [Sortable]
        [Searchable]
        public string TeamName { get; set; }

        [Sortable]
        [Searchable]
        public string Gender { get; set; }
    }
}
