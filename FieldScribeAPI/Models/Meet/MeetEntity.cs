using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MeetEntity
    {
        [Key]
        public int meetId { get; set; }

        public string MeetName { get; set; }

        public DateTime MeetDate { get; set; }

        public string MeetLocation { get; set; }

        public string MeasurementType { get; set; }
    }
}
