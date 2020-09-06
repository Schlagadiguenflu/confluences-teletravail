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
    public class SessionStudentsController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public SessionStudentsController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/SessionStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionStudent>>> GetSessionStudents()
        {
            return await _context.SessionStudents
                                         .AsNoTracking()
                                         .Include(s => s.Session)
                                         .Include(s => s.Student)
                                         .ToListAsync();
        }

        // GET: api/SessionStudents/5
        [HttpGet("{sessionId}/{studentId}")]
        public async Task<ActionResult<SessionStudent>> GetSessionStudent(int sessionId, string studentId)
        {
            var sessionStudent = await _context.SessionStudents
                                           .Include(s => s.Session)
                                                .ThenInclude(s => s.SchoolClassRoom)
                                           .Include(s => s.Student)
                                           .Where(s => s.SessionId == sessionId && s.StudentId == studentId)
                                           .SingleOrDefaultAsync();

            if (sessionStudent == null)
            {
                return NotFound();
            }

            return sessionStudent;
        }

        // POST: api/SessionStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionStudent>> PostSessionStudent(SessionStudent sessionStudent)
        {
            _context.SessionStudents.Add(sessionStudent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SessionStudentExists(sessionStudent.SessionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSessionStudent", new { sessionId = sessionStudent.SessionId, studentId= sessionStudent.StudentId }, sessionStudent);
        }

        // DELETE: api/SessionStudents/5
        [HttpDelete("{sessionId}/{studentId}")]
        public async Task<ActionResult<SessionStudent>> DeleteSessionStudent(int sessionId, string studentId)
        {
            var sessionStudent = await _context.SessionStudents
                                            .Where(s => s.SessionId == sessionId && s.StudentId == studentId)
                                            .SingleOrDefaultAsync();
            if (sessionStudent == null)
            {
                return NotFound();
            }

            _context.SessionStudents.Remove(sessionStudent);
            await _context.SaveChangesAsync();

            return sessionStudent;
        }

        private bool SessionStudentExists(int id)
        {
            return _context.SessionStudents.Any(e => e.SessionId == id);
        }
    }
}
