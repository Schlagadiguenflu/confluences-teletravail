using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public GendersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Genders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGenders()
        {
            return await _context.Genders.AsNoTracking().ToListAsync();
        }

        // GET: api/Genders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gender>> GetGender(short id)
        {
            var gender = await _context.Genders.FindAsync(id);

            if (gender == null)
            {
                return NotFound();
            }

            return gender;
        }

        //// PUT: api/Genders/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGender(short id, Gender gender)
        //{
        //    if (id != gender.GenderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(gender).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GenderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Genders
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Gender>> PostGender(Gender gender)
        //{
        //    _context.Genders.Add(gender);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGender", new { id = gender.GenderId }, gender);
        //}

        //// DELETE: api/Genders/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Gender>> DeleteGender(short id)
        //{
        //    var gender = await _context.Genders.FindAsync(id);
        //    if (gender == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Genders.Remove(gender);
        //    await _context.SaveChangesAsync();

        //    return gender;
        //}

        private bool GenderExists(short id)
        {
            return _context.Genders.Any(e => e.GenderId == id);
        }
    }
}
