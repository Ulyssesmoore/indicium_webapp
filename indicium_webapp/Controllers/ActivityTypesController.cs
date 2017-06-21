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
using Microsoft.AspNetCore.Authorization;

namespace indicium_webapp.Controllers
{
    [Route("activiteit-types"), Authorize(Roles = "Bestuur, Secretaris")]
    public class ActivityTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: /activiteit-types
        public async Task<IActionResult> Index()
        {
            var activityTypesResult = await _context.ActivityType.ToListAsync();
            IEnumerable<ActivityTypeViewModel> model = activityTypesResult.Select(CreateActivityTypeViewModel);

            return View(model);
        }

        // GET: /activiteit-types/aanmaken
        [Route("aanmaken")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /activiteit-types/aanmaken
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("aanmaken")]
        public async Task<IActionResult> Create(ActivityTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(CreateActivityType(model));
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /activiteit-types/bewerken/{id}
        [Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activitytypeResult = await _context.ActivityType.SingleOrDefaultAsync(activityType => activityType.ActivityTypeID == id);

            if (activitytypeResult == null)
            {
                return NotFound();
            }

            return View(CreateActivityTypeViewModel(activitytypeResult));
        }

        // POST: /activiteit-types/bewerken/{id}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int id, ActivityTypeViewModel model)
        {
            if (id != model.ActivityTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CreateActivityType(model));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(model.ActivityTypeID))
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
            return View(model);
        }

        // GET: /activiteit-types/verwijderen/{id}
        [Route("verwijderen/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityTypeResult = await _context.ActivityType.SingleOrDefaultAsync(activityType => activityType.ActivityTypeID == id);

            if (activityTypeResult == null)
            {
                return NotFound();
            }

            return View(CreateActivityTypeViewModel(activityTypeResult));
        }

        // POST: /activiteit-types/verwijderen/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Route("verwijderen/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityTypeResult = await _context.ActivityType.SingleOrDefaultAsync(activityType => activityType.ActivityTypeID == id);
            
            _context.ActivityType.Remove(activityTypeResult);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityType.Any(activityType => activityType.ActivityTypeID == id);
        }

        private ActivityType CreateActivityType(ActivityTypeViewModel model)
        {
            return new ActivityType
            {
                ActivityTypeID = model.ActivityTypeID,
                Name = model.Name,
                BackgroundColor = model.BackgroundColor,
                BorderColor = model.BorderColor,
                TextColor = model.TextColor
            };
        }
        
        private ActivityTypeViewModel CreateActivityTypeViewModel(ActivityType activityType)
        {
            return new ActivityTypeViewModel
            {
                ActivityTypeID = activityType.ActivityTypeID,
                Name = activityType.Name,
                BackgroundColor = activityType.BackgroundColor,
                BorderColor = activityType.BorderColor,
                TextColor = activityType.TextColor
            };
        }
    }
}
