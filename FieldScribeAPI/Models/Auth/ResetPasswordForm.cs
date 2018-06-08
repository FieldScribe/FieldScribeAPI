using FieldScribeAPI.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FieldScribeAPI.Models
{
    public class ResetPasswordForm
    {
        [Required]
        [Display(Name = "userId", Description = "ID of user (scribe)")]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [Display(Name = "newPassword", Description = "New Password")]
        [Secret]
        public string NewPassword { get; set; }
    }
}
