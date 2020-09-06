using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.ViewModels
{
    public class ForgotPasswordInput
    {
        [Required]
        public string UserName { get; set; }
    }
}
