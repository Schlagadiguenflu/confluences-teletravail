using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Teacher")]
    public class ServicesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new List<string> {
                "value1",
                "value2"
             };
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
