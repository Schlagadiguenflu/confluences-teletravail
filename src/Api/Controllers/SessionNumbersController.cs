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
    public class SessionNumbersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public SessionNumbersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/SessionNumbers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionNumber>>> GetSessionNumbers()
        {
            return await _context.SessionNumbers.AsNoTracking().ToListAsync();
        }

        // GET: api/SessionNumbers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionNumber>> GetSessionNumber(int id)
        {
            var sessionNumber = await _context.SessionNumbers.FindAsync(id);

            if (sessionNumber == null)
            {
                return NotFound();
            }

            return sessionNumber;
        }

        // PUT: api/SessionNumbers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionNumber(int id, SessionNumber sessionNumber)
        {
            if (id != sessionNumber.SessionNumberId)
            {
                return BadRequest();
            }

            _context.Entry(sessionNumber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionNumberExists(id))
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

        // POST: api/SessionNumbers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionNumber>> PostSessionNumber(SessionNumber sessionNumber)
        {
            _context.SessionNumbers.Add(sessionNumber);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSessionNumber", new { id = sessionNumber.SessionNumberId }, sessionNumber);
        }

        // DELETE: api/SessionNumbers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SessionNumber>> DeleteSessionNumber(int id)
        {
            var sessionNumber = await _context.SessionNumbers.FindAsync(id);
            if (sessionNumber == null)
            {
                return NotFound();
            }

            _context.SessionNumbers.Remove(sessionNumber);
            await _context.SaveChangesAsync();

            return sessionNumber;
        }

        private bool SessionNumberExists(int id)
        {
            return _context.SessionNumbers.Any(e => e.SessionNumberId == id);
        }
    }
}
