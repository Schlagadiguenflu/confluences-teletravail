using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mvc.Models;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration _configuration { get; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (User.IsAuthenticated())
            {
                if (User.Claims.Any(c => c.Value == "Teacher"))
                {
                    ViewData["IsStudent"] = false;
                }
                else
                {
                    ViewData["IsStudent"] = true;
                }
            }

            // Préparation de l'appel à l'API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");

            // Récupération de la liste de teachers
            //string contentUsers = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Teachers");
            //List<AspNetUser> users = JsonSerializer.Deserialize<List<AspNetUser>>(contentUsers);
            List<AspNetUser> users = await JsonSerializer.DeserializeAsync<List<AspNetUser>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Role/TeachersHome"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            ViewData["URLAPI"] = _configuration["URLAPI"];
            return View(users);
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
