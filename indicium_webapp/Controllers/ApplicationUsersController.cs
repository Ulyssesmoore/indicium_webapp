using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using indicium_webapp.Models.AccountViewModels;
using System.Globalization;
using indicium_webapp.Models.InterfaceItemModels;

namespace indicium_webapp.Controllers
{
    [Authorize(Roles = "Bestuur, Secretaris")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index(
            string studyTypesList,
            string nameFilter, 
            string studyFilter,
            string statusFilter)
        {
            ViewData["NameFilter"] = nameFilter;
            ViewData["StudyFilter"] = studyFilter;
            ViewData["StatusFilter"] = statusFilter;

            var users = from u in _context.ApplicationUser select u;

            if (!String.IsNullOrEmpty(nameFilter))
            {
                // Names are converted to lowercase to make filtering case-insenitive
                users = users.Where(u => u.FirstName.ToLower().Contains(nameFilter.ToLower()) || u.LastName.ToLower().Contains(nameFilter.ToLower()));
            }

            if (!String.IsNullOrEmpty(studyFilter))
            {
                // Cast string to enum and filter
                users = users.Where(u => u.StudyType.Equals((StudyType) Enum.Parse(typeof(StudyType), studyFilter)));
            }

            if (!String.IsNullOrEmpty(statusFilter))
            {
                // Cast string to enum and filter
                users = users.Where(u => u.Status.Equals((Status)Enum.Parse(typeof(Status), statusFilter)));
            }

            var applicationusers = await users.AsNoTracking().ToListAsync();

            IEnumerable<ApplicationUserViewModel> applicationuserviewmodel = applicationusers.Select(applicationuser => new ApplicationUserViewModel
            {
                Id = applicationuser.Id,
                FirstName = applicationuser.FirstName,
                LastName = applicationuser.LastName,
                Sex = applicationuser.Sex.ToString(),
                Birthday = applicationuser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = applicationuser.AddressStreet,
                AddressNumber = applicationuser.AddressNumber,
                AddressPostalCode = applicationuser.AddressPostalCode,
                AddressCity = applicationuser.AddressCity,
                AddressCountry = applicationuser.AddressCountry,
                Iban = applicationuser.Iban,
                StudentNumber = applicationuser.StudentNumber.ToString(),
                StartdateStudy = applicationuser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                StudyType = applicationuser.StudyType.ToString(),
                PhoneNumber = applicationuser.PhoneNumber
            });

            return View(applicationuserviewmodel);
        }

        // GET: ApplicationUsers/Approval
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Approval()
        {
            var users = from u in _context.ApplicationUser select u;

            users = users.OrderBy(u => u.RegistrationDate);

            var applicationusers = await users.AsNoTracking()
                .Where(x => x.Status == Status.Nieuw)
                .ToListAsync();

            IEnumerable<ApplicationUserViewModel> applicationuserviewmodel = applicationusers.Select(applicationsuser => new ApplicationUserViewModel
            {
                Id = applicationsuser.Id,
                FirstName = applicationsuser.FirstName,
                LastName = applicationsuser.LastName,
                Sex = applicationsuser.Sex.ToString(),
                Birthday = applicationsuser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = applicationsuser.AddressStreet,
                AddressNumber = applicationsuser.AddressNumber,
                AddressPostalCode = applicationsuser.AddressPostalCode,
                AddressCity = applicationsuser.AddressCity,
                AddressCountry = applicationsuser.AddressCountry,
                Iban = applicationsuser.Iban,
                StudentNumber = applicationsuser.StudentNumber.ToString(),
                StartdateStudy = applicationsuser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                StudyType = applicationsuser.StudyType.ToString(),
                PhoneNumber = applicationsuser.PhoneNumber
            });

            return View(applicationuserviewmodel);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            
            if (applicationuser == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel applicationuserviewmodel = new ApplicationUserViewModel {
                Id = applicationuser.Id,
                FirstName = applicationuser.FirstName,
                LastName = applicationuser.LastName,
                Sex = applicationuser.Sex.ToString(),
                Birthday = applicationuser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = applicationuser.AddressStreet,
                AddressNumber = applicationuser.AddressNumber,
                AddressPostalCode = applicationuser.AddressPostalCode,
                AddressCity = applicationuser.AddressCity,
                AddressCountry = applicationuser.AddressCountry,
                Iban = applicationuser.Iban,
                StudentNumber = applicationuser.StudentNumber.ToString(),
                StartdateStudy = applicationuser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                StudyType = applicationuser.StudyType.ToString(),
                PhoneNumber = applicationuser.PhoneNumber,
                Status = applicationuser.Status
            };

            return View(applicationuserviewmodel);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);

            if (applicationuser == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel applicationuserviewmodel = new ApplicationUserViewModel {
                Id = applicationuser.Id,
                FirstName = applicationuser.FirstName,
                LastName = applicationuser.LastName,
                Sex = applicationuser.Sex.ToString(),
                Birthday = applicationuser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = applicationuser.AddressStreet,
                AddressNumber = applicationuser.AddressNumber,
                AddressPostalCode = applicationuser.AddressPostalCode,
                AddressCity = applicationuser.AddressCity,
                AddressCountry = applicationuser.AddressCountry,
                Iban = applicationuser.Iban,
                StudentNumber = applicationuser.StudentNumber.ToString(),
                StartdateStudy = applicationuser.StartdateStudy.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                StudyType = applicationuser.StudyType.ToString(),
                PhoneNumber = applicationuser.PhoneNumber,
                Email = applicationuser.Email,
                Status = applicationuser.Status
            };

            var checkBoxListItems = new List<CheckBoxListItem>();
            foreach (var role in _context.ApplicationRole.ToListAsync().Result)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = role.Id,
                    Display = role.Name,
                    IsChecked = _userManager.IsInRoleAsync(applicationuser, role.Name).Result
                });
            }
            applicationuserviewmodel.Roles = checkBoxListItems;

            return View(applicationuserviewmodel);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUserViewModel applicationuserviewmodel)
        {
            var applicationUser = _context.ApplicationUser.Find(id);
            if (id != applicationuserviewmodel.Id || applicationUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.FirstName = applicationuserviewmodel.FirstName;
                    applicationUser.LastName = applicationuserviewmodel.LastName;
                    applicationUser.Sex = (Sex) Convert.ToInt32(applicationuserviewmodel.Sex);
                    applicationUser.Birthday = DateTime.ParseExact(applicationuserviewmodel.Birthday, "dd-MM-yyyy", new CultureInfo("nl-NL"));
                    applicationUser.AddressStreet = applicationuserviewmodel.AddressStreet;
                    applicationUser.AddressNumber = applicationuserviewmodel.AddressNumber;
                    applicationUser.AddressPostalCode = applicationuserviewmodel.AddressPostalCode;
                    applicationUser.AddressCity = applicationuserviewmodel.AddressCity;
                    applicationUser.AddressCountry = applicationuserviewmodel.AddressCountry;
                    applicationUser.Iban = applicationuserviewmodel.Iban;
                    applicationUser.StudentNumber = Convert.ToInt32(applicationuserviewmodel.StudentNumber);
                    applicationUser.StartdateStudy = DateTime.ParseExact(applicationuserviewmodel.StartdateStudy, "dd-MM-yyyy", new CultureInfo("nl-NL"));
                    applicationUser.StudyType = (StudyType) Convert.ToInt32(applicationuserviewmodel.StudyType);
                    applicationUser.PhoneNumber = applicationuserviewmodel.PhoneNumber;
                    
                    var getCurrentUserRole = _userManager.GetRolesAsync(GetCurrentUserAsync()).Result[0];
                    if (getCurrentUserRole == "Secretaris")
                    {
                        applicationUser.Status = applicationuserviewmodel.Status;
                    }

                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();

                    foreach (var role in applicationuserviewmodel.Roles.ToList())
                    {
                        // Get the role values from the database, because Name has dissappeared from 
                        // the above role and we need name for the methods below
                        var dbRole = await _context.Roles.FindAsync(role.ID);
                        
                        if (role.IsChecked)
                        {
                            if (!await _userManager.IsInRoleAsync(applicationUser, dbRole.Name))
                            {
                                await _userManager.AddToRoleAsync(applicationUser, dbRole.Name);
                            }
                        }
                        else
                        {
                            if (await _userManager.IsInRoleAsync(applicationUser, dbRole.Name))
                            {
                                await _userManager.RemoveFromRoleAsync(applicationUser, dbRole.Name);
                            }
                        }

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationuserviewmodel.Id))
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
            
            return View(applicationuserviewmodel);
        }

        // POST: ApplicationUsers/Approve/5
        [HttpPost]
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Approve(string id)
        {
            var newApplicationUser = _context.ApplicationUser.Find(id);
            if (newApplicationUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    newApplicationUser.Status = Status.Lid;

                    _context.Update(newApplicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Approval");
            }
            return View(newApplicationUser);
        }

        // POST: ApplicationUsers/Disapprove/5
        [HttpPost]
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Disapprove(string id)
        {
            var newApplicationUser = _context.ApplicationUser.Find(id);
            if (newApplicationUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    newApplicationUser.Status = Status.Afgekeurd;

                    _context.Update(newApplicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Approval");
            }
            return View(newApplicationUser);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        private ApplicationUser GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User).Result;
        }

    }
}
