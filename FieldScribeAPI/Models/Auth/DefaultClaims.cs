using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public static class DefaultClaims
    {
        private static readonly string _scribe = "scribe";

        public static string Scribe => _scribe;
    }
}
