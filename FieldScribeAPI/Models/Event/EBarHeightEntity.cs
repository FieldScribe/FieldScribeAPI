using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EBarHeightEntity
    {
        [Key]
        public int EHeightId { get; set; }

        public int eventId { get; set; }

        public int HeightNum { get; set; }

        public int Feet { get; set; }

        public float Inches { get; set; }
    }
}
