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
    public class AppointmentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AppointmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //private class Day {
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //}

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments/");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);

            return View(appointments);
        }

        // GET: Appointments/Details/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments/" + id);
            Appointment appointment = JsonConvert.DeserializeObject<Appointment>(content);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> CreateAsync()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Teachers");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,AppointmentName,DateStart,DateEnd,TeacherId,IsWeekly")] Appointment appointment)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

           // AspNetUser user = await System.Text.Json.JsonSerializer.DeserializeAsync<AspNetUser>(
           //   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
           //   new System.Text.Json.JsonSerializerOptions
           //   {
           //       PropertyNameCaseInsensitive = true
           //   }
           //);
           // appointment.TeacherId = user.Id;
            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(appointment.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Appointments", httpContent);

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
                    if(id != 0)
                    {
                        return RedirectToAction(nameof(Details), new { id = id});
                    }
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Teachers");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            return View(appointment);
        }

        // GET: Appointments/Edit/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments/" + id);
            Appointment appointment = JsonConvert.DeserializeObject<Appointment>(content);

            if (appointment == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Teachers");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            //ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text", appointment.TeacherId);
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,AppointmentName,DateStart,DateEnd,TeacherId,IsWeekly")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
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
                StringContent httpContent = new StringContent(appointment.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Appointments/" + id, httpContent);
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
                    return RedirectToAction(nameof(Details), new { id = id });
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Teachers");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text", appointment.TeacherId);

            return View(appointment);
        }

        // GET: Appointments/Delete/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments/" + id);
            Appointment appointment = JsonConvert.DeserializeObject<Appointment>(content);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer le rendez vous
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/Appointments/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Appointments");
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
