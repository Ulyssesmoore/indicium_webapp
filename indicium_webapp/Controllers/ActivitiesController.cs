using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace indicium_webapp.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activity.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.SignUps)
                .SingleOrDefaultAsync(m => m.ActivityID == id);
            
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Bestuur, Secretaris")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> Create([Bind("ActivityID,Name,Description,StartDateTime,EndDateTime,NeedsSignUp,Price")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);
            
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityID,Name,Description,StartDateTime,EndDateTime,NeedsSignUp,Price")] Activity activity)
        {
            if (id != activity.ActivityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bestuur, Secretaris")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: Activities/Calendar
        public async Task<IActionResult> Calendar()
        {
            return View(await _context.Activity.ToListAsync());
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.ActivityID == id);
        }
    }
}
