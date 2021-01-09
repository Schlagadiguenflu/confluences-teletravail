using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class RessourceController : Controller
    {
        private readonly IConfiguration _configuration;

        public RessourceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Ressource about homeworks
        public async Task<IActionResult> Homeworks()
        {
            return View();
        }
    }
}
