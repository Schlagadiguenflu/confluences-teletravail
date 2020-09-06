using IdentityServerAspNetIdentity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.ViewModels
{
    public class RegisterInput : ApplicationUser
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string CountryCode { get; set; }
    }
}
