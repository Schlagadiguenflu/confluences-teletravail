﻿using System;
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
    public class TypeStagesController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public TypeStagesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: api/TypeStages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeStage>>> GetTypeStages()
        {
            return await _context.TypeStages.AsNoTracking().ToListAsync();
        }

        // GET: api/TypeStages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeStage>> GetTypeStage(int id)
        {
            var typeStage = await _context.TypeStages.FindAsync(id);

            if (typeStage == null)
            {
                return NotFound();
            }

            return typeStage;
        }

        // PUT: api/TypeStages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeStage(int id, TypeStage typeStage)
        {
            if (id != typeStage.TypeStageId)
            {
                return BadRequest();
            }

            _context.Entry(typeStage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeStageExists(id))
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

        // POST: api/TypeStages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeStage>> PostTypeStage(TypeStage typeStage)
        {
            _context.TypeStages.Add(typeStage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeStage", new { id = typeStage.TypeStageId }, typeStage);
        }

        // DELETE: api/TypeStages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeStage>> DeleteTypeStage(int id)
        {
            var typeStage = await _context.TypeStages.FindAsync(id);
            if (typeStage == null)
            {
                return NotFound();
            }

            _context.TypeStages.Remove(typeStage);
            await _context.SaveChangesAsync();

            return typeStage;
        }

        private bool TypeStageExists(int id)
        {
            return _context.TypeStages.Any(e => e.TypeStageId == id);
        }
    }
}
