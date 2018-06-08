using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class BarHeightsForm<T>
    {
        [Required]
        [Display(Name = "eventId", Description = "Id of event")]
        public int eventId { get; set; }

        [Required]
        [Display(Name = "Heights", Description = "Array of heights")]
        public T[] Heights { get; set; }
    }
}
