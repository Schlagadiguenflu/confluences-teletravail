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
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            List<HomeworkV2s> homeworks;
            try
            {
                homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/Ressource/"),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            homeworks = homeworks.OrderByDescending(h => h.HomeworkV2date).ToList();

            return View(homeworks);
        }
    }
}
