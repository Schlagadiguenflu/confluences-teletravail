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
    public class EntrepriseDomainesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public EntrepriseDomainesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/EntrepriseDomaines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntrepriseDomaine>>> GetEntrepriseDomaines()
        {
            return await _context.EntrepriseDomaines.ToListAsync();
        }

        // GET: api/EntrepriseDomaines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntrepriseDomaine>> GetEntrepriseDomaine(int id)
        {
            var entrepriseDomaine = await _context.EntrepriseDomaines.FindAsync(id);

            if (entrepriseDomaine == null)
            {
                return NotFound();
            }

            return entrepriseDomaine;
        }

        // PUT: api/EntrepriseDomaines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrepriseDomaine(int id, EntrepriseDomaine entrepriseDomaine)
        {
            if (id != entrepriseDomaine.EntrepriseId)
            {
                return BadRequest();
            }

            _context.Entry(entrepriseDomaine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntrepriseDomaineExists(id))
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

        // POST: api/EntrepriseDomaines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EntrepriseDomaine>> PostEntrepriseDomaine(EntrepriseDomaine entrepriseDomaine)
        {
            _context.EntrepriseDomaines.Add(entrepriseDomaine);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntrepriseDomaineExists(entrepriseDomaine.EntrepriseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEntrepriseDomaine", new { id = entrepriseDomaine.EntrepriseId }, entrepriseDomaine);
        }

        // DELETE: api/EntrepriseDomaines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EntrepriseDomaine>> DeleteEntrepriseDomaine(int id)
        {
            var entrepriseDomaine = await _context.EntrepriseDomaines.FindAsync(id);
            if (entrepriseDomaine == null)
            {
                return NotFound();
            }

            _context.EntrepriseDomaines.Remove(entrepriseDomaine);
            await _context.SaveChangesAsync();

            return entrepriseDomaine;
        }

        private bool EntrepriseDomaineExists(int id)
        {
            return _context.EntrepriseDomaines.Any(e => e.EntrepriseId == id);
        }
    }
}
