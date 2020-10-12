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
    public class ExercicesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExercicesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Exercices
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

            List<Exercice> exercices = await JsonSerializer.DeserializeAsync<List<Exercice>>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Exercices/"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return View(exercices);
        }

        [AllowAnonymous]
        // GET: Exercices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["IsTeacher"] = false;
            ViewData["IsStudent"] = false;
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

            Exercice exercice = await JsonSerializer.DeserializeAsync<Exercice>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Exercices/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if ((bool)ViewData["IsStudent"])
            {
                AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

                exercice.HomeworkV2students = exercice.HomeworkV2students.Where(h => h.StudentId == user.Id).ToList();
            }

            if (exercice == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(exercice);
        }

        // GET: Exercices/Create
        public async Task<IActionResult> CreateAsync(int? theoryId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<Theory> theories = new List<Theory>();

            if (theoryId != null)
            {
                Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + theoryId),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

                theories.Add(theory);
            }
            else
            {
                theories = await JsonSerializer.DeserializeAsync<List<Theory>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories"),
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

            ViewData["ExerciceDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["TeacherId"] = user.Id;
            ViewData["TheoryId"] = new SelectList(theories, "TheoryId", "TheoryName");
            return View();
        }

        // POST: Exercices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciceId,ExerciceDate,ExerciceName,ExerciceLink,IsHomeworkAdditionnal,TeacherId,TheoryId,CorrectionDate,CorrectionLink,AudioLink,VideoLink")] Exercice exercice, [FromForm] List<IFormFile> files, [FromForm] List<IFormFile> filesCorrection, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (exercice.CorrectionDate == new DateTime(1, 1, 1))
            {
                int year = DateTime.Now.AddYears(10).Year;
                exercice.CorrectionDate = new DateTime(year, 1, 1);
            }
            ModelState.Clear();
            TryValidateModel(exercice);

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
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.ExerciceLink = await responseTheory.Content.ReadAsStringAsync();

                }

                if (filesCorrection.Count() > 0)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    foreach (var item in filesCorrection)
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
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.CorrectionLink = await responseTheory.Content.ReadAsStringAsync();

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

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.AudioLink = await responseTheory.Content.ReadAsStringAsync();

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

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.VideoLink = await responseTheory.Content.ReadAsStringAsync();

                }

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(exercice.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Exercices", httpContent);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la création d'un exercice, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                        await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + exercice.TheoryId),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = theory.HomeworkV2.HomeworkV2id });
                }
            }

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            List<Theory> theories = await JsonSerializer.DeserializeAsync<List<Theory>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
            );

            ViewData["TeacherId"] = user.Id;
            ViewData["TheoryId"] = new SelectList(theories, "TheoryId", "TheoryName");
            return View(exercice);
        }

        // GET: Exercices/Edit/5
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

            Exercice exercice = await JsonSerializer.DeserializeAsync<Exercice>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Exercices/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (exercice == null)
            {
                return NotFound();
            }

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

            List<Theory> theories = await JsonSerializer.DeserializeAsync<List<Theory>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
            );

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            ViewData["TheoryId"] = new SelectList(theories, "TheoryId", "TheoryName");
            return View(exercice);
        }

        // POST: Exercices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciceId,ExerciceDate,ExerciceName,ExerciceLink,IsHomeworkAdditionnal,TeacherId,TheoryId,CorrectionDate,CorrectionLink,AudioLink,VideoLink")] Exercice exercice, [FromForm] List<IFormFile> files, [FromForm] List<IFormFile> filesCorrection, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
        {
            if (id != exercice.ExerciceId)
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
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.ExerciceLink = await responseTheory.Content.ReadAsStringAsync();

                }

                if (filesCorrection.Count() > 0)
                {
                    // Préparation de la requête update à l'API
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("files");
                    form.Add(content, "files");

                    foreach (var item in filesCorrection)
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
                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.CorrectionLink = await responseTheory.Content.ReadAsStringAsync();

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

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.AudioLink = await responseTheory.Content.ReadAsStringAsync();

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

                    HttpResponseMessage responseTheory = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveExercice", form);

                    exercice.VideoLink = await responseTheory.Content.ReadAsStringAsync();

                }

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(exercice.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Exercices/" + id, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'un exercice, contacter l'administrateur.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    Theory theory = await JsonSerializer.DeserializeAsync<Theory>(
                        await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories/" + exercice.TheoryId),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = theory.HomeworkV2.HomeworkV2id });
                }
            }

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

            List<Theory> theories = await JsonSerializer.DeserializeAsync<List<Theory>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/Theories"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
            );

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            ViewData["TheoryId"] = new SelectList(theories, "TheoryId", "TheoryName");
            return View(exercice);
        }

        // GET: Exercices/Delete/5
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

            Exercice exercice = await JsonSerializer.DeserializeAsync<Exercice>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Exercices/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (exercice == null)
            {
                return NotFound();
            }

            return View(exercice);
        }

        // POST: Exercices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            Exercice exercice = await JsonSerializer.DeserializeAsync<Exercice>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/Exercices/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/Exercices/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = exercice.Theory.HomeworkV2id });
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
