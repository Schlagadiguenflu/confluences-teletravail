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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Swan;

namespace mvc.Controllers
{
    public class ExercicesAlonesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExercicesAlonesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: ExercicesAlones
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
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return View(exercices);
        }

        // GET: ExercicesAlones/Details/5
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

            ExercicesAlone exercice = await JsonSerializer.DeserializeAsync<ExercicesAlone>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
            if (ViewData["IsStudent"] != null)
            {
                if ((bool)ViewData["IsStudent"])
                {
                    AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
                       await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       }
                    );

                    exercice.HomeworkV2studentExerciceAlones = exercice.HomeworkV2studentExerciceAlones.Where(h => h.StudentId == user.Id).ToList();
                }
            }

            if (exercice == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(exercice);
        }

        // GET: ExercicesAlones/Create
        public async Task<IActionResult> CreateAsync(int? homeworkId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<HomeworkV2s> homeworks = new List<HomeworkV2s>();

            if (homeworkId != null)
            {
                HomeworkV2s homework = await JsonSerializer.DeserializeAsync<HomeworkV2s>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s/" + homeworkId),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

                homeworks.Add(homework);
                ViewData["ExerciceDate"] = homework.HomeworkV2date.ToString("yyyy-MM-dd");
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
                ViewData["ExerciceDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            }

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            ViewData["TeacherId"] = user.Id;
            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");
            return View();
        }

        // POST: ExercicesAlones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciceId,ExerciceDate,ExerciceName,ExerciceLink,AudioLink,CorrectionDate,CorrectionLink,IsHomeworkAdditionnal,TeacherId,HomeworkV2id,VideoLink")] ExercicesAlone exercice, [FromForm] List<IFormFile> files, [FromForm] List<IFormFile> filesCorrection, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
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
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/ExercicesAlones", httpContent);

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
                    return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = exercice.HomeworkV2id});
                }
            }

            AspNetUser user = await JsonSerializer.DeserializeAsync<AspNetUser>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/Account/getUserInfo"),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );

            //content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Sessions");
            //List<Session> sessions = JsonSerializer.Deserialize<List<Session>>(content);
            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

            ViewData["TeacherId"] = user.Id;
            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name");

            return View(exercice);
        }

        // GET: ExercicesAlones/Edit/5
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

            ExercicesAlone exercice = await JsonSerializer.DeserializeAsync<ExercicesAlone>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id),
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

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                    await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                 );

            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text");
            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name", exercice.HomeworkV2id);
            return View(exercice);
        }

        // POST: ExercicesAlones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciceId,ExerciceDate,ExerciceName,ExerciceLink,AudioLink,CorrectionDate,CorrectionLink,IsHomeworkAdditionnal,TeacherId,HomeworkV2id,VideoLink")] ExercicesAlone exercicesAlone, [FromForm] List<IFormFile> files, [FromForm] List<IFormFile> filesCorrection, [FromForm] IFormFile fileAudio, [FromForm] IFormFile fileVideo)
        {
            if (id != exercicesAlone.ExerciceId)
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

                    exercicesAlone.ExerciceLink = await responseTheory.Content.ReadAsStringAsync();

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

                    exercicesAlone.CorrectionLink = await responseTheory.Content.ReadAsStringAsync();

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

                    exercicesAlone.AudioLink = await responseTheory.Content.ReadAsStringAsync();

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

                    exercicesAlone.VideoLink = await responseTheory.Content.ReadAsStringAsync();

                }

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(exercicesAlone.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id, httpContent);
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
                    return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = exercicesAlone.HomeworkV2id });
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

            List<HomeworkV2s> homeworks = await JsonSerializer.DeserializeAsync<List<HomeworkV2s>>(
                   await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2s"),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                );

            ViewData["HomeworkV2id"] = new SelectList(homeworks, "HomeworkV2id", "HomeworkV2name", exercicesAlone.HomeworkV2id);
            ViewData["TeacherId"] = new SelectList(selectListItemsUsers, "Value", "Text", exercicesAlone.TeacherId);
            return View(exercicesAlone);
        }

        // GET: ExercicesAlones/Delete/5
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

            ExercicesAlone exercice = await JsonSerializer.DeserializeAsync<ExercicesAlone>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id),
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

        // POST: ExercicesAlones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            ExercicesAlone exercice = await JsonSerializer.DeserializeAsync<ExercicesAlone>(
                await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Details), "HomeworkV2s", new { id = exercice.HomeworkV2id });
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

        // GET: ExercicesAlones/Duppliquer/5
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
                    ExercicesAlone exerciceAlone = await JsonSerializer.DeserializeAsync<ExercicesAlone>(
                       await client.GetStreamAsync(_configuration["URLAPI"] + "api/ExercicesAlones/" + id),
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       }
                    );
                    exerciceAlone.ExerciceId = 0;
                    exerciceAlone.HomeworkV2id = HomeworkV2id;
                    exerciceAlone.HomeworkV2studentExerciceAlones = null;

                    httpContent = new StringContent(exerciceAlone.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = await client.PostAsync(_configuration["URLAPI"] + "api/ExercicesAlones", httpContent);

                    return RedirectToAction(nameof(Edit), "HomeworkV2s", new { id = HomeworkV2id });
                }
            }

            return RedirectToAction(nameof(Index), "Ressources");

        }
    }
}
