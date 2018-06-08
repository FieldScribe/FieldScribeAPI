using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EventKey
    {
        public string EventName { get; set; }

        public int RoundNum { get; set; }

        public int FlightNum { get; set; }

        public override int GetHashCode()
        {
            return EventName.GetHashCode() + RoundNum + FlightNum;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EventKey);
        }

        public bool Equals(EventKey obj)
        {
            return obj.EventName == EventName
                && obj.RoundNum == RoundNum
                && obj.FlightNum == FlightNum;
        }
    }
}
