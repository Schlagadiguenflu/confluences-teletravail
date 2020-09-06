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
    [Authorize(Policy="Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TheoriesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TheoriesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Theories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theory>>> GetTheories()
        {
            return await _context.Theories.AsNoTracking()
                                    .Include(t => t.HomeworkV2)
                                    .Include(t => t.Teacher)
                                    .ToListAsync();
        }

        [AllowAnonymous]
        // GET: api/Theories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theory>> GetTheory(int id)
        {
            var theory = await _context.Theories
                                       .AsNoTracking()
                                       .Include(t => t.HomeworkV2)
                                       .Include(t => t.Teacher)
                                       .Include(t => t.Exercices)
                                       .Where(t => t.TheoryId == id)
                                       .SingleOrDefaultAsync();

            if (theory == null)
            {
                return NotFound();
            }

            return theory;
        }

        // PUT: api/Theories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheory(int id, Theory theory)
        {
            if (id != theory.TheoryId)
            {
                return BadRequest();
            }

            _context.Entry(theory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheoryExists(id))
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

        // POST: api/Theories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Theory>> PostTheory(Theory theory)
        {
            _context.Theories.Add(theory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheory", new { id = theory.TheoryId }, theory);
        }

        // DELETE: api/Theories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Theory>> DeleteTheory(int id)
        {
            var theory = await _context.Theories.FindAsync(id);
            if (theory == null)
            {
                return NotFound();
            }

            _context.Theories.Remove(theory);
            await _context.SaveChangesAsync();

            return theory;
        }

        private bool TheoryExists(int id)
        {
            return _context.Theories.Any(e => e.TheoryId == id);
        }
    }
}
