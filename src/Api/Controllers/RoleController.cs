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
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public RoleController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetRole(string userId = "")
        {
            return await _context.AspNetUserRoles
                            .AsNoTracking()
                            .Where(a => a.UserId == userId)
                            .Select(a => a.RoleId)
                            .ToListAsync();
        }

        // GET: api/Role/TeachersHome
        [Route("TeachersHome")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetTeachers()
        {
            List<AspNetUser> teachers =  await _context.AspNetUserRoles
                                                .Include(a => a.User)
                                                .Where(a => a.RoleId == "Teacher")
                                                .Select(a => a.User)
                                                .ToListAsync();
            teachers = teachers.Where(u => u.UserName != "TA").ToList();

            List<AspNetUser> teachersCleaned = new List<AspNetUser>();
            foreach (AspNetUser teacher in teachers)
            {
                teachersCleaned.Add(new AspNetUser { 
                    Firstname = teacher.Firstname,
                    LastName = teacher.LastName,
                    PathImage = teacher.PathImage,
                    PhoneNumber = teacher.PhoneNumber
                });
            }

            return teachersCleaned;
        }

        [Authorize(Policy = "Teacher")]
        // GET: api/Role/Teachers
        [Route("Teachers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetTeachersHome()
        {
            return await _context.AspNetUserRoles
                            .Include(a => a.User)
                            .Where(a => a.RoleId == "Teacher")
                            .Select(a => a.User)
                            .OrderBy(a => a.Firstname)
                            .ToListAsync();
        }

        [Authorize(Policy = "Teacher")]
        // GET: api/Role/Students
        [Route("Students")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetStudents()
        {
            List<string> usersId = await _context.AspNetUserRoles
                            .Include(a => a.User)
                            .Where(a => a.RoleId == "Teacher")
                            .Select(a => a.User.Id)
                            .ToListAsync();

            return await _context.AspNetUsers
                            .Where(a => !usersId.Contains(a.Id))
                            .OrderBy(a => a.Firstname)
                            .ToListAsync();
        }
    }
}