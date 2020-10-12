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
    public class TypeMoyensController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeMoyensController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeMoyens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeMoyen>>> GetTypeMoyens()
        {
            return await _context.TypeMoyens.ToListAsync();
        }

        // GET: api/TypeMoyens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeMoyen>> GetTypeMoyen(int id)
        {
            var typeMoyen = await _context.TypeMoyens
                .Include(t => t.Entrepris)
                .Where(t => t.TypeMoyenId == id)
                .SingleOrDefaultAsync();

            if (typeMoyen == null)
            {
                return NotFound();
            }

            return typeMoyen;
        }

        // PUT: api/TypeMoyens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeMoyen(int id, TypeMoyen typeMoyen)
        {
            if (id != typeMoyen.TypeMoyenId)
            {
                return BadRequest();
            }

            _context.Entry(typeMoyen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeMoyenExists(id))
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
                if (TypeMoyenUniqueExists(typeMoyen.Libelle))
                {
                    return Conflict();
                }
            }

            return NoContent();
        }

        // POST: api/TypeMoyens
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeMoyen>> PostTypeMoyen(TypeMoyen typeMoyen)
        {
            _context.TypeMoyens.Add(typeMoyen);

            if (TypeMoyenUniqueExists(typeMoyen.Libelle))
            {
                return Conflict();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeMoyen", new { id = typeMoyen.TypeMoyenId }, typeMoyen);
        }

        // DELETE: api/TypeMoyens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeMoyen>> DeleteTypeMoyen(int id)
        {
            var typeMoyen = await _context.TypeMoyens.FindAsync(id);
            if (typeMoyen == null)
            {
                return NotFound();
            }

            _context.TypeMoyens.Remove(typeMoyen);
            await _context.SaveChangesAsync();

            return typeMoyen;
        }

        private bool TypeMoyenExists(int id)
        {
            return _context.TypeMoyens.Any(e => e.TypeMoyenId == id);
        }

        private bool TypeMoyenUniqueExists(string libelle)
        {
            return _context.TypeMoyens.Any(e => e.Libelle == libelle);
        }
    }
}
