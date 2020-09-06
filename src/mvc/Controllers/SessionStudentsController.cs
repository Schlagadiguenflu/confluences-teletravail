using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Swan;

namespace mvc.Controllers
{
    public class SessionStudentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SessionStudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: SessionStudents
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la liste SessionTeachers
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SessionStudents/");
            List<SessionStudent> sessionStudents = JsonConvert.DeserializeObject<List<SessionStudent>>(content);

            return View(sessionStudents);
        }

        // GET: SessionStudents/Create
        public async Task<IActionResult> CreateAsync(int? id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la liste de session
            string contentSessions = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/");
            List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(contentSessions);
            if (id != null || id != 0)
            {
                sessions = sessions.Where(s => s.SessionId == id).ToList();
            }

            // Récupération de la liste de users
            string contentUsers = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(contentUsers);

            // Preparer les viewdata
            var selectListItemsSessions = sessions
                  .Select(s => new SelectListItem
                  {
                      Value = s.SessionId.ToString(),
                      Text = s.SchoolClassRoom.SchoolClassRoomName + " " + "session" + " " + s.SessionNumberId + " " + s.DateStart.Year.ToString()
                  });

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["SessionId"] = new SelectList(selectListItemsSessions, "Value", "Text");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View();
        }

        // POST: SessionStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,StudentId")] SessionStudent sessionStudent)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(sessionStudent.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/SessionStudents", httpContent);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ModelState.AddModelError(string.Empty, "Le-la participant-e existe déjà pour cette période.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'une sessionStudents, contacter l'administrateur.");
                }
                else
                {
                    // Récupération de la session
                    string contentSession = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + sessionStudent.SessionId);
                    Session session = JsonConvert.DeserializeObject<Session>(contentSession);

                    return RedirectToAction("Details", "SchoolClassRooms", new { id = session.SchoolClassRoomId });
                }
            }

            // Récupération de la liste de session
            string contentSessions = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/");
            List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(contentSessions);

            // Récupération de la liste de users
            string contentUsers = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(contentUsers);

            // Preparer les viewdata
            var selectListItemsSessions = sessions
                  .Select(s => new SelectListItem
                  {
                      Value = s.SessionId.ToString(),
                      Text = s.SchoolClassRoom.SchoolClassRoomName + " " + "session" + " " + s.SessionNumberId + " " + s.DateStart.Year.ToString()
                  });

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["SessionId"] = new SelectList(selectListItemsSessions, "Value", "Text");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(sessionStudent);
        }

        // GET: SessionStudents/Delete/5/asdf
        public async Task<IActionResult> Delete(int id, string id2)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération de la SessionTeacher
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SessionStudents/" + id + "/" + id2);
            SessionStudent sessionStudent = JsonConvert.DeserializeObject<SessionStudent>(content);


            if (sessionStudent == null)
            {
                return NotFound();
            }

            return View(sessionStudent);
        }

        // POST: SessionStudents/Delete/5/asdf
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string id2)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récupération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SessionStudents/" + id + "/" + id2);
            SessionStudent sessionStudent = JsonConvert.DeserializeObject<SessionStudent>(content);

            if (sessionStudent == null)
            {
                return NotFound();
            }

            // Récupération de la session
            string contentSession = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions/" + sessionStudent.SessionId);
            Session session = JsonConvert.DeserializeObject<Session>(contentSession);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/SessionStudents/" + id + "/" + id2);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Details", "SchoolClassRooms", new { id = session.SchoolClassRoomId });
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
