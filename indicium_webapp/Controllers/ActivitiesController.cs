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
    [Route("activiteiten"), Authorize(Roles = "Bestuur, Secretaris")]
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

        public enum ActivityMessageId
        {
            GuestSignUpSuccess
        }

        // GET: /activiteiten
        public async Task<IActionResult> Index()
        {
            var activitiesResult = await _context.Activity.Include(activity => activity.SignUps).ToListAsync();
            IEnumerable<ActivityViewModel> model = activitiesResult.Select(CreateActivitiesViewModel);

            return View(model);
        }

        // GET: /activiteiten/details/{id}
        [AllowAnonymous, Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityResult = await _context.Activity
                .Include(activity => activity.SignUps).ThenInclude(activity => activity.ApplicationUser)
                .Include(activity => activity.SignUps).ThenInclude(activity => activity.Guest)
                .Include(activity => activity.ActivityType)
                .SingleOrDefaultAsync(activity => activity.ActivityID == id);
            
            if (activityResult == null)
            {
                return NotFound();
            }

            ActivityViewModel model = CreateActivitiesViewModel(activityResult);

            if (_signInManager.IsSignedIn(User))
            {
                ViewBag.LoggedInUserSignedUp = model.SignUps.Any(signUps => signUps.ApplicationUserID == _userManager.GetUserAsync(User).Result.Id);
                ViewBag.ApplicationUsersSignedUpCount = model.SignUps.Where(signUps => signUps.ApplicationUserID != null).Count();
                ViewBag.GuestsSignedUpCount = model.SignUps.Where(signUps => signUps.Guest != null).Count();
            }

            return View(model);
        }

        // GET: /activiteiten/aanmaken
        [Route("aanmaken")]
        public IActionResult Create()
        {
            var activityTypesResult = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityTypes = new SelectList(activityTypesResult, "ActivityTypeID", "Name");

            return View();
        }

        // POST: /activiteiten/aanmaken
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("aanmaken")]
        public async Task<IActionResult> Create(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(CreateActivity(model));
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var activityTypesResult = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityTypes = new SelectList(activityTypesResult, "ActivityTypeID", "Name");

            return View(model);
        }

        // GET: /activiteiten/bewerken/{id}
        [Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityResult = await _context.Activity
                .Include(activity => activity.ActivityType)
                .SingleOrDefaultAsync(activity => activity.ActivityID == id);
            
            var activityTypeResult = _context.ActivityType.SingleOrDefault(activityType => activityType.ActivityTypeID == activityResult.ActivityTypeID);
            
            if (activityResult == null)
            {
                return NotFound();
            }

            var activityTypesResult = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityTypes = new SelectList(activityTypesResult, "ActivityTypeID", "Name", activityTypeResult.ActivityTypeID);

            return View(CreateActivitiesViewModel(activityResult));
        }

        // POST: /activiteiten/bewerken/{id}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int id, ActivityViewModel model)
        {
            if (id != model.ActivityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CreateActivity(model));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(model.ActivityID))
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

            var activityResult = await _context.Activity
                .Include(activity => activity.ActivityType)
                .SingleOrDefaultAsync(activity => activity.ActivityID == id);
            
            var activityTypeResult = _context.ActivityType.SingleOrDefault(activityType => activityType.ActivityTypeID == activityResult.ActivityTypeID);
            
            if (activityResult == null)
            {
                return NotFound();
            }
            
            var activityTypesResult = _context.ActivityType.ToListAsync().Result;
            ViewBag.ActivityTypes = new SelectList(activityTypesResult, "ActivityTypeID", "Name", activityTypeResult.ActivityTypeID);

            return View(model);
        }

        // GET: /activiteiten/verwijderen/{id}
        [Route("verwijderen/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityResult = await _context.Activity
                .Include(activity => activity.ActivityType)
                .SingleOrDefaultAsync(activity => activity.ActivityID == id);

            if (activityResult == null)
            {
                return NotFound();
            }

            return View(CreateActivitiesViewModel(activityResult));
        }

        // POST: /activiteiten/verwijderen/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Route("verwijderen/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityResult = await _context.Activity.SingleOrDefaultAsync(activity => activity.ActivityID == id);

            _context.Activity.Remove(activityResult);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: /activiteiten/kalender
        [AllowAnonymous, Route("kalender")]
        public IActionResult Calendar(ActivityMessageId? message = null)
        {
            ViewData["StatusMessage"] = message == ActivityMessageId.GuestSignUpSuccess ? "Je inschrijving is gelukt." : "";
            
            return View();
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.Activity.Any(activity => activity.ActivityTypeID == id);
        }

        private Activity CreateActivity(ActivityViewModel model)
        {
            var activityTypeResult = _context.ActivityType.SingleOrDefault(activityType => activityType.ActivityTypeID == Convert.ToInt32(model.ActivityTypeID));
            
            return new Activity
            {
                ActivityID = model.ActivityID,
                Name = model.Name,
                Description = model.Description,
                StartDateTime = DateTime.ParseExact(model.StartDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                EndDateTime = DateTime.ParseExact(model.EndDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                NeedsSignUp = model.NeedsSignUp,
                ActivityType = activityTypeResult
            };
        }
        
        private ActivityViewModel CreateActivitiesViewModel(Activity activity)
        {
            return new ActivityViewModel
            {
                ActivityID = activity.ActivityID,
                Name = activity.Name,
                Description = activity.Description,
                StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                NeedsSignUp = activity.NeedsSignUp,
                SignUps = activity.SignUps,
                ActivityType = activity.ActivityType
            };
        }
    }
}
