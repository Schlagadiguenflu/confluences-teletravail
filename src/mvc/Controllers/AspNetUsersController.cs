using System;
using System.Collections.Generic;
using System.IO;
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
using Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class AspNetUsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public AspNetUsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;

            HttpClient client = new HttpClient(hch);
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            //HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            Stream streamTask;
            try
            {
                streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/AspNetUsers/");
            }
            catch (Exception)
            {
                return SignOut("Cookies", "oidc");
            }

            
            List<AspNetUser> users = await JsonSerializer.DeserializeAsync<List<AspNetUser>>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            ViewData["URLIdentityServer4"] = _configuration["URLIdentityServer4"];

            return View(users);

        }

        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUser = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (aspNetUser == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUser = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (aspNetUser == null)
            {
                return NotFound();
            }

            streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/genders");
            List<Gender> genders = await JsonSerializer.DeserializeAsync<List<Gender>>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            ViewData["GenderId"] = new SelectList(genders, "GenderId", "GenderName", aspNetUser.GenderId);

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Birthday,Firstname,LastName,GenderId, Pictures, WantsMoreHomeworks, Nationality, Language")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUserFromApi = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (id != aspNetUser.Id || id != aspNetUserFromApi.Id)
            {
                return NotFound();
            }

            // Changement des données par rapport à l'utilisateur selon le claim, pour éviter qu'un pirate ne change un autre user
            aspNetUserFromApi.Email = aspNetUser.Email;
            aspNetUserFromApi.PhoneNumber = aspNetUser.PhoneNumber;
            aspNetUserFromApi.Firstname = aspNetUser.Firstname;
            aspNetUserFromApi.LastName = aspNetUser.LastName;
            aspNetUserFromApi.GenderId = aspNetUser.GenderId;
            aspNetUserFromApi.Birthday = aspNetUser.Birthday;
            aspNetUserFromApi.WantsMoreHomeworks = aspNetUser.WantsMoreHomeworks;
            aspNetUserFromApi.Nationality = aspNetUser.Nationality;
            aspNetUserFromApi.Language = aspNetUser.Language;

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(aspNetUserFromApi.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }

                // Préparation de la requête update à l'API pour l'image
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent contentForm = new StringContent("files");
                form.Add(contentForm, "files");

                if (aspNetUser.Pictures != null)
                {
                    foreach (var item in aspNetUser.Pictures)
                    {
                        var stream = item.OpenReadStream();
                        contentForm = new StreamContent(stream);
                        contentForm.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "files",
                            FileName = Path.GetExtension(item.FileName),

                        };
                        form.Add(contentForm);
                    }
                    HttpResponseMessage responseImage = await client.PutAsync(_configuration["URLAPI"] + "api/Picture/" + id, form);
                    if (responseImage.StatusCode != HttpStatusCode.OK)
                    {
                        // Ne se passe rien,
                        //return BadRequest(response.Content);
                    }
                }


                return RedirectToAction(nameof(Details), new { id = id });
            }

            streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/genders");
            List<Gender> genders = await JsonSerializer.DeserializeAsync<List<Gender>>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
            ViewData["GenderId"] = new SelectList(genders, "GenderId", "GenderName", aspNetUser.GenderId);

            return View(aspNetUserFromApi);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            // Récurération des données et convertion des données dans le bon type
            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUser = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUser = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<bool> AspNetUserExistsAsync(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var streamTask = await client.GetStreamAsync(_configuration["URLAPI"] + "api/aspnetusers/" + id);
            AspNetUser aspNetUser = await JsonSerializer.DeserializeAsync<AspNetUser>(streamTask,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (aspNetUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
