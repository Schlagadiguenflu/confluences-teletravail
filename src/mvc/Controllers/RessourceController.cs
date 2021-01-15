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
using Microsoft.Extensions.Logging;
using mvc.Models;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class RessourceController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private string Message { get; set; }

        public RessourceController(IConfiguration configuration, ILogger<RessourceController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: Ressource about homeworks
        public async Task<IActionResult> Homeworks()
        {
            ViewData["URLAPI"] = _configuration["URLAPI"];

            Message = $"About page 'Ressources' visited at {DateTime.UtcNow.ToLongTimeString()}";
            _logger.LogInformation(Message);
            return View();
        }
    }
}
