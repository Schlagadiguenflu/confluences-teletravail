using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace mvc.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClaimsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Claims
        public async Task<ActionResult> IndexAsync()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "identity/");
            return new JsonResult(content);
        }

        //// GET: Claims/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Claims/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Claims/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Claims/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Claims/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Claims/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Claims/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}