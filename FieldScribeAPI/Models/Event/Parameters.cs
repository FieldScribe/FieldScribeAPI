using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class Parameters
    {
        [JsonProperty]
        public string MeasurementType { get; set; }

        [JsonProperty]
        public string EventType { get; set; }

        [JsonProperty]
        public float Precision { get; set; }

        [JsonProperty]
        public float Maximum { get; set; }
    }
}
