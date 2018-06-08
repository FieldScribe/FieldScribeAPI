using FieldScribeAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class AssignToMeetForm
    {
        [Required]
        [Display(Name = "userId", Description = "ID of user (scribe)")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "meetId", Description = "ID of meet")]
        public int meetId { get; set; }
    }
}
