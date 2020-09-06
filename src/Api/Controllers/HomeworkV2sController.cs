using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkV2sController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public HomeworkV2sController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/HomeworkV2s
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeworkV2s>>> GetHomeworkV2s()
        {
            return await _context.HomeworkV2s.AsNoTracking()
                                         .Include(h => h.HomeworkType)
                                         .Include(h => h.Session)
                                            .ThenInclude(h => h.SchoolClassRoom)
                                         .Include(h => h.Teacher)
                                         .OrderByDescending(h => h.HomeworkV2date)
                                         .ToListAsync();
        }

        // GET: api/HomeworkV2s/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkV2s>> GetHomeworkV2s(int id)
        {
            var homeworkV2s = await _context.HomeworkV2s.AsNoTracking()
                                         .Include(h => h.HomeworkType)
                                         .Include(h => h.Session)
                                            .ThenInclude(h => h.SchoolClassRoom)
                                         .Include(h => h.Teacher)
                                         .Include(h => h.Theories)
                                            .ThenInclude(t => t.Exercices)
                                         .Include(h => h.ExercicesAlones)
                                         .Where(h => h.HomeworkV2id == id)
                                         .SingleOrDefaultAsync();

            if (homeworkV2s == null)
            {
                return NotFound();
            }

            return homeworkV2s;
        }

        // PUT: api/HomeworkV2s/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomeworkV2s(int id, HomeworkV2s homeworkV2s)
        {
            if (id != homeworkV2s.HomeworkV2id)
            {
                return BadRequest();
            }

            _context.Entry(homeworkV2s).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkV2sExists(id))
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

        // POST: api/HomeworkV2s
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HomeworkV2s>> PostHomeworkV2s(HomeworkV2s homeworkV2s)
        {
            _context.HomeworkV2s.Add(homeworkV2s);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomeworkV2s", new { id = homeworkV2s.HomeworkV2id }, homeworkV2s);
        }

        // DELETE: api/HomeworkV2s/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HomeworkV2s>> DeleteHomeworkV2s(int id)
        {
            var homeworkV2s = await _context.HomeworkV2s.FindAsync(id);
            if (homeworkV2s == null)
            {
                return NotFound();
            }

            _context.HomeworkV2s.Remove(homeworkV2s);
            await _context.SaveChangesAsync();

            return homeworkV2s;
        }

        private bool HomeworkV2sExists(int id)
        {
            return _context.HomeworkV2s.Any(e => e.HomeworkV2id == id);
        }
    }
}
