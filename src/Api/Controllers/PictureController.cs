using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        public static ConfluencesContext _context;
        public static IWebHostEnvironment _environnement;

        public PictureController(ConfluencesContext context,
                                  IWebHostEnvironment environnement)
        {
            _context = context;
            _environnement = environnement;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPictureAsync(string id, List<IFormFile> files)
        {
            try
            {
                string folder = "ImageProfil";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        AspNetUser user = await _context.AspNetUsers
                                                .Where(a => a.Id == id)
                                                .SingleOrDefaultAsync();
                        if (user.Id != null)
                        {
                            string filename = String.Concat(user.Id, file.FileName);
                            using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                            {
                                file.CopyTo(fileStream);
                                fileStream.Flush();

                                user.PathImage = folder + "/" + filename;
                                _context.AspNetUsers.Update(user);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }                     
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
