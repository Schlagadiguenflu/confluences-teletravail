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
            var typeOffre = await _context.TypeOffres.FindAsync(id);

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

            return NoContent();
        }

        // POST: api/TypeOffres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeOffre>> PostTypeOffre(TypeOffre typeOffre)
        {
            _context.TypeOffres.Add(typeOffre);
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
    }
}
