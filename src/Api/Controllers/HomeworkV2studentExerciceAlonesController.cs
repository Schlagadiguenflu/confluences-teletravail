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
    public class HomeworkV2studentExerciceAlonesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public HomeworkV2studentExerciceAlonesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/HomeworkV2studentExerciceAlones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeworkV2studentExerciceAlones>>> GetHomeworkV2studentExerciceAlones()
        {
            return await _context.HomeworkV2studentExerciceAlones.ToListAsync();
        }

        // GET: api/HomeworkV2studentExerciceAlones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkV2studentExerciceAlones>> GetHomeworkV2studentExerciceAlones(int id)
        {
            var homeworkV2studentExerciceAlones = await _context.HomeworkV2studentExerciceAlones.FindAsync(id);

            if (homeworkV2studentExerciceAlones == null)
            {
                return NotFound();
            }

            return homeworkV2studentExerciceAlones;
        }

        // PUT: api/HomeworkV2studentExerciceAlones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomeworkV2studentExerciceAlones(int id, HomeworkV2studentExerciceAlones homeworkV2studentExerciceAlones)
        {
            if (id != homeworkV2studentExerciceAlones.HomeworkV2studentId)
            {
                return BadRequest();
            }

            _context.Entry(homeworkV2studentExerciceAlones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkV2studentExerciceAlonesExists(id))
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

        // POST: api/HomeworkV2studentExerciceAlones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HomeworkV2studentExerciceAlones>> PostHomeworkV2studentExerciceAlones(HomeworkV2studentExerciceAlones homeworkV2studentExerciceAlones)
        {
            _context.HomeworkV2studentExerciceAlones.Add(homeworkV2studentExerciceAlones);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomeworkV2studentExerciceAlones", new { id = homeworkV2studentExerciceAlones.HomeworkV2studentId }, homeworkV2studentExerciceAlones);
        }

        // DELETE: api/HomeworkV2studentExerciceAlones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HomeworkV2studentExerciceAlones>> DeleteHomeworkV2studentExerciceAlones(int id)
        {
            var homeworkV2studentExerciceAlones = await _context.HomeworkV2studentExerciceAlones.FindAsync(id);
            if (homeworkV2studentExerciceAlones == null)
            {
                return NotFound();
            }

            _context.HomeworkV2studentExerciceAlones.Remove(homeworkV2studentExerciceAlones);
            await _context.SaveChangesAsync();

            return homeworkV2studentExerciceAlones;
        }

        private bool HomeworkV2studentExerciceAlonesExists(int id)
        {
            return _context.HomeworkV2studentExerciceAlones.Any(e => e.HomeworkV2studentId == id);
        }
    }
}
