using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using indicium_webapp.Models.ViewModels.AccountViewModels;
using indicium_webapp.Models.ViewModels;
using System.Globalization;
using indicium_webapp.Services;

namespace indicium_webapp.Controllers
{
    [Authorize]
    public class SignUpsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public SignUpsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: SignUps
        public async Task<IActionResult> Index()
        {
            var signUpsResult = await _context.SignUp
                .Include(signUp => signUp.Activity).ThenInclude(signUp => signUp.ActivityType)
                .Where(signUp => signUp.ApplicationUserID == GetCurrentUserAsync().Id)
                .ToListAsync();
            
            IEnumerable<SignUpViewModel> model = signUpsResult.Select(CreateSignUpViewModel);

            return View(model);
        }

        // GET: SignUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUpResult = await _context.SignUp
                .Include(signUp => signUp.Activity).ThenInclude(signUp => signUp.ActivityType)
                .Include(signUp => signUp.ApplicationUser)
                .SingleOrDefaultAsync(signUp => signUp.SignUpID == id);

            if (signUpResult == null)
            {
                return NotFound();
            }

            return View(CreateSignUpViewModel(signUpResult));
        }

        // POST: SignUps/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, SignUpViewModel model)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Assigns the currently logged in user Id and activity Id to the signup. 
                model.ApplicationUser.Id = GetCurrentUserAsync().Id;
                model.Activity.ActivityID = id;

                // Validates if activity needs a signup and if user is not already signed up to said activity.
                if (_context.Activity.Find(id).NeedsSignUp)
                {
                    if (!UserSignedUp(id))
                    {
                        // Saves the signup to the database.
                        _context.Add(CreateSignUp(model));
                        await _context.SaveChangesAsync();

                        await _emailSender.SendCalendarInviteAsync(model.ApplicationUser.Email, CreateActivity(model.Activity));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", "activities", new { id });
                    }
                }
                else
                {
                    return RedirectToAction("Details", "activities", new { id });
                }
            }

            return View(model);
        }

        // GET: SignUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUpResult = await _context.SignUp
                .Include(signUp => signUp.Activity)
                .ThenInclude(signUp => signUp.ActivityType)
                .Include(signUp => signUp.ApplicationUser)
                .SingleOrDefaultAsync(signUp => signUp.ActivityID == id && signUp.ApplicationUserID == GetCurrentUserAsync().Id);

            if (signUpResult == null)
            {
                return NotFound();
            }

            return View(CreateSignUpViewModel(signUpResult));
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signUpResult = await _context.SignUp.SingleOrDefaultAsync(signUp => signUp.ActivityID == id && signUp.ApplicationUserID == GetCurrentUserAsync().Id);

            _context.SignUp.Remove(signUpResult);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: SignUps/Guest/5
        [AllowAnonymous]
        public async Task<IActionResult> Guest(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Forbid();
            }
            
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

            if (activityResult.NeedsSignUp == false)
            {
                return Forbid();
            }
            
            return View(CreateGuestViewModel(activityResult));
        }

        // POST: SignUps/Guest/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Guest(int? id, GuestViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var activityResult = await _context.Activity.SingleOrDefaultAsync(activity => activity.ActivityID == id);

                if (activityResult.NeedsSignUp)
                {
                    _context.Add(CreateGuestSignUp(activityResult.ActivityID, model));
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Calendar", "activities");
                }
                else
                {
                    return RedirectToAction("Details", "activities", new { id });
                }
            }

            return View(model);
        }

        private bool UserSignedUp(int activityId)
        {
            return _context.SignUp.Any(signUp => signUp.ActivityID == activityId && signUp.ApplicationUserID == GetCurrentUserAsync().Id);
        }

        private ApplicationUser GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User).Result;
        }
        
        private SignUp CreateSignUp(SignUpViewModel model)
        {
            return new SignUp
            {
                ActivityID = model.Activity.ActivityID,
                ApplicationUserID = model.ApplicationUser.Id,
                GuestID = model.Guest.GuestID
            };
        }
        
        private SignUp CreateGuestSignUp(int id, GuestViewModel model)
        {
            var activityResult = _context.Activity.SingleOrDefaultAsync(activity => activity.ActivityID == id);
            
            var test = new SignUp
            {
                ActivityID = id,
                Guest = new Guest
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                }
            };

            return test;
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
        
        private GuestViewModel CreateGuestViewModel(Activity activity)
        {
            return new GuestViewModel
            {
                Activity = activity
            };
        }
        
        private SignUpViewModel CreateSignUpViewModel(SignUp signUp)
        {
            return new SignUpViewModel
            {
                SignUpID = signUp.SignUpID,
                ApplicationUser = new ApplicationUserViewModel {
                    FirstName = signUp.ApplicationUser.FirstName,
                    LastName = signUp.ApplicationUser.LastName,
                    Sex = signUp.ApplicationUser.Sex.ToString(),
                    Birthday = signUp.ApplicationUser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    AddressStreet = signUp.ApplicationUser.AddressStreet,
                    AddressNumber = signUp.ApplicationUser.AddressNumber,
                    AddressPostalCode = signUp.ApplicationUser.AddressPostalCode,
                    AddressCity = signUp.ApplicationUser.AddressCity,
                    AddressCountry = signUp.ApplicationUser.AddressCountry,
                    StudentNumber = signUp.ApplicationUser.StudentNumber.ToString(),
                    StartdateStudy = signUp.ApplicationUser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    StudyType = signUp.ApplicationUser.StudyType.ToString(),
                    PhoneNumber = signUp.ApplicationUser.PhoneNumber
                },
                Activity = new ActivityViewModel {
                    ActivityID = signUp.Activity.ActivityID,
                    Name = signUp.Activity.Name,
                    Description = signUp.Activity.Description,
                    StartDateTime = signUp.Activity.StartDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    EndDateTime = signUp.Activity.EndDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    NeedsSignUp = signUp.Activity.NeedsSignUp,
                    ActivityType = signUp.Activity.ActivityType,
                    SignUps = signUp.Activity.SignUps
                }
            };
        }
    }
}
