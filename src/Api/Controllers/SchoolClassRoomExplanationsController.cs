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
    public class SchoolClassRoomExplanationsController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public SchoolClassRoomExplanationsController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/SchoolClassRoomExplanations?schoolClassRoomId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClassRoomExplanation>>> GetSchoolClassRoomExplanations(int? schoolClassRoomId = 0)
        {
            if (schoolClassRoomId == 0)
            {
                return await _context.SchoolClassRoomExplanations
                    .AsNoTracking()
                    .Include(s => s.SchoolClassRoom)
                    .ToListAsync();
            }
            else
            {
                return await _context.SchoolClassRoomExplanations
                    .AsNoTracking()
                    .Include(s => s.SchoolClassRoom)
                    .Where(s => s.SchoolClassRoomId == schoolClassRoomId)
                    .ToListAsync();
            }
        }

        // GET: api/SchoolClassRoomExplanations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolClassRoomExplanation>> GetSchoolClassRoomExplanation(int id)
        {
            var schoolClassRoomExplanation = await _context.SchoolClassRoomExplanations
                                                            .Include(s => s.SchoolClassRoom)
                                                            .Where(s => s.SchoolClassRoomExplanationId ==id)
                                                            .SingleOrDefaultAsync();

            if (schoolClassRoomExplanation == null)
            {
                return NotFound();
            }

            return schoolClassRoomExplanation;
        }

        // PUT: api/SchoolClassRoomExplanations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchoolClassRoomExplanation(int id, SchoolClassRoomExplanation schoolClassRoomExplanation)
        {
            if (id != schoolClassRoomExplanation.SchoolClassRoomExplanationId)
            {
                return BadRequest();
            }

            _context.Entry(schoolClassRoomExplanation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassRoomExplanationExists(id))
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

        // POST: api/SchoolClassRoomExplanations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SchoolClassRoomExplanation>> PostSchoolClassRoomExplanation(SchoolClassRoomExplanation schoolClassRoomExplanation)
        {
            _context.SchoolClassRoomExplanations.Add(schoolClassRoomExplanation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchoolClassRoomExplanation", new { id = schoolClassRoomExplanation.SchoolClassRoomExplanationId }, schoolClassRoomExplanation);
        }

        // DELETE: api/SchoolClassRoomExplanations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SchoolClassRoomExplanation>> DeleteSchoolClassRoomExplanation(int id)
        {
            var schoolClassRoomExplanation = await _context.SchoolClassRoomExplanations.FindAsync(id);
            if (schoolClassRoomExplanation == null)
            {
                return NotFound();
            }

            _context.SchoolClassRoomExplanations.Remove(schoolClassRoomExplanation);
            await _context.SaveChangesAsync();

            return schoolClassRoomExplanation;
        }

        private bool SchoolClassRoomExplanationExists(int id)
        {
            return _context.SchoolClassRoomExplanations.Any(e => e.SchoolClassRoomExplanationId == id);
        }
    }
}
