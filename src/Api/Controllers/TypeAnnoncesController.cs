/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table TypeAnnonces
 * Fichier : TypeAnnoncesController.cs
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
    public class TypeAnnoncesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeAnnoncesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeAnnonces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeAnnonce>>> GetTypeAnnonces()
        {
            return await _context.TypeAnnonces.AsNoTracking().OrderBy(s => s.Libelle).ToListAsync();
        }

        // GET: api/TypeAnnonces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeAnnonce>> GetTypeAnnonce(int id)
        {
            var typeAnnonce = await _context.TypeAnnonces
                .Include(t => t.Stages)
                    .ThenInclude(t => t.Stagiaire)
                .Where(t => t.TypeAnnonceId == id)
                .SingleOrDefaultAsync();

            if (typeAnnonce == null)
            {
                return NotFound();
            }

            return typeAnnonce;
        }

        // PUT: api/TypeAnnonces/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeAnnonce(int id, TypeAnnonce typeAnnonce)
        {
            if (id != typeAnnonce.TypeAnnonceId)
            {
                return BadRequest();
            }

            _context.Entry(typeAnnonce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeAnnonceExists(id))
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
                if (TypeAnnonceUniqueExists(typeAnnonce.Libelle))
                {
                    return Conflict();
                }
            }

            return NoContent();
        }

        // POST: api/TypeAnnonces
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeAnnonce>> PostTypeAnnonce(TypeAnnonce typeAnnonce)
        {
            _context.TypeAnnonces.Add(typeAnnonce);

            if (TypeAnnonceUniqueExists(typeAnnonce.Libelle))
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeAnnonce", new { id = typeAnnonce.TypeAnnonceId }, typeAnnonce);
        }

        // DELETE: api/TypeAnnonces/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeAnnonce>> DeleteTypeAnnonce(int id)
        {
            var typeAnnonce = await _context.TypeAnnonces.FindAsync(id);
            if (typeAnnonce == null)
            {
                return NotFound();
            }

            _context.TypeAnnonces.Remove(typeAnnonce);
            await _context.SaveChangesAsync();

            return typeAnnonce;
        }

        private bool TypeAnnonceExists(int id)
        {
            return _context.TypeAnnonces.Any(e => e.TypeAnnonceId == id);
        }

        private bool TypeAnnonceUniqueExists(string libelle)
        {
            return _context.TypeAnnonces.Any(e => e.Libelle == libelle);
        }
    }
}
