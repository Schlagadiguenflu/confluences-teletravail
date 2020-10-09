using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class StudentPageController : Controller
    {
        private IConfiguration _configuration { get; }

        public StudentPageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewData["IsStudent"] = true;

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfoStudent");
            }
            catch (UnauthorizedAccessException)
            {
                return SignOut("Cookies", "oidc");
            }
            catch (Exception e)
            {
                return SignOut("Cookies", "oidc");
            }

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user != null)
            {
                if (user.SessionStudents.Count() <= 0)
                {
                    return RedirectToAction("WaitingZone", "StudentPage");
                }
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];
            return View(user);

        }

        public async Task<IActionResult> HelpAsync()
        {
            ViewData["IsStudent"] = true;

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfoStudent");
            }
            catch (UnauthorizedAccessException)
            {
                return SignOut("Cookies", "oidc");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user != null)
            {
                if (user.SessionStudents.Count() > 0)
                {
                    DateTime dateStartMax = user.SessionStudents.Max(s => s.Session.DateStart);
                    user.SessionStudents = user.SessionStudents.Where(s => s.Session.DateStart == dateStartMax).ToList();
                }
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];
            return View(user);

        }

        public IActionResult MyPictures()
        {
            ViewData["IsStudent"] = true;
            return View();
        }

        public IActionResult Covid()
        {
            ViewData["IsStudent"] = true;
            return View();
        }

        public async Task<IActionResult> MyAppointmentsAsync()
        {
            ViewData["IsStudent"] = true;

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfoAppointments");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            // get all futur appointements and the weekly ones
            if (user != null)
            {
                if (user.AppointmentStudents.Count() > 0)
                {
                    DateTime now = DateTime.Now.AddDays(-1);
                    user.AppointmentStudents = user.AppointmentStudents.Where(s => s.Appointment.DateEnd > now).ToList();
                }
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];
            return View("MyAppointments", user);

        }

        public async Task<IActionResult> PreviousLessonsAsync()
        {
            ViewData["IsStudent"] = true;

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfoStudent");
            //try
            //{
            //    content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfoStudent");
            //}
            //catch (UnauthorizedAccessException)
            //{
            //    return SignOut("Cookies", "oidc");
            //}
            //catch (Exception)
            //{
            //    return SignOut("Cookies", "oidc");
            //}

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user != null)
            {
                if (user.SessionStudents.Count() > 0)
                {
                    DateTime dateStartMax = user.SessionStudents.Max(s => s.Session.DateStart);
                    ViewData["SessionId"] = user.SessionStudents.Where(s => s.Session.DateStart == dateStartMax).Select(u => u.SessionId).FirstOrDefault();
                    user.SessionStudents = user.SessionStudents.Where(s => s.Session.DateStart == dateStartMax).ToList();
                }
            }

            ViewData["URLClientMVC"] = _configuration["URLClientMVC"];
            ViewData["URLAPI"] = _configuration["URLAPI"];
            ViewData["USERID"] = user.Id;
            return View(user);

        }

        public IActionResult WaitingZone()
        {
            ViewData["IsStudent"] = false;
            return View();
        }

        public async Task<IActionResult> IGetItAsync()
        {
            ViewData["IsStudent"] = true;
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            try
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/IGetIt");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }
            return RedirectToAction("Index", "StudentPage");
        }
    }
}