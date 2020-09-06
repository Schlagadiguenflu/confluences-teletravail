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
    public class EntrepriseMetiersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public EntrepriseMetiersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/EntrepriseMetiers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntrepriseMetier>>> GetEntrepriseMetiers()
        {
            return await _context.EntrepriseMetiers.ToListAsync();
        }

        // GET: api/EntrepriseMetiers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntrepriseMetier>> GetEntrepriseMetier(int id)
        {
            var entrepriseMetier = await _context.EntrepriseMetiers.FindAsync(id);

            if (entrepriseMetier == null)
            {
                return NotFound();
            }

            return entrepriseMetier;
        }

        // PUT: api/EntrepriseMetiers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrepriseMetier(int id, EntrepriseMetier entrepriseMetier)
        {
            if (id != entrepriseMetier.EntrepriseId)
            {
                return BadRequest();
            }

            _context.Entry(entrepriseMetier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntrepriseMetierExists(id))
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

        // POST: api/EntrepriseMetiers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EntrepriseMetier>> PostEntrepriseMetier(EntrepriseMetier entrepriseMetier)
        {
            entrepriseMetier.TypeMetier = null;
            _context.EntrepriseMetiers.Add(entrepriseMetier);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntrepriseMetierExists(entrepriseMetier.EntrepriseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEntrepriseMetier", new { id = entrepriseMetier.EntrepriseId }, entrepriseMetier);
        }

        // DELETE: api/EntrepriseMetiers/5
        [HttpDelete("{entrepriseId}/{metierId}")]
        public async Task<ActionResult<EntrepriseMetier>> DeleteEntrepriseMetier(int entrepriseId, int metierId)
        {
            var entrepriseMetier = await _context.EntrepriseMetiers.Where(e => e.EntrepriseId == entrepriseId && e.TypeMetierId == metierId).SingleOrDefaultAsync();
            if (entrepriseMetier == null)
            {
                return NotFound();
            }

            _context.EntrepriseMetiers.Remove(entrepriseMetier);
            await _context.SaveChangesAsync();

            return entrepriseMetier;
        }

        private bool EntrepriseMetierExists(int id)
        {
            return _context.EntrepriseMetiers.Any(e => e.EntrepriseId == id);
        }
    }
}
