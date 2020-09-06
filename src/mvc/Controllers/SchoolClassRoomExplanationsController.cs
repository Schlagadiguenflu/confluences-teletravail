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
    public class SchoolClassRoomExplanationsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SchoolClassRoomExplanationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: SchoolClassRoomExplanations
        public async Task<IActionResult> Index(int? id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";

            if (id != null)
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations?schoolclassroomid=" + id);
                ViewData["SchoolClassRoomId"] = (int)id;
            }
            else
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/");
                ViewData["SchoolClassRoomId"] = (int)0;
            }
            List<SchoolClassRoomExplanation> schoolClassRoomExplanations = JsonConvert.DeserializeObject<List<SchoolClassRoomExplanation>>(content);

            return View(schoolClassRoomExplanations);
        }

        // GET: SchoolClassRoomExplanations/Details/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/" + id);
            SchoolClassRoomExplanation schoolClassRoomExplanation = JsonConvert.DeserializeObject<SchoolClassRoomExplanation>(content);

            if (schoolClassRoomExplanation == null)
            {
                return NotFound();
            }

            return View(schoolClassRoomExplanation);
        }

        // GET: SchoolClassRoomExplanations/Create
        public async Task<IActionResult> CreateAsync(int? id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";
            List<SchoolClassRoom> schoolClassRooms = new List<SchoolClassRoom>();
            if (id == null)
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms");
                schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);
                ViewData["SchoolClassRoomIdReturn"] = (int)0;
            }
            else
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id);
                SchoolClassRoom schoolClassRoom = JsonConvert.DeserializeObject<SchoolClassRoom>(content);
                schoolClassRooms.Add(schoolClassRoom);
                ViewData["SchoolClassRoomIdReturn"] = (int)id;
            }

            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName");
            return View();
        }

        // POST: SchoolClassRoomExplanations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolClassRoomExplanationId,LanguageName,LanguageImage,AudioLink,SchoolClassRoomId")] SchoolClassRoomExplanation schoolClassRoomExplanation)
        {
            ViewData["SchoolClassRoomId"] = schoolClassRoomExplanation.SchoolClassRoomId;
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(schoolClassRoomExplanation.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations", httpContent);

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
                    return RedirectToAction("Index", "SchoolClassRoomExplanations", new { id = schoolClassRoomExplanation.SchoolClassRoomId });
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);

            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", schoolClassRoomExplanation.SchoolClassRoomId);
            return View(schoolClassRoomExplanation);
        }

        // GET: SchoolClassRoomExplanations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (id == null)
            {
                return NotFound();
            }


            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/" + id);
            SchoolClassRoomExplanation schoolClassRoomExplanation = JsonConvert.DeserializeObject<SchoolClassRoomExplanation>(content);

            if (schoolClassRoomExplanation == null)
            {
                return NotFound();
            }
            ViewData["SchoolClassRoomIdReturn"] = schoolClassRoomExplanation.SchoolClassRoomId;

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);

            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", schoolClassRoomExplanation.SchoolClassRoomId);

            return View(schoolClassRoomExplanation);
        }

        // POST: SchoolClassRoomExplanations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SchoolClassRoomExplanationId,LanguageName,LanguageImage,AudioLink,SchoolClassRoomId")] SchoolClassRoomExplanation schoolClassRoomExplanation)
        {
            if (id != schoolClassRoomExplanation.SchoolClassRoomExplanationId)
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
                StringContent httpContent = new StringContent(schoolClassRoomExplanation.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/" + id, httpContent);
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
                    return RedirectToAction("Index", "SchoolClassRoomExplanations", new { id = schoolClassRoomExplanation.SchoolClassRoomId });
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations");
            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);

            ViewData["SchoolClassRoomId"] = new SelectList(schoolClassRooms, "SchoolClassRoomId", "SchoolClassRoomName", schoolClassRoomExplanation.SchoolClassRoomId);

            return View(schoolClassRoomExplanation);
        }

        // GET: SchoolClassRoomExplanations/Delete/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/" + id);
            SchoolClassRoomExplanation schoolClassRoomExplanation = JsonConvert.DeserializeObject<SchoolClassRoomExplanation>(content);

            if (schoolClassRoomExplanation == null)
            {
                return NotFound();
            }

            return View(schoolClassRoomExplanation);
        }

        // POST: SchoolClassRoomExplanations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/SchoolClassRoomExplanations/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "SchoolClassRoomExplanations");
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
