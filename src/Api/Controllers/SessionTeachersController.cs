using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class SessionTeachersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public SessionTeachersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/SessionTeachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionTeacher>>> GetSessionTeachers()
        {
            return await _context.SessionTeachers
                                         .AsNoTracking()
                                         .Include(s => s.Session)
                                         .Include(s => s.Teacher)
                                         .ToListAsync();
        }

        // GET: api/SessionTeachers/5
        [HttpGet("{sessionId}/{teacherId}")]
        public async Task<ActionResult<SessionTeacher>> GetSessionTeacher(int sessionId, string teacherId)
        {
            var sessionTeacher = await _context.SessionTeachers
                                            .Include(s => s.Session)
                                            .Include(s => s.Teacher)
                                            .Where(s => s.SessionId == sessionId && s.TeacherId == teacherId)
                                            .SingleOrDefaultAsync();

            if (sessionTeacher == null)
            {
                return NotFound();
            }

            return sessionTeacher;
        }


        // POST: api/SessionTeachers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionTeacher>> PostSessionTeacher(SessionTeacher sessionTeacher)
        {
            _context.SessionTeachers.Add(sessionTeacher);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SessionTeacherExists(sessionTeacher.SessionId, sessionTeacher.TeacherId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSessionTeacher", new { sessionId = sessionTeacher.SessionId, teacherId = sessionTeacher.TeacherId }, sessionTeacher);
        }

        // DELETE: api/SessionTeachers/5
        [HttpDelete("{sessionId}/{teacherId}")]
        public async Task<ActionResult<SessionTeacher>> DeleteSessionTeacher(int sessionId, string teacherId)
        {
            var sessionTeacher = await _context.SessionTeachers
                                            .Where(s => s.SessionId == sessionId && s.TeacherId == teacherId)
                                            .SingleOrDefaultAsync();
            if (sessionTeacher == null)
            {
                return NotFound();
            }

            _context.SessionTeachers.Remove(sessionTeacher);
            await _context.SaveChangesAsync();

            return sessionTeacher;
        }

        private bool SessionTeacherExists(int session, string teacher)
        {
            return _context.SessionTeachers.Any(e => e.SessionId == session && e.TeacherId == teacher);
        }
    }
}
