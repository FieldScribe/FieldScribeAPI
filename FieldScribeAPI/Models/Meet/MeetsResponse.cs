using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MeetsResponse : PagedCollection<Meet>
    {
        public Form MeetsQuery { get; set; }
    }
}
