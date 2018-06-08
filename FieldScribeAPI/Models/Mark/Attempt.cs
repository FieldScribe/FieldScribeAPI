using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class Attempt
    {
        public int AttemptNum { get; set; }

        public string Mark { get; set; }

        public float? Wind { get; set; }
    }
}
