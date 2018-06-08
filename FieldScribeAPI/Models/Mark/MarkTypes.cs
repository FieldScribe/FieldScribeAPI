using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public static class MarkTypes
    {
        private const string _vertical = "Vertical";
        private const string _english = "English";
        private const string _metric = "Metric";

        public static string Vertical => _vertical;
        public static string English => _english;
        public static string Metric => _metric;
    }
}
