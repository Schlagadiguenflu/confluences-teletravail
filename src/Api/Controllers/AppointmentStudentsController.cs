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
    public class AppointmentStudentsController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public AppointmentStudentsController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentStudent>>> GetAppointmentStudents()
        {
            return await _context.AppointmentStudents
                                    .AsNoTracking()
                                    .Include(a => a.Appointment)
                                    .Include(a => a.Student)
                                    .ToListAsync();
        }

        // GET: api/AppointmentStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentStudent>> GetAppointmentStudent(int id)
        {
            var appointmentStudent = await _context.AppointmentStudents
                                                        .Include(a => a.Appointment)
                                                        .Include(a => a.Student)
                                                        .Where(a => a.AppointmentStudentId == id)
                                                        .SingleOrDefaultAsync();

            if (appointmentStudent == null)
            {
                return NotFound();
            }

            return appointmentStudent;
        }

        // PUT: api/AppointmentStudents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentStudent(int id, AppointmentStudent appointmentStudent)
        {
            if (id != appointmentStudent.AppointmentStudentId)
            {
                return BadRequest();
            }

            _context.Entry(appointmentStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentStudentExists(id))
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

        // POST: api/AppointmentStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AppointmentStudent>> PostAppointmentStudent(AppointmentStudent appointmentStudent)
        {
            _context.AppointmentStudents.Add(appointmentStudent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentStudent", new { id = appointmentStudent.AppointmentStudentId }, appointmentStudent);
        }

        // DELETE: api/AppointmentStudents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppointmentStudent>> DeleteAppointmentStudent(int id)
        {
            var appointmentStudent = await _context.AppointmentStudents.FindAsync(id);
            if (appointmentStudent == null)
            {
                return NotFound();
            }

            _context.AppointmentStudents.Remove(appointmentStudent);
            await _context.SaveChangesAsync();

            return appointmentStudent;
        }

        private bool AppointmentStudentExists(int id)
        {
            return _context.AppointmentStudents.Any(e => e.AppointmentStudentId == id);
        }
    }
}
