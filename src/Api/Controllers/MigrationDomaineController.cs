using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationDomaineController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public MigrationDomaineController(ConfluencesContext context)
        {
            _context = context;
        }
        // GET: api/<MigrationDomaineController>
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var entreprises = await _context.Entreprises.ToListAsync();

            foreach (var entreprise in entreprises)
            {
                if (entreprise.TypeDomaineId != null)
                {
                    EntrepriseDomaine ed = new EntrepriseDomaine { EntrepriseId = entreprise.EntrepriseId, TypeDomaineId = (int)entreprise.TypeDomaineId };
                    _context.EntrepriseDomaines.Add(ed);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return new string[] { "Error" };
                    }
                }
            }

            return new string[] { "Migration completed" };
        }

    }
}
