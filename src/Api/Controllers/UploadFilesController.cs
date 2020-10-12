using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        public static IWebHostEnvironment _environnement;
        private readonly ConfluencesContext _context;

        public UploadFilesController(IWebHostEnvironment environnement, ConfluencesContext context)
        {
            _environnement = environnement;
            _context = context;
        }

        [Authorize(Policy = "teacher")]
        [Route("SaveTheory")]
        [HttpPost]
        public ActionResult<string> PostSaveTheory(List<IFormFile> files)
        {
            try
            {
                string path = "";
                string folder = "Cours";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;
                    }
                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "teacher")]
        [Route("SaveTheoryVideo")]
        [HttpPost]
        public ActionResult<string> PostSaveTheoryVideo(List<IFormFile> files)
        {
            try
            {
                string path = "";
                string folder = "Cours";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;
                    }
                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "teacher")]
        [Route("SaveExercice")]
        [HttpPost]
        public ActionResult<string> PostSaveExercice(List<IFormFile> files)
        {
            try
            {
                string path = "";
                string folder = "Exercice";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;
                    }
                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [Route("SaveHomeworkStudent")]
        [HttpPost]
        public async Task<ActionResult<string>> PostSaveHomeworkStudentAsync(List<IFormFile> files, int id)
        {
            try
            {
                string path = "";
                string folder = "Devoirs_Participants";
                string userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;
                        HomeworkV2students homework = new HomeworkV2students { 
                            ExerciceId = id,
                            HomeworkFile = path,
                            HomeworkStudentDate = DateTime.Now,
                            StudentId = userId
                        };

                        await _context.HomeworkV2students.AddAsync(homework);
                        await _context.SaveChangesAsync();
                    }

                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "teacher")]
        [Route("EditHomeworkStudent")]
        [HttpPost]
        public async Task<ActionResult<string>> PostEditHomeworkStudentAsync(List<IFormFile> files, int id)
        {
            try
            {
                string path = "";
                string folder = "Devoirs_Participants";
                string userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;

                        var homework = await _context.HomeworkV2students.FindAsync(id);

                        homework.HomeworkFile = path;

                        _context.HomeworkV2students.Update(homework);

                        await _context.SaveChangesAsync();
                    }

                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [Route("EraseHomeworkStudent")]
        [HttpDelete]
        public async Task<ActionResult<HomeworkV2students>> PostEraseHomeworkStudentAsync(int id)
        {
            var homework = await _context.HomeworkV2students.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            _context.HomeworkV2students.Remove(homework);
            await _context.SaveChangesAsync();

            return homework;

        }

        [Authorize]
        [Route("SaveHomeworkStudentAlone")]
        [HttpPost]
        public async Task<ActionResult<string>> PostSaveHomeworkStudentAloneAsync(List<IFormFile> files, int id)
        {
            try
            {
                string path = "";
                string folder = "Devoirs_Participants";
                string userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;
                        HomeworkV2studentExerciceAlones homework = new HomeworkV2studentExerciceAlones
                        {
                            ExerciceId = id,
                            HomeworkFile = path,
                            HomeworkStudentDate = DateTime.Now,
                            StudentId = userId
                        };

                        await _context.HomeworkV2studentExerciceAlones.AddAsync(homework);
                        await _context.SaveChangesAsync();
                    }

                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "teacher")]
        [Route("EditHomeworkStudentAlone")]
        [HttpPost]
        public async Task<ActionResult<string>> PostEditHomeworkStudentAloneAsync(List<IFormFile> files, int id)
        {
            try
            {
                string path = "";
                string folder = "Devoirs_Participants";
                string userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + Path.GetRandomFileName() + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        path = folder + "/" + filename;

                        var homework = await _context.HomeworkV2studentExerciceAlones.FindAsync(id);

                        homework.HomeworkFile = path;

                        _context.HomeworkV2studentExerciceAlones.Update(homework);

                        await _context.SaveChangesAsync();
                    }

                }
                return Content(path);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [Route("EraseHomeworkStudentAlone")]
        [HttpDelete]
        public async Task<ActionResult<HomeworkV2studentExerciceAlones>> PostEraseHomeworkStudentAloneAsync(int id)
        {
            var homework = await _context.HomeworkV2studentExerciceAlones.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            _context.HomeworkV2studentExerciceAlones.Remove(homework);
            await _context.SaveChangesAsync();

            return homework;

        }
    }
}
