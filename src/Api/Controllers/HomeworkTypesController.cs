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
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkTypesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public HomeworkTypesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/HomeworkTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeworkType>>> GetHomeworkTypes()
        {
            return await _context.HomeworkTypes.AsNoTracking().OrderBy(h => h.HomeworkTypeName).ToListAsync();
        }

        // GET: api/HomeworkTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkType>> GetHomeworkType(int id)
        {
            var homeworkType = await _context.HomeworkTypes.FindAsync(id);

            if (homeworkType == null)
            {
                return NotFound();
            }

            return homeworkType;
        }

        [Authorize(Policy = "Teacher")]
        // PUT: api/HomeworkTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomeworkType(int id, HomeworkType homeworkType)
        {
            if (id != homeworkType.HomeworkTypeId)
            {
                return BadRequest();
            }

            _context.Entry(homeworkType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkTypeExists(id))
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

        [Authorize(Policy = "Teacher")]
        // POST: api/HomeworkTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HomeworkType>> PostHomeworkType(HomeworkType homeworkType)
        {
            _context.HomeworkTypes.Add(homeworkType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomeworkType", new { id = homeworkType.HomeworkTypeId }, homeworkType);
        }

        [Authorize(Policy = "Teacher")]
        // DELETE: api/HomeworkTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HomeworkType>> DeleteHomeworkType(int id)
        {
            var homeworkType = await _context.HomeworkTypes.FindAsync(id);
            if (homeworkType == null)
            {
                return NotFound();
            }

            _context.HomeworkTypes.Remove(homeworkType);
            await _context.SaveChangesAsync();

            return homeworkType;
        }

        private bool HomeworkTypeExists(int id)
        {
            return _context.HomeworkTypes.Any(e => e.HomeworkTypeId == id);
        }
    }
}
