using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.ViewModels
{
    public class SMSVerification
    {
        [Phone]
        public string PhoneNumber { get; set; }

        [BindProperty, Required, Display(Name = "Code")] 
        public string VerificationCode { get; set; }
    }
}
