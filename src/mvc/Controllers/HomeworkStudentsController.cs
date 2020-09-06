using System;
using System.Collections.Generic;
using System.IO;
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
using Swan;

namespace mvc.Controllers
{
    [Authorize]
    public class HomeworkStudentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeworkStudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: HomeworkStudents
        //public async Task<IActionResult> Index()
        //{
        //    var confluencesContext = _context.HomeworkStudents.Include(h => h.Homework);
        //    return View(await confluencesContext.ToListAsync());
        //}

        // GET: HomeworkStudents/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var homeworkStudent = await _context.HomeworkStudents
        //        .Include(h => h.Homework)
        //        .FirstOrDefaultAsync(m => m.HomeworkStudentId == id);
        //    if (homeworkStudent == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(homeworkStudent);
        //}

        // GET: HomeworkStudents/Create
        //public IActionResult Create()
        //{
        //    ViewData["HomeworkId"] = new SelectList(_context.Homework, "HomeworkId", "TeacherId");
        //    return View();
        //}

        // POST: HomeworkStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pictures, HomeworkId")] HomeworkStudent homeworkStudent)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Préparation de la requête update à l'API
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("files");
            form.Add(content, "files");

            foreach (var item in homeworkStudent.Pictures)
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

            HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/HomeworkStudents?homeworkId=" + homeworkStudent.HomeworkId, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(response.Content);
            }
            return RedirectToAction("Index", "StudentPage");

        }

        // GET: HomeworkStudents/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var homeworkStudent = await _context.HomeworkStudents.FindAsync(id);
        //    if (homeworkStudent == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["HomeworkId"] = new SelectList(_context.Homework, "HomeworkId", "TeacherId", homeworkStudent.HomeworkId);
        //    return View(homeworkStudent);
        //}

        [Authorize(Policy = "Teacher")]
        // POST: HomeworkStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Pictures, HomeworkStudentId, HomeworkId")] HomeworkStudent homeworkStudent)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Préparation de la requête update à l'API
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("files");
            form.Add(content, "files");

            foreach (var item in homeworkStudent.Pictures)
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

            HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/HomeworkStudents/" + homeworkStudent.HomeworkStudentId, form);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return BadRequest();
            }

            return RedirectToAction("Details","Homework", new { id = homeworkStudent.HomeworkId });

        }

        // GET: HomeworkStudents/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var homeworkStudent = await _context.HomeworkStudents
        //        .Include(h => h.Homework)
        //        .FirstOrDefaultAsync(m => m.HomeworkStudentId == id);
        //    if (homeworkStudent == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(homeworkStudent);
        //}

        // POST: HomeworkStudents/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var homeworkStudent = await _context.HomeworkStudents.FindAsync(id);
        //    _context.HomeworkStudents.Remove(homeworkStudent);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool HomeworkStudentExists(int id)
        //{
        //    return _context.HomeworkStudents.Any(e => e.HomeworkStudentId == id);
        //}
    }
}
