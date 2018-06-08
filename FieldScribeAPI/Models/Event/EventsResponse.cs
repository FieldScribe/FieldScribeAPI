using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventsResponse : PagedCollection<Event>
    {
        public Form EventsQuery { get; set; }
    }
}
