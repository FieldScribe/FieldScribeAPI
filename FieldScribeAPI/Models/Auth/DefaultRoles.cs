using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class DefaultRoles
    {
        private static readonly string _scribe = "Scribe";
        private static readonly string _timer = "Timer";
        private static readonly string _admin = "Admin";

        public static string Scribe => _scribe;

        public static string Timer => _timer;

        public static string Admin => _admin;
    }
}
