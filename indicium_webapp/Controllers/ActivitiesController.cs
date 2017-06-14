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
using System.Globalization;
using indicium_webapp.Models.ViewModels;

namespace indicium_webapp.Controllers
{
    
    [Authorize(Roles = "Bestuur, Secretaris")]
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ActivitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Activities
        public IActionResult Index()
        {
            var activities = _context.Activity.Include(a => a.SignUps);

            IEnumerable<ActivityViewModel> activityviewmodel = activities.Select(activity => new ActivityViewModel
            {
                ActivityID = activity.ActivityID,
                Name = activity.Name,
                Description = activity.Description,
                StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                NeedsSignUp = activity.NeedsSignUp,
                Price = activity.Price,
                ActivityType = activity.ActivityType,
                SignUps = activity.SignUps
            });

            return View(activityviewmodel);
        }

        // GET: Activities/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.SignUps).ThenInclude(a => a.ApplicationUser)
                .Include(a => a.SignUps).ThenInclude(a => a.Guest)
                .Include(t => t.ActivityType)
                .SingleOrDefaultAsync(m => m.ActivityID == id);
            
            if (activity == null)
            {
                return NotFound();
            }

            ActivityViewModel activityviewmodel = new ActivityViewModel {
                    ActivityID = activity.ActivityID,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    NeedsSignUp = activity.NeedsSignUp,
                    Price = activity.Price,
                    ActivityType = activity.ActivityType,
                    SignUps = activity.SignUps
            };

            if (_signInManager.IsSignedIn(User))
            {
                ViewData["SignedUp"] = activityviewmodel.SignUps.Any(x => x.ApplicationUserID == GetCurrentUserAsync().Id);
                ViewData["applicationUserResult"] = activityviewmodel.SignUps.Any(x => x.ApplicationUserID != null);
                ViewData["guestResult"] = activityviewmodel.SignUps.Any(x => x.Guest != null);
            }

            return View(activityviewmodel);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            var types = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityTypeID = new SelectList(types, "ActivityTypeID", "Name");

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel activityviewmodel)
        {
            if (ModelState.IsValid)
            {
                
                var activity = new Activity
                {
                    Name = activityviewmodel.Name,
                    Description = activityviewmodel.Description,
                    StartDateTime = DateTime.ParseExact(activityviewmodel.StartDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    EndDateTime = DateTime.ParseExact(activityviewmodel.EndDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    NeedsSignUp = activityviewmodel.NeedsSignUp,
                    Price = activityviewmodel.Price,
                    ActivityType = activityviewmodel.ActivityType
                };
                
                _context.Add(activity);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(activityviewmodel);
        }

        // GET: Activities/Edit/5
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

            ActivityViewModel activityviewmodel = new ActivityViewModel {
                    ActivityID = activity.ActivityID,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    NeedsSignUp = activity.NeedsSignUp,
                    Price = activity.Price,
                    ActivityType = activity.ActivityType,
                    SignUps = activity.SignUps
            };

            var types = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityType = new SelectList(types, "ActivityTypeID", "Name", activityviewmodel.ActivityType);

            return View(activityviewmodel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityViewModel activityviewmodel)
        {
            if (id != activityviewmodel.ActivityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activity = new Activity {
                        ActivityID = activityviewmodel.ActivityID,
                        Name = activityviewmodel.Name,
                        Description = activityviewmodel.Description,
                        StartDateTime = DateTime.ParseExact(activityviewmodel.StartDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                        EndDateTime = DateTime.ParseExact(activityviewmodel.EndDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                        NeedsSignUp = activityviewmodel.NeedsSignUp,
                        Price = activityviewmodel.Price,
                        ActivityType = activityviewmodel.ActivityType
                    };

                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Activity.Any(e => e.ActivityID == activityviewmodel.ActivityID))
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
            return View(activityviewmodel);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(t => t.ActivityType)
                .SingleOrDefaultAsync(m => m.ActivityID == id);

            if (activity == null)
            {
                return NotFound();
            }

            ActivityViewModel activityviewmodel = new ActivityViewModel {
                    ActivityID = activity.ActivityID,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    NeedsSignUp = activity.NeedsSignUp,
                    Price = activity.Price,
                    ActivityType = activity.ActivityType,
                    SignUps = activity.SignUps
            };

            return View(activityviewmodel);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: Activities/Calendar
        [AllowAnonymous]
        public IActionResult Calendar()
        {
            return View();
        }

        private ApplicationUser GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User).Result;
        }
    }
}
