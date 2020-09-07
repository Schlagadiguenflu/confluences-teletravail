﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class EntrepriseController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public EntrepriseController(ConfluencesContext context)
        {
            _context = context;
        }

        public class Filter
        {
            public List<int?> domaines { get; set; }
            public List<int?> metiers { get; set; }
            public List<int?> offres { get; set; }
            public string codePostal { get; set; }
            public string nom { get; set; }
            public DateTime? dateModification { get; set; }
            public string createur { get; set; }
        }

        // GET: api/Entreprise
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrepris>>> GetEntreprises([FromQuery] Filter filter)
        {
            var entreprises = await _context.Entreprises
                                .Include(e => e.TypeDomaine)
                                .Include(e => e.TypeMoyen)
                                .Include(e => e.TypeEntreprise)
                                .Include(e => e.EntrepriseOffres)
                                    .ThenInclude(e => e.TypeOffre)
                                .Include(e => e.EntrepriseMetiers)
                                    .ThenInclude(e => e.TypeMetier)
                                .Include(e => e.Stages)
                                    .ThenInclude(s => s.Stagiaire)
                                .Include(e => e.Stages)
                                    .ThenInclude(s => s.TypeMetier)
                                .Include(s => s.Contacts)
                                .AsNoTracking()
                                .ToListAsync();

            if(filter.domaines != null) {
                entreprises = entreprises.Where(e => filter.domaines.Contains(e.TypeDomaineId)).ToList();
            }
            if (filter.metiers != null)
            {
                entreprises = entreprises.Where(e => e.EntrepriseMetiers.Any(x => filter.metiers.Contains(x.TypeMetierId))).ToList();
            }
            if (filter.offres != null)
            {
                entreprises = entreprises.Where(e => e.EntrepriseOffres.Any(x => filter.offres.Contains(x.TypeOffreId))).ToList();
            }
            if (filter.codePostal != null)
            {
                entreprises = entreprises.Where(e => e.CodePostal == filter.codePostal).ToList();
            }
            if (filter.nom != null)
            {
                entreprises = entreprises.Where(e => e.Nom.ToUpper().Contains(filter.nom.ToUpper())).ToList();
            }
            if (filter.dateModification != null)
            {
                entreprises = entreprises.Where(e => e.DateCreation > filter.dateModification).ToList();
            }
            if (filter.createur != null)
            {
                entreprises = entreprises.Where(e => e.CreateurId == filter.createur).ToList();
            }
            return entreprises;
        }



        // GET: api/Entreprise/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrepris>> GetEntrepris(int id)
        {
            var entrepris = await _context.Entreprises
                                .Include(e => e.TypeDomaine)
                                .Include(e => e.TypeMoyen)
                                .Include(e => e.TypeEntreprise)
                                .Include(e => e.EntrepriseOffres)
                                    .ThenInclude(e => e.TypeOffre)
                                .Include(e => e.EntrepriseMetiers)
                                    .ThenInclude(e => e.TypeMetier)
                                .Include(e => e.Stages)
                                    .ThenInclude(s => s.Stagiaire)
                                .Include(e => e.Stages)
                                    .ThenInclude(s => s.TypeMetier)
                                .Include(s => s.Contacts)
                                .AsNoTracking()
                                .Where(e => e.EntrepriseId == id)
                                .SingleOrDefaultAsync();

            if (entrepris == null)
            {
                return NotFound();
            }

            entrepris.Stages = entrepris.Stages.OrderByDescending(s => s.Debut).ToList();

            return entrepris;
        }

        // PUT: api/Entreprise/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrepris(int id, Entrepris entrepris)
        {
            if (id != entrepris.EntrepriseId)
            {
                return BadRequest();
            }

            _context.Entry(entrepris).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntreprisExists(id))
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

        // POST: api/Entreprise
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Entrepris>> PostEntrepris(Entrepris entrepris)
        {
            _context.Entreprises.Add(entrepris);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntrepris", new { id = entrepris.EntrepriseId }, entrepris);
        }

        // DELETE: api/Entreprise/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Entrepris>> DeleteEntrepris(int id)
        {
            var entrepris = await _context.Entreprises.Include(e => e.Stages).Where(e => e.EntrepriseId == id).SingleOrDefaultAsync();
            if (entrepris == null)
            {
                return NotFound();
            }

            _context.Entreprises.Remove(entrepris);
            await _context.SaveChangesAsync();

            return entrepris;
        }

        private bool EntreprisExists(int id)
        {
            return _context.Entreprises.Any(e => e.EntrepriseId == id);
        }
    }
}
