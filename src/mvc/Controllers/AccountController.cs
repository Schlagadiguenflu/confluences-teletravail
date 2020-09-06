using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace mvc.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult SignIn()
        {
            if (User.IsAuthenticated())
            {
                if (User.Claims.Any(c => c.Value == "Teacher"))
                {
                    return RedirectToAction("Index", "SchoolClassRooms");
                }

                return RedirectToAction("Index", "StudentPage");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}