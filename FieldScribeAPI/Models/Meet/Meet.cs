using FieldScribeAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class Meet : Resource
    {
        public Link Athletes { get; set; }

        public Link Events { get; set; }

        // Not sure whether to use int or string for some of these values.
        [Sortable(Default = true)]
        [SearchableInt]
        public int meetId { get; set; }

        [Sortable]
        [Searchable]
        public string MeetName { get; set; }

        [Sortable]
        [Searchable]
        public DateTime MeetDate { get; set; }

        [Sortable]
        [Searchable]
        public string MeetLocation { get; set; }

        [Sortable]
        [Searchable]
        public string MeasurementType { get; set; }
    }
}
