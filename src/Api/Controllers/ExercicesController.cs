using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExercicesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public ExercicesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Exercices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercice>>> GetExercices()
        {
            return await _context.Exercices
                                        .AsNoTracking()
                                        .Include(e => e.Teacher)
                                        .Include(e => e.Theory)
                                        .ToListAsync();
        }

        [AllowAnonymous]
        // GET: api/Exercices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercice>> GetExercice(int id)
        {
            var exercice = await _context.Exercices
                                        .AsNoTracking()
                                        .Include(e => e.Teacher)
                                        .Include(e => e.Theory)
                                        .Include(e => e.HomeworkV2students)
                                            .ThenInclude(e => e.Student)
                                        .Where(e => e.ExerciceId == id)
                                        .SingleOrDefaultAsync();

            if (exercice == null)
            {
                return NotFound();
            }

            return exercice;
        }

        // PUT: api/Exercices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercice(int id, Exercice exercice)
        {
            if (id != exercice.ExerciceId)
            {
                return BadRequest();
            }

            _context.Entry(exercice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciceExists(id))
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

        // POST: api/Exercices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Exercice>> PostExercice(Exercice exercice)
        {
            _context.Exercices.Add(exercice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercice", new { id = exercice.ExerciceId }, exercice);
        }

        // DELETE: api/Exercices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Exercice>> DeleteExercice(int id)
        {
            var exercice = await _context.Exercices.FindAsync(id);
            if (exercice == null)
            {
                return NotFound();
            }

            _context.Exercices.Remove(exercice);
            await _context.SaveChangesAsync();

            return exercice;
        }

        private bool ExerciceExists(int id)
        {
            return _context.Exercices.Any(e => e.ExerciceId == id);
        }
    }
}
