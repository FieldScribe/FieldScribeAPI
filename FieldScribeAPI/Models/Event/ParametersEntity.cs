using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FieldScribeAPI.Models
{
    public class ParametersEntity
    {
        [Key]
        public int eventId { get; set; }

        public string MeasurementType { get; set; }

        public string EventType { get; set; }

        public float Precision { get; set; }

        public float Maximum { get; set; }
    }
}
