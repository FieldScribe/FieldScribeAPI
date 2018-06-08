using FieldScribeAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class User : Resource
    {
        public Guid Id { get; set; }

        [Sortable]
        [Searchable]
        public string Email { get; set; }

        [Sortable]
        [Searchable]
        public string FirstName { get; set; }

        [Sortable]
        [Searchable]
        public string LastName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [Sortable]
        [Searchable]
        public IList<string> Roles { get; set; }
    }
}
