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
    public class ExercicesAlonesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public ExercicesAlonesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/ExercicesAlones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExercicesAlone>>> GetExercicesAlones()
        {
            return await _context.ExercicesAlones.ToListAsync();
        }

        [AllowAnonymous]
        // GET: api/ExercicesAlones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExercicesAlone>> GetExercicesAlone(int id)
        {
            var exercicesAlone = await _context.ExercicesAlones
                                                .Include(e => e.HomeworkV2studentExerciceAlones)
                                                    .ThenInclude(e => e.Student)
                                                .Where(e => e.ExerciceId == id)
                                                .SingleOrDefaultAsync();

            if (exercicesAlone == null)
            {
                return NotFound();
            }

            return exercicesAlone;
        }

        // PUT: api/ExercicesAlones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercicesAlone(int id, ExercicesAlone exercicesAlone)
        {
            if (id != exercicesAlone.ExerciceId)
            {
                return BadRequest();
            }

            _context.Entry(exercicesAlone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExercicesAloneExists(id))
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

        // POST: api/ExercicesAlones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ExercicesAlone>> PostExercicesAlone(ExercicesAlone exercicesAlone)
        {
            _context.ExercicesAlones.Add(exercicesAlone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercicesAlone", new { id = exercicesAlone.ExerciceId }, exercicesAlone);
        }

        // DELETE: api/ExercicesAlones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExercicesAlone>> DeleteExercicesAlone(int id)
        {
            var exercicesAlone = await _context.ExercicesAlones.FindAsync(id);
            if (exercicesAlone == null)
            {
                return NotFound();
            }

            _context.ExercicesAlones.Remove(exercicesAlone);
            await _context.SaveChangesAsync();

            return exercicesAlone;
        }

        private bool ExercicesAloneExists(int id)
        {
            return _context.ExercicesAlones.Any(e => e.ExerciceId == id);
        }
    }
}
