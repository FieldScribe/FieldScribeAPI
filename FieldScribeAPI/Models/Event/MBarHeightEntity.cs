using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MBarHeightEntity
    {
        [Key]
        public int MHeightId { get; set; }

        public int eventId { get; set; }

        public int HeightNum { get; set; }

        public float Meters { get; set; }
    }
}
