/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table TypeAffiliations
 * Fichier : TypeAffiliationsController.cs
 **/

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
    public class TypeAffiliationsController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeAffiliationsController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeAffiliations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeAffiliation>>> GetTypeAffiliations()
        {
            return await _context.TypeAffiliations.AsNoTracking().OrderBy(s => s.Libelle).ToListAsync();
        }

        // GET: api/TypeAffiliations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeAffiliation>> GetTypeAffiliation(int id)
        {
            var typeAffiliation = await _context.TypeAffiliations.FindAsync(id);

            if (typeAffiliation == null)
            {
                return NotFound();
            }

            return typeAffiliation;
        }

        // PUT: api/TypeAffiliations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeAffiliation(int id, TypeAffiliation typeAffiliation)
        {
            if (id != typeAffiliation.TypeAffiliationId)
            {
                return BadRequest();
            }

            _context.Entry(typeAffiliation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeAffiliationExists(id))
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

        // POST: api/TypeAffiliations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeAffiliation>> PostTypeAffiliation(TypeAffiliation typeAffiliation)
        {
            _context.TypeAffiliations.Add(typeAffiliation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeAffiliation", new { id = typeAffiliation.TypeAffiliationId }, typeAffiliation);
        }

        // DELETE: api/TypeAffiliations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeAffiliation>> DeleteTypeAffiliation(int id)
        {
            var typeAffiliation = await _context.TypeAffiliations.FindAsync(id);
            if (typeAffiliation == null)
            {
                return NotFound();
            }

            _context.TypeAffiliations.Remove(typeAffiliation);
            await _context.SaveChangesAsync();

            return typeAffiliation;
        }

        private bool TypeAffiliationExists(int id)
        {
            return _context.TypeAffiliations.Any(e => e.TypeAffiliationId == id);
        }
    }
}
