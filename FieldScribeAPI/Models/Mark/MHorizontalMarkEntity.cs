using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MHorizontalMarkEntity
    {
        [Key]
        public int MHorizontalId { get; set; }

        public int entryId { get; set; }

        public int AttemptNum { get; set; }

        public float Meters { get; set; }

        public float? Wind { get; set; }
    }
}
