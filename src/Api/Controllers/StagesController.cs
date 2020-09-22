/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant le CRUD sur la table Stages
 * Fichier : StagesController.cs
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
    public class StagesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public StagesController(ConfluencesContext context)
        {
            _context = context;
        }

        // Modèle du filtre entreprise
        public class Filter
        {
            public string nom { get; set; }
            public int? typeMetierId { get; set; }
            public int? entrepriseId { get; set; }
            public string stagiaireId { get; set; }
            public DateTime? dateDebut { get; set; }
            public DateTime? dateFin { get; set; }
            public int? typeStageId { get; set; }
            public int? typeAnnonceId { get; set; }

        }

        // GET: api/Stages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stage>>> GetStages([FromQuery] Filter filter)
        {
            var stages = await _context.Stages
                                        .Include(s => s.TypeMetier)
                                        .Include(s => s.Entreprise)
                                        .Include(s => s.Stagiaire)
                                        .Include(s => s.TypeAnnonce)
                                        .Include(s => s.TypeStage)
                                        .AsNoTracking()
                                        .OrderByDescending(s => s.Debut)
                                        .ToListAsync();

            if (filter.nom != null)
            {
                stages = stages.Where(e => e.Nom.ToUpper().Contains(filter.nom.ToUpper())).ToList();
            }

            if (filter.typeMetierId != null)
            {
                stages = stages.Where(e => e.TypeMetierId == filter.typeMetierId).ToList();
            }

            if (filter.entrepriseId != null)
            {
                stages = stages.Where(e => e.EntrepriseId == filter.entrepriseId).ToList();
            }

            if (filter.stagiaireId != null)
            {
                stages = stages.Where(e => e.StagiaireId == filter.stagiaireId).ToList();
            }

            if (filter.dateDebut != null)
            {
                stages = stages.Where(e => e.Debut >= filter.dateDebut).ToList();
            }

            if (filter.dateFin != null)
            {
                stages = stages.Where(e => e.Fin <= filter.dateFin).ToList();
            }

            if (filter.typeStageId != null)
            {
                stages = stages.Where(e => e.TypeStageId == filter.typeStageId).ToList();
            }

            if (filter.typeAnnonceId != null)
            {
                stages = stages.Where(e => e.TypeAnnonceId == filter.typeAnnonceId).ToList();
            }

            return stages;
        }

        // GET: api/Stages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stage>> GetStage(int id)
        {
            var stage = await _context.Stages.FindAsync(id);

            if (stage == null)
            {
                return NotFound();
            }

            return stage;
        }

        // PUT: api/Stages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStage(int id, Stage stage)
        {
            if (id != stage.StageId)
            {
                return BadRequest();
            }

            _context.Entry(stage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StageExists(id))
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

        // POST: api/Stages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Stage>> PostStage(Stage stage)
        {
            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStage", new { id = stage.StageId }, stage);
        }

        // DELETE: api/Stages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stage>> DeleteStage(int id)
        {
            var stage = await _context.Stages.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();

            return stage;
        }

        private bool StageExists(int id)
        {
            return _context.Stages.Any(e => e.StageId == id);
        }
    }
}
