using FieldScribeAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class Event : Resource
    {
        public Link Meet { get; set; }

        public Link Events { get; set; }

        public Link Entries { get; set; }
        
        [Sortable]
        [SearchableInt]
        public int eventId { get; set; }

        [Sortable]
        [SearchableInt]
        public int meetId { get; set; }

        [Sortable (Default = true)]
        [SearchableInt]
        public int EventNum { get; set; }

        public int RoundNum { get; set; }

        public int FlightNum { get; set; }

        [Sortable]
        [Searchable]
        public string EventName { get; set; }

        public Parameters Params { get; set; }

        public BarHeight [] BarHeights { get; set; }
    }
}
