using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkStudentsController : ControllerBase
    {
        private readonly ConfluencesContext _context;
        public static IWebHostEnvironment _environnement;

        public HomeworkStudentsController(ConfluencesContext context,
                                  IWebHostEnvironment environnement)
        {
            _context = context;
            _environnement = environnement;
        }

        // GET: api/HomeworkStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeworkStudent>>> GetHomeworkStudents()
        {
            return await _context.HomeworkStudents.AsNoTracking().ToListAsync();
        }

        // GET: api/HomeworkStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkStudent>> GetHomeworkStudent(int id)
        {
            var homeworkStudent = await _context.HomeworkStudents.FindAsync(id);

            if (homeworkStudent == null)
            {
                return NotFound();
            }

            return homeworkStudent;
        }

        [Authorize(Policy="Teacher")]
        // PUT: api/HomeworkStudents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomeworkStudent(int id, List<IFormFile> files)
        {
            var homeworkStudent = await _context.HomeworkStudents.Where(h => h.HomeworkStudentId == id).SingleOrDefaultAsync();

            _context.Entry(homeworkStudent).State = EntityState.Modified;

            string userId = "";
            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            string userName = await _context.AspNetUsers.Where(a => a.Id == userId).Select(a => a.UserName).SingleOrDefaultAsync();
            try
            {
                try
                {
                    string folder = "Devoirs";
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                            {
                                Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                            }

                            string filename = userName + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + file.FileName;
                            using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                            {
                                file.CopyTo(fileStream);
                                fileStream.Flush();
                                homeworkStudent.HomeworkFileTeacher = folder + "/" + filename;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkStudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HomeworkStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HomeworkStudent>> PostHomeworkStudent(int homeworkId, List<IFormFile> files)
        {
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            string userName = await _context.AspNetUsers.Where(a => a.Id == id).Select(a => a.UserName).SingleOrDefaultAsync();
            try
            {
                string folder = "Devoirs";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = userName + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                            _context.HomeworkStudents.Add(new HomeworkStudent 
                                                { HomeworkStudentDate = DateTime.Now, 
                                                  HomeworkFile = folder + "/" + filename,
                                                  StudentId = id,
                                                  HomeworkId = homeworkId
                            }
                            );
                            _context.SaveChanges();
                        }
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: api/HomeworkStudents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HomeworkStudent>> DeleteHomeworkStudent(int id)
        {
            var homeworkStudent = await _context.HomeworkStudents.FindAsync(id);
            if (homeworkStudent == null)
            {
                return NotFound();
            }

            _context.HomeworkStudents.Remove(homeworkStudent);
            await _context.SaveChangesAsync();

            return homeworkStudent;
        }

        private bool HomeworkStudentExists(int id)
        {
            return _context.HomeworkStudents.Any(e => e.HomeworkStudentId == id);
        }
    }
}
