using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebApp.Models;

namespace CollegeWebApp.Controllers
{
    public class ModulesController : Controller
    {
        private readonly CollegeDbContext _context;

        public ModulesController(CollegeDbContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var collegeDbContext = _context.Modules.Include(m => m.Student).Include(m => m.Venue);
            return View(await collegeDbContext.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .Include(m => m.Student)
                .Include(m => m.Venue)
                .FirstOrDefaultAsync(m => m.ModuleID == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueID");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleID,StudentID,VenueID,Date,Time")] Module @module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", @module.StudentID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueID", @module.VenueID);
            return View(@module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", @module.StudentID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueID", @module.VenueID);
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleID,StudentID,VenueID,Date,Time")] Module @module)
        {
            if (id != @module.ModuleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.ModuleID))
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
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", @module.StudentID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueID", @module.VenueID);
            return View(@module);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .Include(m => m.Student)
                .Include(m => m.Venue)
                .FirstOrDefaultAsync(m => m.ModuleID == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await _context.Modules.FindAsync(id);
            if (@module != null)
            {
                _context.Modules.Remove(@module);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleID == id);
        }
    }
}
