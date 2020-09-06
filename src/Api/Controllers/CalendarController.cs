using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public CalendarController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Calendar
        [HttpGet]
        public async Task<IActionResult> GetAsync(DateTime start, DateTime end, string userId)
        {
            // Bloque les cours futurs
            if (end >= DateTime.Now)
            {
                end = DateTime.Now.AddDays(-1);
            }

            var sessionStudent = await _context.SessionStudents.AsNoTracking().Where(s => s.StudentId == userId).Select(s => s.SessionId).ToListAsync();
            var data = await _context.HomeworkV2s
                                .AsNoTracking()
                                .Include(h => h.HomeworkType)
                                .Where(h => h.HomeworkV2date.Date >= start.Date && h.HomeworkV2date.Date <= end.Date && sessionStudent.Contains(h.SessionId))
                                .ToListAsync();

            var listCalendar = data.
               Select(x => new
               {
                   id = x.HomeworkV2id,
                   title = x.HomeworkV2name,
                   start = x.HomeworkV2date.ToString("yyyy-MM-ddTHH:mm:ss"),
                   url = "/HomeworkV2s/Details/" + x.HomeworkV2id,
                   color = "#ffffff",    // an option!
                   textColor = "#02559e"
               }).ToList();

            return new JsonResult(listCalendar);
        }

        // GET api/<CalendarController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CalendarController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CalendarController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CalendarController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
