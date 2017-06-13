using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using indicium_webapp.Models.ViewModels;

namespace indicium_webapp.Controllers
{
    public class ActivityTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ActivityTypes
        public async Task<IActionResult> Index()
        {
            var activitytypes = await _context.ActivityType.ToListAsync();
            
            IEnumerable<ActivityTypeViewModel> activitytypeviewmodel = activitytypes.Select(activitytype => new ActivityTypeViewModel
            {
                ActivityTypeID = activitytype.ActivityTypeID,
                Name = activitytype.Name,
                BackgroundColor = activitytype.BackgroundColor,
                BorderColor = activitytype.BorderColor,
                TextColor = activitytype.TextColor
            });

            return View(activitytypeviewmodel);
        }

        // GET: ActivityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityTypeViewModel activitytypeviewmodel)
        {
            if (ModelState.IsValid)
            {
                var activitytype = new ActivityType
                {
                    ActivityTypeID = activitytypeviewmodel.ActivityTypeID,
                    Name = activitytypeviewmodel.Name,
                    BackgroundColor = activitytypeviewmodel.BackgroundColor,
                    BorderColor = activitytypeviewmodel.BorderColor,
                    TextColor = activitytypeviewmodel.TextColor
                };

                _context.Add(activitytype);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(activitytypeviewmodel);
        }

        // GET: ActivityTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activitytype = await _context.ActivityType.SingleOrDefaultAsync(m => m.ActivityTypeID == id);

            if (activitytype == null)
            {
                return NotFound();
            }

            ActivityTypeViewModel activitytypeviewmodel = new ActivityTypeViewModel
            {
                ActivityTypeID = activitytype.ActivityTypeID,
                Name = activitytype.Name,
                BackgroundColor = activitytype.BackgroundColor,
                BorderColor = activitytype.BorderColor,
                TextColor = activitytype.TextColor
            };

            return View(activitytypeviewmodel);
        }

        // POST: ActivityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityTypeViewModel activitytypeviewmodel)
        {
            if (id != activitytypeviewmodel.ActivityTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activitytype = new ActivityType
                    {
                        ActivityTypeID = activitytypeviewmodel.ActivityTypeID,
                        Name = activitytypeviewmodel.Name,
                        BackgroundColor = activitytypeviewmodel.BackgroundColor,
                        BorderColor = activitytypeviewmodel.BorderColor,
                        TextColor = activitytypeviewmodel.TextColor
                    };
                    
                    _context.Update(activitytype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(activitytypeviewmodel.ActivityTypeID))
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
            return View(activitytypeviewmodel);
        }

        // GET: ActivityTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activitytype = await _context.ActivityType.SingleOrDefaultAsync(m => m.ActivityTypeID == id);

            if (activitytype == null)
            {
                return NotFound();
            }

            ActivityTypeViewModel activitytypeviewmodel = new ActivityTypeViewModel
            {
                ActivityTypeID = activitytype.ActivityTypeID,
                Name = activitytype.Name,
                BackgroundColor = activitytype.BackgroundColor,
                BorderColor = activitytype.BorderColor,
                TextColor = activitytype.TextColor
            };

            return View(activitytypeviewmodel);
        }

        // POST: ActivityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityType = await _context.ActivityType.SingleOrDefaultAsync(m => m.ActivityTypeID == id);
            _context.ActivityType.Remove(activityType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityType.Any(e => e.ActivityTypeID == id);
        }
    }
}
