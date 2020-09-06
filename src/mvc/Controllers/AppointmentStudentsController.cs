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
    public class AppointmentStudentsController : Controller
    {
        private IConfiguration _configuration { get; }

        public AppointmentStudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: AppointmentStudents
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/AppointmentStudents/");
            List<AppointmentStudent> appointmentStudents = JsonConvert.DeserializeObject<List<AppointmentStudent>>(content);

            return View(appointmentStudents);
        }

        // GET: AppointmentStudents/Details/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/AppointmentStudent/" + id);
            AppointmentStudent appointmentStudent = JsonConvert.DeserializeObject<AppointmentStudent>(content);

            if (appointmentStudent == null)
            {
                return NotFound();
            }

            return View(appointmentStudent);
        }

        // GET: AppointmentStudents/Create
        public async Task<IActionResult> CreateAsync(int? id)
        {
            ViewData["AppointmentIdReturn"] = 0;
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = "";
            List<Appointment> appointments = new List<Appointment>();
            if (id == null)
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments");
                appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);
            }
            else
            {
                ViewData["AppointmentIdReturn"] = id;
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments/" + id);
                Appointment appointment = JsonConvert.DeserializeObject<Appointment>(content);
                appointments.Add(appointment);
            }

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["AppointmentId"] = new SelectList(appointments, "AppointmentId", "AppointmentName");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View();
        }

        // POST: AppointmentStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentStudentId,AppointmentId,StudentId")] AppointmentStudent appointmentStudent)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(appointmentStudent.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/AppointmentStudents", httpContent);

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
                    return RedirectToAction("Details", "Appointments", new {id = appointmentStudent.AppointmentId});
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments");
            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["AppointmentId"] = new SelectList(appointments, "AppointmentId", "AppointmentName");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            return View(appointmentStudent);
        }

        // GET: AppointmentStudents/Edit/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/AppointmentStudents/" + id);
            AppointmentStudent appointmentStudent = JsonConvert.DeserializeObject<AppointmentStudent>(content);

            if (appointmentStudent == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments");
            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["AppointmentId"] = new SelectList(appointments, "AppointmentId", "AppointmentName");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(appointmentStudent);
        }

        // POST: AppointmentStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentStudentId,AppointmentId,StudentId")] AppointmentStudent appointmentStudent)
        {
            if (id != appointmentStudent.AppointmentStudentId)
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
                StringContent httpContent = new StringContent(appointmentStudent.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/AppointmentStudents/" + id, httpContent);
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
                    return RedirectToAction("Details", "Appointments", new { id = appointmentStudent.AppointmentId });
                }
            }

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Appointments");
            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Role/Students");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            var selectListItemsUsers = users
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Firstname + " " + s.LastName.ToString()
                  });

            ViewData["AppointmentId"] = new SelectList(appointments, "AppointmentId", "AppointmentName");
            ViewData["StudentId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(appointmentStudent);
        }

        // GET: AppointmentStudents/Delete/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/AppointmentStudents/" + id);
            AppointmentStudent appointmentStudent = JsonConvert.DeserializeObject<AppointmentStudent>(content);

            if (appointmentStudent == null)
            {
                return NotFound();
            }

            return View(appointmentStudent);
        }

        // POST: AppointmentStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer le rendez vous
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/AppointmentStudents/" + id);
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
