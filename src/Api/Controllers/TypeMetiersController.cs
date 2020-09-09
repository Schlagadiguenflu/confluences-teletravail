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
    public class TypeMetiersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeMetiersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeMetiers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeMetier>>> GetTypeMetiers()
        {
            return await _context.TypeMetiers.AsNoTracking().OrderBy(s => s.Libelle).ToListAsync();
        }

        // GET: api/TypeMetiers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeMetier>> GetTypeMetier(int id)
        {
            var typeMetier = await _context.TypeMetiers.FindAsync(id);

            if (typeMetier == null)
            {
                return NotFound();
            }

            return typeMetier;
        }

        // PUT: api/TypeMetiers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeMetier(int id, TypeMetier typeMetier)
        {
            if (id != typeMetier.TypeMetierId)
            {
                return BadRequest();
            }

            _context.Entry(typeMetier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeMetierExists(id))
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

        // POST: api/TypeMetiers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeMetier>> PostTypeMetier(TypeMetier typeMetier)
        {
            _context.TypeMetiers.Add(typeMetier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeMetier", new { id = typeMetier.TypeMetierId }, typeMetier);
        }

        // DELETE: api/TypeMetiers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeMetier>> DeleteTypeMetier(int id)
        {
            var typeMetier = await _context.TypeMetiers.FindAsync(id);
            if (typeMetier == null)
            {
                return NotFound();
            }

            _context.TypeMetiers.Remove(typeMetier);
            await _context.SaveChangesAsync();

            return typeMetier;
        }

        private bool TypeMetierExists(int id)
        {
            return _context.TypeMetiers.Any(e => e.TypeMetierId == id);
        }
    }
}
