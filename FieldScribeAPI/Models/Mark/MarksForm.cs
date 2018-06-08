using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class MarksForm<T>
    {
        [Required]
        [Display(Name = "eventId", Description = "Id of event")]
        public int eventId { get; set; }

        [Required]
        [Display(Name = "entryId", Description = "Id of entry")]
        public int entryId { get; set; }

        [Required]
        [Display(Name = "Marks", Description = "Array of marks")]
        public T[] Marks { get; set; }
    }
}
