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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Swan;

namespace mvc.Controllers
{
    public class HomeworkV2studentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeworkV2studentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHomework(string action, int id, int ExerciceId, List<IFormFile> files)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

            var response = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/EditHomeworkStudent?id=" + id, form);

            return RedirectToAction("Details", action, new { id = ExerciceId });

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, List<IFormFile> files)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

            var response = await client.PostAsync(_configuration["URLAPI"] + "api/UploadFiles/SaveHomeworkStudent?id=" + id, form);

            return RedirectToAction("Details", "Exercices", new { id = id });

        }

        // POST: HomeworkV2students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HomeworkV2studentId,HomeworkStudentDate,HomeworkFile,HomeworkFileTeacher,HomeworkCommentaryTeacher,ExerciceId,StudentId")] HomeworkV2students homeworkV2students)
        {
            if (id != homeworkV2students.HomeworkV2studentId)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HomeworkV2students homework = await JsonSerializer.DeserializeAsync<HomeworkV2students>(
               await client.GetStreamAsync(_configuration["URLAPI"] + "api/HomeworkV2students/" + id),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
            );
            string commentary = homeworkV2students.HomeworkCommentaryTeacher;
            homeworkV2students = homework;
            homeworkV2students.HomeworkCommentaryTeacher = commentary;

            // Préparation de la requête update à l'API
            StringContent httpContent = new StringContent(homeworkV2students.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/HomeworkV2students/" + id, httpContent);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                ModelState.AddModelError(string.Empty, "Erreur à la mise à jour d'un commentaire, contacter l'administrateur.");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                return RedirectToAction("Details", "Exercices", new { id = homeworkV2students.ExerciceId });
            }


            return RedirectToAction("Details", "Exercices", new { id = homeworkV2students.ExerciceId });
        }

        // POST: HomeworkV2students/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Effacer l'utilisateur
            HttpResponseMessage result = await client.DeleteAsync(_configuration["URLAPI"] + "api/UploadFiles/EraseHomeworkStudent?id=" + id);

            HomeworkV2students homework = await JsonSerializer.DeserializeAsync<HomeworkV2students>(
                await result.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Details", "Exercices", new { id = homework.ExerciceId });
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
