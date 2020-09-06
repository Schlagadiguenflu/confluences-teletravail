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
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public HomeworkController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Homework
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Homework>>> GetHomework()
        {
            return await _context.Homework
                                .AsNoTracking()
                                .Include(h => h.Session)
                                    .ThenInclude(h => h.SchoolClassRoom)
                                .Include(h => h.HomeworkType)
                                .Include(h => h.Teacher)
                                .Include(h => h.HomeworkStudents)
                                .ToListAsync();
        }

        // GET: api/homework/sessionhomework?sessionId=1&homeworkTypeId=1
        [Route("SessionHomework")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Homework>>> GetSessionHomework(int sessionId, int homeworkTypeId)
        {
            return await _context.Homework
                                .AsNoTracking()
                                .Include(h => h.Session)
                                    .ThenInclude(h => h.SchoolClassRoom)
                                .Include(h => h.HomeworkType)
                                .Include(h => h.Teacher)
                                .Where(h => h.SessionId == sessionId && h.HomeworkTypeId == homeworkTypeId)
                                .ToListAsync();
        }
        // GET: api/homework/sessionhomework?sessionId=1&homeworkTypeId=1
        [Route("SessionHomeworkAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Homework>>> GetSessionHomeworkWithoutType(int sessionId)
        {
            return await _context.Homework
                                .AsNoTracking()
                                .Include(h => h.Session)
                                    .ThenInclude(h => h.SchoolClassRoom)
                                .Include(h => h.HomeworkType)
                                .Include(h => h.Teacher)
                                .Where(h => h.SessionId == sessionId)
                                .ToListAsync();
        }

        // GET: api/Homework/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Homework>> GetHomework(int id)
        {
            var homework = await _context.Homework
                                .Include(h => h.Session)
                                    .ThenInclude(h => h.SchoolClassRoom)
                                .Include(h => h.HomeworkType)
                                .Include(h => h.Teacher)
                                .Include(h => h.HomeworkStudents)
                                    .ThenInclude(h => h.Student)
                                .Where(h => h.HomeworkId == id)
                                .SingleOrDefaultAsync();

            if (homework == null)
            {
                return NotFound();
            }

            return homework;
        }

        [Authorize(Policy = "Teacher")]
        // PUT: api/Homework/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomework(int id, Homework homework)
        {
            if (id != homework.HomeworkId)
            {
                return BadRequest();
            }

            _context.Entry(homework).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkExists(id))
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

        [Authorize(Policy = "Teacher")]
        // POST: api/Homework
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Homework>> PostHomework(Homework homework)
        {
            _context.Homework.Add(homework);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomework", new { id = homework.HomeworkId }, homework);
        }

        [Authorize(Policy = "Teacher")]
        // DELETE: api/Homework/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Homework>> DeleteHomework(int id)
        {
            var homework = await _context.Homework.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            _context.Homework.Remove(homework);
            await _context.SaveChangesAsync();

            return homework;
        }

        private bool HomeworkExists(int id)
        {
            return _context.Homework.Any(e => e.HomeworkId == id);
        }
    }
}
