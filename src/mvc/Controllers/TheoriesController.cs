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
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class TheoriesController : Controller
    {
        private readonly IConfiguration _configuration;

        public TheoriesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Theories
        public async Task<IActionResult> Index()
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

            List<Theory> theories = await JsonSerializer.DeserializeAsync<List<Theory>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return View(theories);
        }

        [AllowAnonymous]
        // GET: Theories/Details/5
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

            Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (theory == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(theory);
        }

        // GET: Theories/Create
        public async Task<IActionResult> CreateAsync(int? homeworkId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            List<HomeworkV2s> homeworks;

            if (homeworkId != null || homeworkId != 0)
            {
                homeworks = new List<HomeworkV2s>();
                HomeworkV2s homework = await JsonSerializer.DeserializeAsync<HomeworkV2s>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + homeworkId),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
                homeworks.Add(homework);
            }
            else
            {
                homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            ViewData["TheoryDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");
            ViewData["TeacherId"] = user.Id;
            return View();
        }

        // POST: Theories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheoryId,TheoryDate,TheoryName,TheoryLink,TeacherId,HomeworkV2id,AudioLink,VideoLink")] Theory theory, [FromForm] List<IFormFile> files, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                if (files.Count() > 0)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    foreach (var item in files)
                    {
                        var stream = item.OpenReadStream();
                        content = new StreamContent(stream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "files",
                            FileName = Path.GetExtension(item.FileName)
                        };
                        form.Add(content);
                    }
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheory", form);

                    theory.TheoryLink = await responseTheory.Content.ReadAsStringAsync();

                }
                if (fileAudio != null)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    var stream = fileAudio.OpenReadStream();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Path.GetExtension(fileAudio.FileName)
                    };
                    form.Add(content);

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheory", form);

                    theory.AudioLink = await responseTheory.Content.ReadAsStringAsync();

                }

                if (fileVideo != null)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    var stream = fileVideo.OpenReadStream();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Path.GetExtension(fileVideo.FileName)
                    };
                    form.Add(content);

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheoryVideo", form);

                    theory.VideoLink = await responseTheory.Content.ReadAsStringAsync();

                }


                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(theory.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Theories", httpContent);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'un cours, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = theory.HomeworkV2id });
                }
            }

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
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

            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");
            ViewData["TeacherId"] = user.Id;

            return View(theory);
        }

        // GET: Theories/Edit/5
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

            Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (theory == null)
            {
                return NotFound();
            }

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

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

            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            return View(theory);
        }

        // POST: Theories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheoryId,TheoryDate,TheoryName,TheoryLink,TeacherId,HomeworkV2id,AudioLink,VideoLink")] Theory theory, [FromForm] List<IFormFile> files, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
        {
            if (id != theory.TheoryId)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                if (files.Count() > 0)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    foreach (var item in files)
                    {
                        var stream = item.OpenReadStream();
                        content = new StreamContent(stream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "files",
                            FileName = Path.GetExtension(item.FileName)
                        };
                        form.Add(content);
                    }
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheory", form);

                    theory.TheoryLink = await responseTheory.Content.ReadAsStringAsync();

                }
                if (fileAudio != null)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    var stream = fileAudio.OpenReadStream();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Path.GetExtension(fileAudio.FileName)
                    };
                    form.Add(content);

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheory", form);

                    theory.AudioLink = await responseTheory.Content.ReadAsStringAsync();

                }
                if (fileVideo != null)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    var stream = fileVideo.OpenReadStream();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Path.GetExtension(fileVideo.FileName)
                    };
                    form.Add(content);

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveTheoryVideo", form);

                    theory.VideoLink = await responseTheory.Content.ReadAsStringAsync();

                }
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(theory.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Theories/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'un cours, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Details", "HomeworkV2s", new { id = theory.HomeworkV2id });
                }
            }

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

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

            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");

            return View(theory);
        }

        // GET: Theories/Delete/5
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

            Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (theory == null)
            {
                return NotFound();
            }

            return View(theory);
        }

        // POST: Theories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/Theories/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = theory.HomeworkV2id });
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

        // GET: Theories/Duppliquer/5
        public async Task<IActionResult> Duppliquer(int id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            HomeworkV2s homeworkV2s = new HomeworkV2s
            {
                HomeworkV2name = "Nom à changer",
                TeacherId = user.Id,
                HomeworkTypeId = 1,
                HomeworkV2date = DateTime.Now
            };

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
                    int HomeworkV2id = 0;
                    try
                    {
                        HomeworkV2id = int.Parse(response.Headers.Location.Segments[3]);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Erreur à la dupplication d'un devoir (réception), contacter l'administrateur.");
                    }

                    // Duplicate theory
                    Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                       await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + id),
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       }
                    );
                    theory.TheoryId = 0;
                    theory.HomeworkV2id = HomeworkV2id;
                    theory.Teacher = null;
                    theory.HomeworkV2 = null;
                    ICollection<Exercice> exercices = theory.Exercices;
                    theory.Exercices = null;

                    httpContent = new StringContent(theory.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = await client.PostAsync(_configuration["URLAPI"] + "api/Theories", httpContent);
                    int theoryId = 0;
                    try
                    {
                        theoryId = int.Parse(response2.Headers.Location.Segments[3]);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Erreur à la dupplication d'un devoir (réception), contacter l'administrateur.");
                    }

                    foreach (var exercice in exercices)
                    {
                        exercice.ExerciceId = 0;
                        exercice.TheoryId = theoryId;
                        httpContent = new StringContent(exercice.ToJson(), Encoding.UTF8, "application/json");
                        HttpResponseMessage response3 = await client.PostAsync(_configuration["URLAPI"] + "api/Exercices", httpContent);
                    }
                    return RedirectToAction(nameof(Edit), "HomeworkV2s", new { id = HomeworkV2id });
                }
            }

            return RedirectToAction(nameof(Index), "Ressources");

        }

    }
}
