using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class AthletesResponse : PagedCollection<Athlete>
    {
        public Form AthletesQuery { get; set; }
    }
}
