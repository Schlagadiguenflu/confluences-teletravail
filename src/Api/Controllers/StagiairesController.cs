using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Api.ViewModel;

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class StagiairesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public StagiairesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Stagiaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StagiaireInfo>>> GetStagiaires()
        {
            var aspnetusers = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.TypeAffiliation)
                            .Include(a => a.StageStagiaires)
                            .Where(a => a.StageStagiaires.Count() > 0)
                            .OrderBy(a => a.Firstname)
                            .Select(v => new StagiaireInfo { StagiaireId = v.Id, Prenom = v.Firstname, Nom = v.LastName, TypeAffiliation = v.TypeAffiliation, StageStagiaires = v.StageStagiaires, TypeAffiliationId = v.TypeAffiliation.TypeAffiliationId })
                            .ToListAsync();

            return aspnetusers;
        }

        // GET: api/Stagiaires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StagiaireInfo>> GetStagiaire(string id)
        {
            var aspNetUser = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.TypeAffiliation)
                            .Include(a => a.StageStagiaires)
                                .ThenInclude(a => a.Entreprise)
                            .Include(a => a.StageStagiaires)
                                .ThenInclude(a => a.TypeMetier)
                            .Where(a => a.StageStagiaires.Count() > 0 && a.Id == id)
                            .Select(v => new StagiaireInfo { StagiaireId = v.Id, Prenom = v.Firstname, Nom = v.LastName, TypeAffiliation = v.TypeAffiliation, StageStagiaires = v.StageStagiaires, TypeAffiliationId = v.TypeAffiliation.TypeAffiliationId })
                            .SingleOrDefaultAsync();

            if (aspNetUser.StageStagiaires != null)
            {
                aspNetUser.StageStagiaires = aspNetUser.StageStagiaires.OrderByDescending(s => s.Debut).ToList();
            }

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return aspNetUser;
        }

        // PUT: api/Stagiaires/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStagiaire(string id, StagiaireInfo stagiaire)
        {
            if (id != stagiaire.StagiaireId)
            {
                return BadRequest();
            }

            AspNetUser aspnetUser = _context.AspNetUsers.Where(a => a.Id == id).SingleOrDefault();

            aspnetUser.Firstname = stagiaire.Prenom;
            aspnetUser.LastName = stagiaire.Nom;
            if (stagiaire.TypeAffiliationId == 0)
            {
                aspnetUser.TypeAffiliationId = null;
            }
            else
            {
                aspnetUser.TypeAffiliationId = stagiaire.TypeAffiliationId;
            }


            _context.Entry(aspnetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
