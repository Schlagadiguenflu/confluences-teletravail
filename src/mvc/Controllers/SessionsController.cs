using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class SessionsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SessionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la session
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/");
            List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(content);

            return View(sessions);
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la session
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + id);
            Session session = JsonConvert.DeserializeObject<Session>(content);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public async Task<IActionResult> CreateAsync(int? schoolClassRoomId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la classe
            string content = "";
            List<SchoolClassRoom> schoolClassRooms = new List<SchoolClassRoom>();
            if (schoolClassRoomId == null)
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/");
                schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);
            }
            else
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + schoolClassRoomId);
                SchoolClassRoom schoolClassRoom = JsonConvert.DeserializeObject<SchoolClassRoom>(content);
                schoolClassRooms.Add(schoolClassRoom);
            }

            List<SessionNumber> sessionNumbers = await System.Text.Json.JsonSerializer.DeserializeAsync<List<SessionNumber>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/SessionNumbers/"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            List<int> years = new List<int>();
            years.Add(DateTime.Now.Year);
            years.Add(DateTime.Now.AddYears(1).Year);

            ViewData["nbClass"] = schoolClassRooms.Count();
            ViewData["SchoolClassRoom"] = schoolClassRooms.First().SchoolClassRoomId;
            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName");
            ViewData["SessionNumberId"] = new SelectList(sessionNumbers, "SessionNumberId", "SessionNumberId");
            ViewData["Years"] = new SelectList(years);
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,DateStart,DateEnd,SchoolClassRoomId,SessionNumberId")] Session session, int year)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            session.DateStart = new DateTime(year, session.SessionNumberId, 1);
            session.DateEnd = new DateTime(year, session.SessionNumberId + 1, 1);

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(session.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Sessions", httpContent);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction(nameof(Details), "SchoolClassRooms", new { id = session.SchoolClassRoomId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'une session, contacter l'administrateur.");
                }
            }

            // Récupération de la liste de classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);

            List<SessionNumber> sessionNumbers = await System.Text.Json.JsonSerializer.DeserializeAsync<List<SessionNumber>>(
              await client.GetStreamAsync(_configuration["URLAPI"] + "api/SessionNumbers/"),
              new JsonSerializerOptions
              {
                  PropertyNameCaseInsensitive = true
              }
           );

            List<int> years = new List<int>();
            years.Add(DateTime.Now.Year);
            years.Add(DateTime.Now.AddYears(1).Year);

            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", session.SchoolClassRoomId);
            ViewData["nbClass"] = 0;
            ViewData["SessionNumberId"] = new SelectList(sessionNumbers, "SessionNumberId", "SessionNumberId");
            ViewData["Years"] = new SelectList(years);
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id, int? schoolClassRoomId)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + id);
            Session session = JsonConvert.DeserializeObject<Session>(content);

            if (session == null)
            {
                return NotFound();
            }

            // Récupération de la liste de classe
            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);
            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", session.SchoolClassRoomId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SessionId,DateStart,DateEnd,SchoolClassRoomId")] Session session)
        {
            if (id != session.SessionId)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(session.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Sessions/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'une session, contacter l'administrateur.");
                    return View(session);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    return RedirectToAction(nameof(Details), "SchoolClassRooms", new { id = session.SchoolClassRoomId });
                }
            }

            // Récupération de la liste de classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);
            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", session.SchoolClassRoomId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + id);
            Session session = JsonConvert.DeserializeObject<Session>(content);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + id);
            Session session = JsonConvert.DeserializeObject<Session>(content);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/Sessions/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Details), "SchoolClassRooms", new { id = session.SchoolClassRoomId });
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
