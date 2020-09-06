using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public AccountController(ConfluencesContext context)
        {
            _context = context;
        }

        // Renvoie l'ensemble des données de l'utilisateur
        // GET: api/Account/checkAccessToken
        [Route("checkAccessToken")]
        [HttpGet()]
        public async Task<ActionResult<bool>> CheckAccessToken()
        {
            return true;
        }

        // Renvoie l'ensemble des données de l'utilisateur
        // GET: api/Account/getUserInfo
        [Route("getUserInfo")]
        [HttpGet()]
        public async Task<ActionResult<AspNetUser>> GetAspNetUser()
        {
            AspNetUser user = null;
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            user = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SchoolClassRoom)
                                        .ThenInclude(a => a.SchoolClassRoomExplanations)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.Homework)
                                        .ThenInclude(a => a.HomeworkType)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.HomeworkV2s)
                                        .ThenInclude(a => a.HomeworkType)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SessionTeachers)
                                         .ThenInclude(a => a.Teacher)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                            .Include(a => a.AppointmentStudents)
                                .ThenInclude(a => a.Appointment)
                            //.ThenInclude(a => a.AppointmentStudents)
                            //    .ThenInclude(a => a.Student)

                            .SingleOrDefaultAsync(a => a.Id == id);

            if (user.SessionStudents.Count() > 0)
            {
                DateTime dateStartMax = user.SessionStudents.Max(s => s.Session.DateStart);
                user.SessionStudents = user.SessionStudents.Where(s => s.Session.DateStart == dateStartMax).ToList();
                foreach (var item in user.SessionStudents)
                {
                    item.Session.SessionStudents = await _context.SessionStudents
                                                            .Include(s => s.Student)
                                                            .Where(s => s.SessionId == item.SessionId)
                                                            .OrderBy(s => s.Student.Firstname)
                                                            .ToListAsync();
                }
            }

            if (user.AppointmentStudents.Count() > 0)
            {
                foreach (var item in user.AppointmentStudents)
                {
                    item.Appointment.AppointmentStudents = await _context.AppointmentStudents
                                                            .Include(s => s.Student)
                                                            .Where(s => s.AppointmentId == item.AppointmentId)
                                                            .ToListAsync();
                }
            }

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                return user;
            }

        }

        // Renvoie l'ensemble des données de l'utilisateur
        // GET: api/Account/getUserInfoStudent
        [Route("getUserInfoStudent")]
        [HttpGet()]
        public async Task<ActionResult<AspNetUser>> GetAspNetUserStudent()
        {
            AspNetUser user = null;
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            user = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SchoolClassRoom)
                                        .ThenInclude(a => a.SchoolClassRoomExplanations)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.Homework)
                                        .ThenInclude(a => a.HomeworkType)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.HomeworkV2s)
                                        .ThenInclude(a => a.HomeworkType)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.Homework)
                                        .ThenInclude(a => a.HomeworkStudents)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)
                                    .ThenInclude(a => a.SessionTeachers)
                                         .ThenInclude(a => a.Teacher)
                            .Include(a => a.SessionStudents)
                                .ThenInclude(a => a.Session)

                            .SingleOrDefaultAsync(a => a.Id == id);

            if (user.SessionStudents.Count() > 0)
            {
                DateTime dateStartMax = user.SessionStudents.Max(s => s.Session.DateStart);
                user.SessionStudents = user.SessionStudents.Where(s => s.Session.DateStart == dateStartMax).ToList();
                foreach (var item in user.SessionStudents)
                {
                    item.Session.SessionStudents = await _context.SessionStudents
                                                            .Include(s => s.Student)
                                                            .Where(s => s.SessionId == item.SessionId)
                                                            .OrderBy(s => s.Student.Firstname)
                                                            .ToListAsync();
                }
            }

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                return user;
            }

        }

        // Renvoie l'ensemble des données de l'utilisateur
        // GET: api/Account/getUserInfo
        [Route("getUserInfoAppointments")]
        [HttpGet()]
        public async Task<ActionResult<AspNetUser>> GetAspNetUserAppointments()
        {
            AspNetUser user = null;
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            user = await _context.AspNetUsers
                            .AsNoTracking()
                            .Include(a => a.AppointmentStudents)
                                .ThenInclude(a => a.Appointment)
                            //.ThenInclude(a => a.AppointmentStudents)
                            //    .ThenInclude(a => a.Student)

                            .SingleOrDefaultAsync(a => a.Id == id);

            if (user.AppointmentStudents.Count() > 0)
            {
                foreach (var item in user.AppointmentStudents)
                {
                    item.Appointment.AppointmentStudents = await _context.AppointmentStudents
                                                            .Include(s => s.Student)
                                                            .Where(s => s.AppointmentId == item.AppointmentId)
                                                            .ToListAsync();
                }
            }

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                return user;
            }

        }

        // Enregistre la confirmation d'avoir vu la vidéo
        // GET: api/Account/IGetIt
        [Route("IGetIt")]
        [HttpGet()]
        public async Task<ActionResult<string>> IGetIt()
        {
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            AspNetUser aspNetUser = await _context.AspNetUsers
                            .AsNoTracking()
                            .Where(a => a.Id == id)
                            .SingleOrDefaultAsync();
            aspNetUser.HasSeenHelpVideo = true;

            _context.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return "";

        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}