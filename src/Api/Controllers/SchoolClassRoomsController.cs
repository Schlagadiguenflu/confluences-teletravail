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
    public class SchoolClassRoomsController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public SchoolClassRoomsController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/SchoolClassRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClassRoom>>> GetSchoolClassRooms()
        {
            return await _context.SchoolClassRooms.AsNoTracking().ToListAsync();
        }

        // GET: api/SchoolClassRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolClassRoom>> GetSchoolClassRoom(int id)
        {
            var schoolClassRoom = await _context.SchoolClassRooms
                                            .Where(s => s.SchoolClassRoomId == id)
                                            .SingleOrDefaultAsync();

            if (schoolClassRoom == null)
            {
                return NotFound();
            }

            var sessions = await _context.Sessions
                                .Include(s => s.SessionStudents)
                                        .ThenInclude(s => s.Student)
                                .Include(s => s.SessionTeachers)
                                        .ThenInclude(s => s.Teacher)
                                .Where(s => s.SchoolClassRoomId == id)
                                .OrderByDescending(s => s.DateStart)
                                .Take(1)
                                .ToListAsync();

            schoolClassRoom.Sessions = sessions;

            return schoolClassRoom;
        }

        [Authorize(Policy = "Teacher")]
        // PUT: api/SchoolClassRooms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchoolClassRoom(int id, SchoolClassRoom schoolClassRoom)
        {
            if (id != schoolClassRoom.SchoolClassRoomId)
            {
                return BadRequest();
            }

            _context.Entry(schoolClassRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassRoomExists(id))
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
        // POST: api/SchoolClassRooms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SchoolClassRoom>> PostSchoolClassRoom(SchoolClassRoom schoolClassRoom)
        {
            _context.SchoolClassRooms.Add(schoolClassRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchoolClassRoom", new { id = schoolClassRoom.SchoolClassRoomId }, schoolClassRoom);
        }

        [Authorize(Policy = "Teacher")]
        // DELETE: api/SchoolClassRooms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SchoolClassRoom>> DeleteSchoolClassRoom(int id)
        {
            var schoolClassRoom = await _context.SchoolClassRooms.FindAsync(id);
            if (schoolClassRoom == null)
            {
                return NotFound();
            }

            _context.SchoolClassRooms.Remove(schoolClassRoom);
            await _context.SaveChangesAsync();

            return schoolClassRoom;
        }

        private bool SchoolClassRoomExists(int id)
        {
            return _context.SchoolClassRooms.Any(e => e.SchoolClassRoomId == id);
        }
    }
}
