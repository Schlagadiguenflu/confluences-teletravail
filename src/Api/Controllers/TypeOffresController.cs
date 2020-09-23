/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table TypeOffres
 * Fichier : TypeOffresController.cs
 **/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOffresController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeOffresController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeOffres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOffre>>> GetTypeOffres()
        {
            return await _context.TypeOffres.AsNoTracking().OrderBy(s => s.Libelle).ToListAsync();
        }

        // GET: api/TypeOffres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeOffre>> GetTypeOffre(int id)
        {
            var typeOffre = await _context.TypeOffres
                .Include(t => t.EntrepriseOffres)
                    .ThenInclude(t => t.Entreprise)
                .Where(t => t.TypeOffreId == id)
                .SingleOrDefaultAsync();

            if (typeOffre == null)
            {
                return NotFound();
            }

            return typeOffre;
        }

        // PUT: api/TypeOffres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeOffre(int id, TypeOffre typeOffre)
        {
            if (id != typeOffre.TypeOffreId)
            {
                return BadRequest();
            }

            _context.Entry(typeOffre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOffreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                if (TypeOffreUniqueExists(typeOffre.Libelle))
                {
                    return Conflict();
                }
            }

            return NoContent();
        }

        // POST: api/TypeOffres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeOffre>> PostTypeOffre(TypeOffre typeOffre)
        {
            _context.TypeOffres.Add(typeOffre);

            if (TypeOffreUniqueExists(typeOffre.Libelle))
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeOffre", new { id = typeOffre.TypeOffreId }, typeOffre);
        }

        // DELETE: api/TypeOffres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeOffre>> DeleteTypeOffre(int id)
        {
            var typeOffre = await _context.TypeOffres.FindAsync(id);
            if (typeOffre == null)
            {
                return NotFound();
            }

            _context.TypeOffres.Remove(typeOffre);
            await _context.SaveChangesAsync();

            return typeOffre;
        }

        private bool TypeOffreExists(int id)
        {
            return _context.TypeOffres.Any(e => e.TypeOffreId == id);
        }

        private bool TypeOffreUniqueExists(string libelle)
        {
            return _context.TypeOffres.Any(e => e.Libelle == libelle);
        }
    }
}
