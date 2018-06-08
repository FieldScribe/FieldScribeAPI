using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class LynxEvent
    {
        public int EventNum { get; set; }

        public EventKey Key { get; set; }

        public List<LynxEntry> Entries { get; set; }
    }
}
