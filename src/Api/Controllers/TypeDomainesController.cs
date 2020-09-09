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
    public class TypeDomainesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeDomainesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeDomaines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeDomaine>>> GetTypeDomaines()
        {
            return await _context.TypeDomaines.AsNoTracking().OrderBy(s => s.Libelle).ToListAsync();
        }

        // GET: api/TypeDomaines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeDomaine>> GetTypeDomaine(int id)
        {
            var typeDomaine = await _context.TypeDomaines.FindAsync(id);

            if (typeDomaine == null)
            {
                return NotFound();
            }

            return typeDomaine;
        }

        // PUT: api/TypeDomaines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeDomaine(int id, TypeDomaine typeDomaine)
        {
            if (id != typeDomaine.TypeDomaineId)
            {
                return BadRequest();
            }

            _context.Entry(typeDomaine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeDomaineExists(id))
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

        // POST: api/TypeDomaines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeDomaine>> PostTypeDomaine(TypeDomaine typeDomaine)
        {
            _context.TypeDomaines.Add(typeDomaine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeDomaine", new { id = typeDomaine.TypeDomaineId }, typeDomaine);
        }

        // DELETE: api/TypeDomaines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeDomaine>> DeleteTypeDomaine(int id)
        {
            var typeDomaine = await _context.TypeDomaines.FindAsync(id);
            if (typeDomaine == null)
            {
                return NotFound();
            }

            _context.TypeDomaines.Remove(typeDomaine);
            await _context.SaveChangesAsync();

            return typeDomaine;
        }

        private bool TypeDomaineExists(int id)
        {
            return _context.TypeDomaines.Any(e => e.TypeDomaineId == id);
        }
    }
}
