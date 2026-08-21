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

        // GET: Modules with Search and Pagination
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;

            var modules = _context.Modules
                .Include(m => m.Student)
                .Include(m => m.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                modules = modules.Where(m =>
                    (m.Student != null && m.Student.StudentName != null && m.Student.StudentName.Contains(searchString)) ||
                    (m.Venue != null && m.Venue.VenueName != null && m.Venue.VenueName.Contains(searchString)) ||
                    m.ModuleID.ToString().Contains(searchString) ||
                    m.Date.ToString().Contains(searchString)
                );
            }

            ViewBag.TotalModules = await modules.CountAsync();

            int pageSize = 10;
            return View(await PaginatedList<Module>.CreateAsync(modules, pageNumber ?? 1, pageSize));
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
            // Show Student Name instead of ID
            ViewData["StudentID"] = new SelectList(
                _context.Students,
                "StudentID",
                "StudentName"
            );

            // Show Venue Name instead of ID
            ViewData["VenueID"] = new SelectList(
                _context.Venues,
                "VenueID",
                "VenueName"
            );
            return View();
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleID,StudentID,VenueID,Date,Time")] Module @module)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check for conflicts
                var conflict = await _context.Modules
                    .AnyAsync(m => m.StudentID == @module.StudentID &&
                                   m.Date == @module.Date &&
                                   m.Time == @module.Time);

                if (conflict)
                {
                    ModelState.AddModelError("", "This student already has a module at this date and time.");
                    ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentName", @module.StudentID);
                    ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @module.VenueID);
                    return View(@module);
                }

                _context.Add(@module);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Module created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentName", @module.StudentID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @module.VenueID);
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

            ViewData["StudentID"] = new SelectList(
                _context.Students,
                "StudentID",
                "StudentName",
                @module.StudentID
            );
            ViewData["VenueID"] = new SelectList(
                _context.Venues,
                "VenueID",
                "VenueName",
                @module.VenueID
            );
            return View(@module);
        }

        // POST: Modules/Edit/5
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
                    TempData["SuccessMessage"] = "Module updated successfully!";
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
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentName", @module.StudentID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @module.VenueID);
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
                TempData["SuccessMessage"] = "Module deleted successfully!";
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