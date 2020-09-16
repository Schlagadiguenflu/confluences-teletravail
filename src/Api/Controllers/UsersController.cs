/**
 * Projet: Gestion des stagiaires
 * Auteur : Tim Allemann
 * Date : 16.09.2020
 * Description : Contrôleur permettant de lire les utilisateurs sur la table AspNetUsers, seul l'identifiant et le nom sont transmis
 * Fichier : UsersController.cs
 **/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Authorize(Policy = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public UsersController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/UsersController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            var aspnetusers = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SchoolClassRoom)
                            .OrderBy(a => a.Firstname)
                            .Select(v => new UserInfo { Id = v.Id, Nom = v.Firstname + " " + v.LastName })
                            .ToListAsync();


            return aspnetusers;
        }

    }
}
