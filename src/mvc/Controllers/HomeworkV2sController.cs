using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class HomeworkV2sController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeworkV2sController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: HomeworkV2s
        public async Task<IActionResult> Index(int? id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var access = await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/checkAccessToken");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (id != null)
            {
                homeworks = homeworks.Where(h => h.SessionId == id).ToList();

                Session session = await JsonSerializer.DeserializeAsync<Session>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions/" + id),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                ViewData["SchoolClassRoomId"] = session.SchoolClassRoomId;
                ViewData["SessionId"] = session.SessionId;
            }

            return View(homeworks);
        }

        [AllowAnonymous]
        // GET: HomeworkV2s/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["IsTeacher"] = false;
            if (User.IsAuthenticated())
            {
                if (User.Claims.Any(c => c.Value == "Teacher"))
                {
                    ViewData["IsTeacher"] = true;
                }
                else
                {
                    ViewData["IsStudent"] = true;
                }
            }
            else
            {
                return BadRequest();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HomeworkV2s homework = await JsonSerializer.DeserializeAsync<HomeworkV2s>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // GET: HomeworkV2s/Create
        public async Task<IActionResult> CreateAsync(int? sessionId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/HomeworkTypes");
            //List<HomeworkType> homeworkTypes = JsonSerializer.Deserialize<List<HomeworkType>>(content);
            List<HomeworkType> homeworkTypes = await JsonSerializer.DeserializeAsync<List<HomeworkType>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkTypes"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<Session> sessions = new List<Session>();

            if (sessionId != null)
            {
                Session session = await JsonSerializer.DeserializeAsync<Session>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions/" + sessionId),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

                sessions.Add(session);
            }
            else
            {
                sessions = await JsonSerializer.DeserializeAsync<List<Session>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );
            }

            // Preparer les viewdata
            var selectListItemsSessions = sessions
                  .Select(s => new SelectListItem
                  {
                      Value = s.SessionId.ToString(),
                      Text = s.SchoolClassRoom.SchoolClassRoomName + " " + "session" + " " + s.SessionNumberId + " " + s.DateStart.Year.ToString()
                  });

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            ViewData["HomeworkTypeId"] = new SelectList(homeworkTypes, "HomeworkTypeId", "HomeworkTypeName");
            ViewData["SessionId"] = new SelectList(selectListItemsSessions, "Value", "Text");
            ViewData["SessionIdReturn"] = sessions.First().SessionId;
            ViewData["TeacherId"] = user.Id;
            return View();
        }

        // POST: HomeworkV2s/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeworkV2id,HomeworkV2date,HomeworkV2name,HomeworkTypeId,SessionId,TeacherId,IsFutur")] HomeworkV2s homeworkV2s)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(homeworkV2s.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/HomeworkV2s", httpContent);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'un devoir, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    int id = 0;
                    try
                    {
                        id = int.Parse(response.Headers.Location.Segments[3]);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Erreur à la création d'un devoir (réception), contacter l'administrateur.");
                    }
                    if (id != 0)
                    {
                        return RedirectToAction(nameof(Details), new { id = id });
                    }
                    return RedirectToAction(nameof(Index), new { id = homeworkV2s.SessionId });
                }
            }

            //string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/HomeworkTypes");
            //List<HomeworkType> homeworkTypes = JsonSerializer.Deserialize<List<HomeworkType>>(content);
            List<HomeworkType> homeworkTypes = await JsonSerializer.DeserializeAsync<List<HomeworkType>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkTypes"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<Session> sessions = await JsonSerializer.DeserializeAsync<List<Session>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            ViewData["HomeworkTypeId"] = new SelectList(homeworkTypes, "HomeworkTypeId", "HomeworkTypeName");
            ViewData["SessionId"] = new SelectList(sessions, "SessionId", "SessionId");
            ViewData["TeacherId"] = user.Id;
            return View(homeworkV2s);
        }

        // GET: HomeworkV2s/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HomeworkV2s homework = await JsonSerializer.DeserializeAsync<HomeworkV2s>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (homework == null)
            {
                return NotFound();
            }

            //string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/HomeworkTypes");
            //List<HomeworkType> homeworkTypes = JsonSerializer.Deserialize<List<HomeworkType>>(content);
            List<HomeworkType> homeworkTypes = await JsonSerializer.DeserializeAsync<List<HomeworkType>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkTypes"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<Session> sessions = await JsonSerializer.DeserializeAsync<List<Session>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );
            // Preparer les viewdata
            var selectListItemsSessions = sessions
                  .Select(s => new SelectListItem
                  {
                      Value = s.SessionId.ToString(),
                      Text = s.SchoolClassRoom.SchoolClassRoomName + " " + "session" + " " + s.SessionNumberId + " " + s.DateStart.Year.ToString()
                  });

            List<AspNetUser> users = await JsonSerializer.DeserializeAsync<List<AspNetUser>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Role/Teachers"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );
            // Preparer les viewdata

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["HomeworkTypeId"] = new SelectList(homeworkTypes, "HomeworkTypeId", "HomeworkTypeName");
            ViewData["SessionId"] = new SelectList(selectListItemsSessions, "Value", "Text");
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            return View(homework);
        }

        // POST: HomeworkV2s/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HomeworkV2id,HomeworkV2date,HomeworkV2name,HomeworkTypeId,SessionId,TeacherId,IsFutur")] HomeworkV2s homework)
        {
            if (id != homework.HomeworkV2id)
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
                StringContent httpContent = new StringContent(homework.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'un devoir, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "HomeworkV2s", new { id = homework.SessionId });
                }
            }

            //string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/HomeworkTypes");
            //List<HomeworkType> homeworkTypes = JsonSerializer.Deserialize<List<HomeworkType>>(content);
            List<HomeworkType> homeworkTypes = await JsonSerializer.DeserializeAsync<List<HomeworkType>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkTypes"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<Session> sessions = await JsonSerializer.DeserializeAsync<List<Session>>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Sessions"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            //string userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;

            // Preparer les viewdata
            var selectListItemsSessions = sessions
                  .Select(s => new SelectListItem
                  {
                      Value = s.SessionId.ToString(),
                      Text = s.SchoolClassRoom.SchoolClassRoomName + " " + "session" + " " + s.SessionNumberId + " " + s.DateStart.Year.ToString()
                  });

            List<AspNetUser> users = await JsonSerializer.DeserializeAsync<List<AspNetUser>>(
              await client.GetStreamAsync(_configuration["URLAPI"] + "api/Role/Teachers"),
              new JsonSerializerOptions
              {
                  PropertyNameCaseInsensitive = true
              }
           );
            // Preparer les viewdata

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });


            ViewData["HomeworkTypeId"] = new SelectList(homeworkTypes, "HomeworkTypeId", "HomeworkTypeName");
            ViewData["SessionId"] = new SelectList(selectListItemsSessions, "Value", "Text");
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(homework);
        }

        // GET: HomeworkV2s/Delete/5
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

            HomeworkV2s homework = await JsonSerializer.DeserializeAsync<HomeworkV2s>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // POST: HomeworkV2s/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
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
