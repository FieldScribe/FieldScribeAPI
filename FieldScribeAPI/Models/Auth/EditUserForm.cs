using FieldScribeAPI.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FieldScribeAPI.Models
{
    public class EditUserForm
    {
        [Required]
        [Display(Name = "userId", Description = "ID of user")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "email", Description = "Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        [Display(Name = "firstName", Description = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        [Display(Name = "lastName", Description = "Last name")]
        public string LastName { get; set; }
    }
}
