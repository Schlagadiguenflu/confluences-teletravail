using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public AspNetUsersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/AspNetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetAspNetUsers()
        {
            var aspnetusers =  await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SchoolClassRoom)
                            .OrderBy(a => a.Firstname)
                            .ToListAsync();


            return aspnetusers;
        }

        // GET: api/AspNetUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUser>> GetAspNetUser(string id)
        {
            var aspNetUser = await _context.AspNetUsers
                                        .Where(a => a.Id == id)
                                        .Include(a => a.Gender)
                                        .Include(a => a.SessionStudents)
                                            .ThenInclude(a => a.Session)
                                                .ThenInclude(a => a.SchoolClassRoom)
                                        .SingleOrDefaultAsync();

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return aspNetUser;
        }

        // PUT: api/AspNetUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUser(string id, AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        // DELETE: api/AspNetUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUser>> DeleteAspNetUser(string id)
        {
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();

            return aspNetUser;
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
