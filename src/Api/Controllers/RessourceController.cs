using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RessourceController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public RessourceController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Ressource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeworkV2s>>> GetHomeworkV2s()
        {
            return await _context.HomeworkV2s.AsNoTracking()
                                         .Include(h => h.HomeworkType)
                                         .Include(h => h.Session)
                                            .ThenInclude(h => h.SchoolClassRoom)
                                         .Include(h => h.Teacher)
                                         .Include(h => h.Theories)
                                            .ThenInclude(h => h.Exercices)
                                         .Include(h => h.ExercicesAlones)
                                         .OrderByDescending(h => h.HomeworkV2date)
                                         .ToListAsync();
        }
    }
}
