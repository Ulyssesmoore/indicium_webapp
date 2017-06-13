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
using indicium_webapp.Models.AccountViewModels;
using System.Globalization;

namespace indicium_webapp.Controllers
{
    [Authorize]
    public class SignUpsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignUpsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: SignUps
        public async Task<IActionResult> Index()
        {
            var signups = await _context.SignUp
                .Include(m => m.Activity)
                .Where(m => m.ApplicationUserID == GetCurrentUserAsync().Id)
                .ToListAsync();
            
            IEnumerable<SignUpViewModel> signupviewmodel = signups.Select(signup => new SignUpViewModel
            {
                SignUpID = signup.SignUpID,
                Status = signup.Status,
                Activity = new ActivityViewModel {
                    ActivityID = signup.Activity.ActivityID,
                    Name = signup.Activity.Name,
                    Description = signup.Activity.Description,
                    StartDateTime = signup.Activity.StartDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    EndDateTime = signup.Activity.EndDateTime.ToString("dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")),
                    NeedsSignUp = signup.Activity.NeedsSignUp,
                    Price = signup.Activity.Price,
                    ActivityType = signup.Activity.ActivityType,
                    SignUps = signup.Activity.SignUps
                }
            });

            return View(signupviewmodel);
        }

        // GET: SignUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signup = await _context.SignUp
                .Include(m => m.Activity)
                .Include(m => m.ApplicationUser)
                .SingleOrDefaultAsync(m => m.SignUpID == id);

            if (signup == null)
            {
                return NotFound();
            }

            SignUpViewModel signupviewmodel = new SignUpViewModel
            {
                SignUpID = signup.SignUpID,
                Status = signup.Status,
                ApplicationUser = new ApplicationUserViewModel {
                    FirstName = signup.ApplicationUser.FirstName,
                    LastName = signup.ApplicationUser.LastName,
                    Sex = signup.ApplicationUser.Sex.ToString(),
                    Birthday = signup.ApplicationUser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    AddressStreet = signup.ApplicationUser.AddressStreet,
                    AddressNumber = signup.ApplicationUser.AddressNumber,
                    AddressPostalCode = signup.ApplicationUser.AddressPostalCode,
                    AddressCity = signup.ApplicationUser.AddressCity,
                    AddressCountry = signup.ApplicationUser.AddressCountry,
                    Iban = signup.ApplicationUser.Iban,
                    StudentNumber = signup.ApplicationUser.StudentNumber.ToString(),
                    StartdateStudy = signup.ApplicationUser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    StudyType = signup.ApplicationUser.StudyType.ToString(),
                    PhoneNumber = signup.ApplicationUser.PhoneNumber
                },
                Activity = new ActivityViewModel {
                    ActivityID = signup.Activity.ActivityID,
                    Name = signup.Activity.Name,
                    Description = signup.Activity.Description,
                    StartDateTime = signup.Activity.StartDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    EndDateTime = signup.Activity.EndDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    NeedsSignUp = signup.Activity.NeedsSignUp,
                    Price = signup.Activity.Price,
                    ActivityType = signup.Activity.ActivityType,
                    SignUps = signup.Activity.SignUps
                }
            };

            return View(signupviewmodel);
        }

        // POST: SignUps/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, SignUp signUp)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Assigns the currently logged in user Id and activity Id to the signup. 
                signUp.ApplicationUserID = GetCurrentUserAsync().Id;
                signUp.ActivityID = id;

                // Validates if activity needs a signup and if user is not already signed up to said activity.
                if (_context.Activity.Find(id).NeedsSignUp)
                {
                    if (!UserSignedUp(id))
                    {
                        // Saves the signup to the database.
                        _context.Add(signUp);
                        await _context.SaveChangesAsync();

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

            return View(signUp);
        }

        // GET: SignUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signup = await _context.SignUp
                .Include(m => m.Activity)
                .Include(m => m.ApplicationUser)
                .SingleOrDefaultAsync(m => m.ActivityID == id && m.ApplicationUserID == GetCurrentUserAsync().Id);

            if (signup == null)
            {
                return NotFound();
            }

            SignUpViewModel signupviewmodel = new SignUpViewModel
            {
                SignUpID = signup.SignUpID,
                Status = signup.Status,
                ApplicationUser = new ApplicationUserViewModel {
                    FirstName = signup.ApplicationUser.FirstName,
                    LastName = signup.ApplicationUser.LastName,
                    Sex = signup.ApplicationUser.Sex.ToString(),
                    Birthday = signup.ApplicationUser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    AddressStreet = signup.ApplicationUser.AddressStreet,
                    AddressNumber = signup.ApplicationUser.AddressNumber,
                    AddressPostalCode = signup.ApplicationUser.AddressPostalCode,
                    AddressCity = signup.ApplicationUser.AddressCity,
                    AddressCountry = signup.ApplicationUser.AddressCountry,
                    Iban = signup.ApplicationUser.Iban,
                    StudentNumber = signup.ApplicationUser.StudentNumber.ToString(),
                    StartdateStudy = signup.ApplicationUser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    StudyType = signup.ApplicationUser.StudyType.ToString(),
                    PhoneNumber = signup.ApplicationUser.PhoneNumber,
                },
                Guest = new GuestViewModel {
                    FirstName = signup.Guest.FirstName,
                    LastName = signup.Guest.LastName,
                    Email = signup.Guest.Email
                },
                Activity = new ActivityViewModel {
                    ActivityID = signup.Activity.ActivityID,
                    Name = signup.Activity.Name,
                    Description = signup.Activity.Description,
                    StartDateTime = signup.Activity.StartDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    EndDateTime = signup.Activity.EndDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    NeedsSignUp = signup.Activity.NeedsSignUp,
                    Price = signup.Activity.Price,
                    ActivityType = signup.Activity.ActivityType,
                    SignUps = signup.Activity.SignUps
                }
            };

            return View(signupviewmodel);
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signUp = await _context.SignUp.SingleOrDefaultAsync(m => m.ActivityID == id && m.ApplicationUserID == GetCurrentUserAsync().Id);

            _context.SignUp.Remove(signUp);
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

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);

            if (activity == null)
            {
                return NotFound();
            }

            GuestViewModel guestviewmodel = new GuestViewModel
            {
                Activity = new ActivityViewModel {
                    ActivityID = activity.ActivityID,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDateTime = activity.StartDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    EndDateTime = activity.EndDateTime.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                    NeedsSignUp = activity.NeedsSignUp,
                    Price = activity.Price,
                    ActivityType = activity.ActivityType,
                    SignUps = activity.SignUps
                }
            };

            if (activity.NeedsSignUp == false)
            {
                return NotFound();
            }
            
            return View(guestviewmodel);
        }

        // POST: SignUps/Guest/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guest(int id, Guest guest)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var activity = await _context.Activity.SingleOrDefaultAsync(m => m.ActivityID == id);

                if (activity.NeedsSignUp)
                {
                    SignUp signup = new SignUp {
                        ActivityID = activity.ActivityID,
                        Guest = guest
                    };
                    
                    _context.Add(signup);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Calendar", "activities");
                }
                else
                {
                    return RedirectToAction("Details", "activities", new { id });
                }
            }

            return View(guest);
        }

        private bool SignUpExists(int signUpId)
        {
            return _context.SignUp.Any(e => e.SignUpID == signUpId);
        }

        private bool UserSignedUp(int activityId)
        {
            return _context.SignUp.Any(e => e.ActivityID == activityId && e.ApplicationUserID == GetCurrentUserAsync().Id);
        }

        private ApplicationUser GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User).Result;
        }
    }
}
