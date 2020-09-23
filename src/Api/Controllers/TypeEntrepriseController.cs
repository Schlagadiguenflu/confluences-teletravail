/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table TypeEntreprises
 * Fichier : TypeEntrepriseController.cs
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
    public class TypeEntrepriseController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeEntrepriseController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeEntreprise
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeEntrepris>>> GetTypeEntreprises()
        {
            return await _context.TypeEntreprises.AsNoTracking().OrderBy(s => s.Nom).ToListAsync();
        }

        // GET: api/TypeEntreprise/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeEntrepris>> GetTypeEntrepris(int id)
        {
            var typeEntrepris = await _context.TypeEntreprises
                .Include(t => t.Entrepris)
                .Where(t => t.TypeEntrepriseId == id)
                .SingleOrDefaultAsync();

            if (typeEntrepris == null)
            {
                return NotFound();
            }

            return typeEntrepris;
        }

        // PUT: api/TypeEntreprise/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeEntrepris(int id, TypeEntrepris typeEntrepris)
        {
            if (id != typeEntrepris.TypeEntrepriseId)
            {
                return BadRequest();
            }

            _context.Entry(typeEntrepris).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeEntreprisExists(id))
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

        // POST: api/TypeEntreprise
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeEntrepris>> PostTypeEntrepris(TypeEntrepris typeEntrepris)
        {
            _context.TypeEntreprises.Add(typeEntrepris);

            if (TypeEntrepriseUniqueExists(typeEntrepris.Nom))
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeEntrepris", new { id = typeEntrepris.TypeEntrepriseId }, typeEntrepris);
        }

        // DELETE: api/TypeEntreprise/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeEntrepris>> DeleteTypeEntrepris(int id)
        {
            var typeEntrepris = await _context.TypeEntreprises.FindAsync(id);
            if (typeEntrepris == null)
            {
                return NotFound();
            }

            _context.TypeEntreprises.Remove(typeEntrepris);
            await _context.SaveChangesAsync();

            return typeEntrepris;
        }

        private bool TypeEntreprisExists(int id)
        {
            return _context.TypeEntreprises.Any(e => e.TypeEntrepriseId == id);
        }

        private bool TypeEntrepriseUniqueExists(string Nom)
        {
            return _context.TypeEntreprises.Any(e => e.Nom == Nom);
        }
    }
}
