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
using System.Globalization;
using indicium_webapp.Models.InterfaceItemModels;
using indicium_webapp.Models.ViewModels.AccountViewModels;

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
        public async Task<IActionResult> Index(string studyTypesList, string nameFilter, string studyFilter, string statusFilter)
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
            else
            {
                // If there isn't a status filter, filter out these members, cause they are cluttering the desired results
                users = users.Where(u => u.Status != Status.Nieuw).Where(u => u.Status != Status.Uitgeschreven);
            }

            var applicationUsersResult = await users.AsNoTracking().ToListAsync();
            IEnumerable<ApplicationUserViewModel> model = applicationUsersResult.Select(CreateApplicationUserViewModel);

            return View(model);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserResult = await _context.ApplicationUser.SingleOrDefaultAsync(applicationUser => applicationUser.Id == id);
            
            if (applicationUserResult == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _userManager.GetRolesAsync(applicationUserResult);

            return View(CreateApplicationUserViewModel(applicationUserResult));
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserResult = await _context.ApplicationUser.SingleOrDefaultAsync(applicationUser => applicationUser.Id == id);

            if (applicationUserResult == null)
            {
                return NotFound();
            }

            var checkBoxListItems = new List<CheckBoxListItem>();
            foreach (var role in _context.ApplicationRole.ToListAsync().Result)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = role.Id,
                    Display = role.Name,
                    IsChecked = _userManager.IsInRoleAsync(applicationUserResult, role.Name).Result
                });
            }
            
            ApplicationUserViewModel model = CreateApplicationUserViewModel(applicationUserResult);
            model.Roles = checkBoxListItems;

            return View(model);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Edit(string id, ApplicationUserViewModel model)
        {
            var applicationUser = _context.ApplicationUser.Find(id);
            
            if (id != model.Id || applicationUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.FirstName = model.FirstName;
                    applicationUser.LastName = model.LastName;
                    applicationUser.Sex = (Sex) Convert.ToInt32(model.Sex);
                    applicationUser.Birthday = DateTime.ParseExact(model.Birthday, "dd-MM-yyyy", new CultureInfo("nl-NL"));
                    applicationUser.AddressStreet = model.AddressStreet;
                    applicationUser.AddressNumber = model.AddressNumber;
                    applicationUser.AddressPostalCode = model.AddressPostalCode;
                    applicationUser.AddressCity = model.AddressCity;
                    applicationUser.AddressCountry = model.AddressCountry;
                    applicationUser.StudentNumber = Convert.ToInt32(model.StudentNumber);
                    applicationUser.StartdateStudy =Int32.Parse(model.StartdateStudy);
                    applicationUser.StudyType = (StudyType) Convert.ToInt32(model.StudyType);
                    applicationUser.PhoneNumber = model.PhoneNumber;
                    applicationUser.Status = model.Status;
                    
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();

                    foreach (var role in model.Roles.ToList())
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
                    if (!ApplicationUserExists(model.Id))
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

        // GET: ApplicationUsers/Approval
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Approval()
        {
            var users = from u in _context.ApplicationUser select u;

            users = users.OrderBy(user => user.RegistrationDate);

            var applicationUsersResult = await users.AsNoTracking()
                .Where(applicationUser => applicationUser.Status == Status.Nieuw)
                .ToListAsync();

            return View(applicationUsersResult.Select(CreateApplicationUserViewModel));
        }
        
        // POST: ApplicationUsers/Approve/5
        [HttpPost, Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Approve(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await ApproveApplicationUser(id, Status.Lid);
            
            return RedirectToAction("Approval");
        }

        // POST: ApplicationUsers/Disapprove/5
        [HttpPost, Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Disapprove(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await ApproveApplicationUser(id, Status.Afgekeurd);
            
            return RedirectToAction("Approval");
        }

        private async Task<bool> ApproveApplicationUser(string id, Status status)
        {
            var result = false;
            var applicationUserResult = await _context.ApplicationUser.SingleOrDefaultAsync(applicationUser => applicationUser.Id == id);            

            try
            {
                applicationUserResult.Status = status;

                _context.Update(applicationUserResult);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return result;
        }
        
        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        private ApplicationUser CreateApplicationUser(ApplicationUserViewModel model)
        {
            return new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Sex = (Sex) Convert.ToInt32(model.Sex),
                Birthday = DateTime.ParseExact(model.Birthday, "dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = model.AddressStreet,
                AddressNumber = model.AddressNumber,
                AddressPostalCode = model.AddressPostalCode,
                AddressCity = model.AddressCity,
                AddressCountry = model.AddressCountry,
                StudentNumber = Convert.ToInt32(model.StudentNumber),
                StartdateStudy = Int32.Parse(model.StartdateStudy),
                StudyType = (StudyType) Convert.ToInt32(model.StudyType),
                PhoneNumber = model.PhoneNumber,
                Status = model.Status
            };
        }
        
        private ApplicationUserViewModel CreateApplicationUserViewModel(ApplicationUser applicationUser)
        {
            return new ApplicationUserViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Sex = applicationUser.Sex.ToString(),
                Birthday = applicationUser.Birthday.ToString("dd-MM-yyyy", new CultureInfo("nl-NL")),
                AddressStreet = applicationUser.AddressStreet,
                AddressNumber = applicationUser.AddressNumber,
                AddressPostalCode = applicationUser.AddressPostalCode,
                AddressCity = applicationUser.AddressCity,
                AddressCountry = applicationUser.AddressCountry,
                StudentNumber = applicationUser.StudentNumber.ToString(),
                StartdateStudy = applicationUser.StartdateStudy.ToString(),
                StudyType = applicationUser.StudyType.ToString(),
                PhoneNumber = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                Status = applicationUser.Status,
                RegistrationDate = applicationUser.RegistrationDate
            };
        }
    }
}
