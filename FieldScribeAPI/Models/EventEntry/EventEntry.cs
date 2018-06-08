using FieldScribeAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventEntry : Resource
    {
        public Link Event { get; set; }

        public Link AthleteEvents { get; set; }

        [Sortable]
        [Searchable]
        public int entryId { get; set; }

        [Sortable]
        [Searchable]
        public int eventId { get; set; }

        [Sortable]
        [Searchable]
        public int athleteId { get; set; }

        [Sortable]
        [Searchable]
        public int CompNum { get; set; }

        [Sortable (Default = true)]
        [Searchable]
        public int CompetePosition { get; set; }

        [Sortable]
        [Searchable]
        public string LastName { get; set; }

        [Sortable]
        [Searchable]
        public string FirstName { get; set; }

        [Sortable]
        [Searchable]
        public string TeamName { get; set; }

        [Sortable]
        [Searchable]
        public string MarkType { get; set; }

        public Attempt[] Marks { get; set; }
    }
}
