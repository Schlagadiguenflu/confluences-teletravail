using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    public class SchoolClassRoomsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SchoolClassRoomsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: SchoolClassRooms
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            List<SchoolClassRoom> schoolClassRooms = JsonConvert.DeserializeObject<List<SchoolClassRoom>>(content);

            return View(schoolClassRooms);
        }

        // GET: SchoolClassRooms/Details/5
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

            // Récupération de la classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id);
            SchoolClassRoom schoolClassRoom = JsonConvert.DeserializeObject<SchoolClassRoom>(content);

            if (schoolClassRoom == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(schoolClassRoom);
        }

        // GET: SchoolClassRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: SchoolClassRooms/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolClassRoomId,SchoolClassRoomName,ExplanationVideoLink")] SchoolClassRoom schoolClassRoom)
        {
            if (ModelState.IsValid)
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(schoolClassRoom.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/SchoolClassRooms", httpContent);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'une classe, contacter l'administrateur.");
                    return View(schoolClassRoom);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(schoolClassRoom);
        }

        //// GET: SchoolClassRooms/Edit/5
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

            // Récupération de la classe
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id);
            SchoolClassRoom schoolClassRoom = JsonConvert.DeserializeObject<SchoolClassRoom>(content);

            if (schoolClassRoom == null)
            {
                return NotFound();
            }
            return View(schoolClassRoom);
        }

        //// POST: SchoolClassRooms/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SchoolClassRoomId,SchoolClassRoomName,ExplanationVideoLink")] SchoolClassRoom schoolClassRoom)
        {
            if (id != schoolClassRoom.SchoolClassRoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(schoolClassRoom.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'une classe, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(schoolClassRoom);
        }

        //// GET: SchoolClassRooms/Delete/5
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id);
            SchoolClassRoom schoolClassRoom = JsonConvert.DeserializeObject<SchoolClassRoom>(content);

            if (schoolClassRoom == null)
            {
                return NotFound();
            }

            return View(schoolClassRoom);
        }

        //// POST: SchoolClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/" + id);
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

        public async Task<IActionResult> Aide(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/SchoolClassRooms/aidereset/" + id);
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
