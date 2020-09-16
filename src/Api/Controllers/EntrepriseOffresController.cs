/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table EntrepriseOffres
 * Fichier : EntrepriseOffresController.cs
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
    public class EntrepriseOffresController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public EntrepriseOffresController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/EntrepriseOffres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntrepriseOffre>>> GetEntrepriseOffres()
        {
            return await _context.EntrepriseOffres.ToListAsync();
        }

        // GET: api/EntrepriseOffres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntrepriseOffre>> GetEntrepriseOffre(int id)
        {
            var entrepriseOffre = await _context.EntrepriseOffres.FindAsync(id);

            if (entrepriseOffre == null)
            {
                return NotFound();
            }

            return entrepriseOffre;
        }

        // PUT: api/EntrepriseOffres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrepriseOffre(int id, EntrepriseOffre entrepriseOffre)
        {
            if (id != entrepriseOffre.EntrepriseId)
            {
                return BadRequest();
            }

            _context.Entry(entrepriseOffre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntrepriseOffreExists(id))
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

        // POST: api/EntrepriseOffres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EntrepriseOffre>> PostEntrepriseOffre(EntrepriseOffre entrepriseOffre)
        {
            entrepriseOffre.TypeOffre = null;
            _context.EntrepriseOffres.Add(entrepriseOffre);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntrepriseOffreExists(entrepriseOffre.EntrepriseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEntrepriseOffre", new { id = entrepriseOffre.EntrepriseId }, entrepriseOffre);
        }

        // DELETE: api/EntrepriseOffres/5
        [HttpDelete("{entrepriseId}/{offreId}")]
        public async Task<ActionResult<EntrepriseOffre>> DeleteEntrepriseOffre(int entrepriseId, int offreId)
        {
            var entrepriseOffre = await _context.EntrepriseOffres.Where(e => e.EntrepriseId == entrepriseId && e.TypeOffreId == offreId).SingleOrDefaultAsync();
            if (entrepriseOffre == null)
            {
                return NotFound();
            }

            _context.EntrepriseOffres.Remove(entrepriseOffre);
            await _context.SaveChangesAsync();

            return entrepriseOffre;
        }

        private bool EntrepriseOffreExists(int id)
        {
            return _context.EntrepriseOffres.Any(e => e.EntrepriseId == id);
        }
    }
}
