using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Controllers
{
    public class AnnouncePagesController : Controller
    {
        private readonly ConfluencesContext _context;

        public AnnouncePagesController(ConfluencesContext context)
        {
            _context = context;
        }

        // GET: AnnouncePages
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnnouncePages.ToListAsync());
        }

        // GET: AnnouncePages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcePage = await _context.AnnouncePages
                .FirstOrDefaultAsync(m => m.AnnouncePageId == id);
            if (announcePage == null)
            {
                return NotFound();
            }

            return View(announcePage);
        }

        // GET: AnnouncePages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnnouncePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnnouncePageId,AnnouncePageName,IsActivated")] AnnouncePage announcePage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(announcePage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcePage);
        }

        // GET: AnnouncePages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcePage = await _context.AnnouncePages.FindAsync(id);
            if (announcePage == null)
            {
                return NotFound();
            }
            return View(announcePage);
        }

        // POST: AnnouncePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnnouncePageId,AnnouncePageName,IsActivated")] AnnouncePage announcePage)
        {
            if (id != announcePage.AnnouncePageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcePage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncePageExists(announcePage.AnnouncePageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(announcePage);
        }

        // GET: AnnouncePages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcePage = await _context.AnnouncePages
                .FirstOrDefaultAsync(m => m.AnnouncePageId == id);
            if (announcePage == null)
            {
                return NotFound();
            }

            return View(announcePage);
        }

        // POST: AnnouncePages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcePage = await _context.AnnouncePages.FindAsync(id);
            _context.AnnouncePages.Remove(announcePage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncePageExists(int id)
        {
            return _context.AnnouncePages.Any(e => e.AnnouncePageId == id);
        }
    }
}
