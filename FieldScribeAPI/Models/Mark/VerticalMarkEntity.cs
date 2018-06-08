using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class VerticalMarkEntity
    {
        [Key]
        public int VerticalId { get; set; }

        public int entryId { get; set; }
        
        public int AttemptNum { get; set; }

        public string Marks { get; set; }

    }
}
