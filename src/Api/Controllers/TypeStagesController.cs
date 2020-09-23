/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table TypeStages
 * Fichier : TypeStagesController.cs
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
    public class TypeStagesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeStagesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeStages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeStage>>> GetTypeStages()
        {
            return await _context.TypeStages.AsNoTracking().OrderBy(s => s.Nom).ToListAsync();
        }

        // GET: api/TypeStages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeStage>> GetTypeStage(int id)
        {
            var typeStage = await _context.TypeStages
                .Include(t => t.Stages)
                    .ThenInclude(t => t.Stagiaire)
                .Where(t => t.TypeStageId == id)
                .SingleOrDefaultAsync();

            if (typeStage == null)
            {
                return NotFound();
            }

            return typeStage;
        }

        // PUT: api/TypeStages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeStage(int id, TypeStage typeStage)
        {
            if (id != typeStage.TypeStageId)
            {
                return BadRequest();
            }

            _context.Entry(typeStage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeStageExists(id))
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
                if (TypeStageUniqueExists(typeStage.Nom))
                {
                    return Conflict();
                }
            }

            return NoContent();
        }

        // POST: api/TypeStages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeStage>> PostTypeStage(TypeStage typeStage)
        {
            _context.TypeStages.Add(typeStage);

            if (TypeStageUniqueExists(typeStage.Nom))
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeStage", new { id = typeStage.TypeStageId }, typeStage);
        }

        // DELETE: api/TypeStages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeStage>> DeleteTypeStage(int id)
        {
            var typeStage = await _context.TypeStages.FindAsync(id);
            if (typeStage == null)
            {
                return NotFound();
            }

            _context.TypeStages.Remove(typeStage);
            await _context.SaveChangesAsync();

            return typeStage;
        }

        private bool TypeStageExists(int id)
        {
            return _context.TypeStages.Any(e => e.TypeStageId == id);
        }

        private bool TypeStageUniqueExists(string Nom)
        {
            return _context.TypeStages.Any(e => e.Nom == Nom);
        }
    }
}
