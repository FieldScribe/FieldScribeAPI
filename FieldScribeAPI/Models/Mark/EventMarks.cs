using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventMarks
    {
        public string MarkType { get; set; }

        public Attempt[] Attempts { get; set; }
    }
}
