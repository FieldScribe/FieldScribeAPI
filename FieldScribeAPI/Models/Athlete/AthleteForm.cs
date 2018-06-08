using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class AthleteForm
    {
        [Required]
        [Display(Name = "meetId", Description = "The meet's assigned ID number")]
        public int meetId { get; set; }

        [Required]
        [Display(Name = "compNum", Description = "The athlete's assigned number")]
        public int CompNum { get; set; }

        [Required]
        [Display(Name = "firstName", Description = "The athlete's first name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "lastName", Description = "The athlete's last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "teamName", Description = "The name of the team the athlete is on")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "gender", Description = "The gender of the athlete")]
        public string Gender { get; set; }
    }
}
