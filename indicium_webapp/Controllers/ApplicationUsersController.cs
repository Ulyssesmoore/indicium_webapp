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
            string sortOrder,
            string currentNameFilter,
            string currentStudyFilter,
            string nameFilter, 
            string studyFilter,            
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameFilter"] = nameFilter;
            ViewData["StudyFilter"] = studyFilter;

            ViewData["FirstNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";

            if (nameFilter != null || studyFilter != null)
            {
                page = 1;
            }
            else
            {
                nameFilter = currentNameFilter;
                studyFilter = currentStudyFilter;
            }

            var users = from u in _context.ApplicationUser select u;

            if (!String.IsNullOrEmpty(nameFilter))
            {
                users = users.Where(u => u.FirstName.Contains(nameFilter) || u.LastName.Contains(nameFilter));
            }

            if (!String.IsNullOrEmpty(studyFilter))
            {
                users = users.Where(u => u.StudyType.Equals(studyFilter));
            }

            switch (sortOrder)
            {
                case "firstname_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "lastname_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    break;
            }

            List<string> studyTypes = new List<string>
            {
                "BIM", 
                "SIE",
                "SNE",
                "TI",
                "Propedeuse",
            };

            //ViewData["StudyTypesList"] = new SelectList(studyTypes);

            //ViewData["studyTypesList"] = new List<SelectListItem>();

            var StudyTypesList = new List<SelectListItem>();
            StudyTypesList.Add(new SelectListItem { Value = "", Text = "" });

            foreach (string study in studyTypes)
            {
                if (study == studyFilter)
                {
                    StudyTypesList.Add(new SelectListItem { Value = study, Text = study });
                }
                else
                {
                    StudyTypesList.Add(new SelectListItem { Value = study, Text = study, Selected = true });
                }
            }

            ViewData["studyTypesList"] = StudyTypesList;


            int pageSize = 1;

            return View(await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: ApplicationUsers/Approval
        [Authorize(Roles = "Secretaris")]
        public async Task<IActionResult> Approval()
        {
            var users = from u in _context.ApplicationUser select u;

            users = users.OrderBy(u => u.RegistrationDate);

            return View(await users.AsNoTracking().ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Details/5
        [HttpPost]
        public async Task<IActionResult> Details(string id, int isApproved)
        {
            var newApplicationUser = _context.ApplicationUser.Find(id);
            if (newApplicationUser == null)
            {
                return NotFound();
            }

            System.Diagnostics.Debug.WriteLine(id);

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState Is Valid!");
                try
                {
                    if (isApproved == 1)
                    {
                        System.Diagnostics.Debug.WriteLine("IsApproved == 1!");
                        newApplicationUser.IsApproved = isApproved;

                        _context.Update(newApplicationUser);
                        await _context.SaveChangesAsync();
                    }
                    else if (isApproved == 2)
                    {
                        System.Diagnostics.Debug.WriteLine("IsApproved == 2!");
                        _context.Remove(newApplicationUser);
                        _context.SaveChanges();
                    }
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

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            //ViewData["allroles"] = _userManager.GetRolesAsync(applicationUser);
            IList<string> roles = new List<string>();
            ViewData["allroles"] = _context.ApplicationRole.ToListAsync().Result;
            ViewData["currentrole"] = _userManager.GetRolesAsync(applicationUser).Result[0];

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Sex,Birthday,AddressStreet,AddressNumber,AddressPostalCode,AddressCity,AddressCountry,Iban,StudentNumber,StartdateStudy,StudyType,IsActive,Id,PhoneNumber")] ApplicationUser applicationUser)
        {
            var newApplicationUser = _context.ApplicationUser.Find(applicationUser.Id);
            if (id != applicationUser.Id || newApplicationUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    newApplicationUser.FirstName = applicationUser.FirstName;
                    newApplicationUser.LastName = applicationUser.LastName;
                    newApplicationUser.Sex = applicationUser.Sex;
                    newApplicationUser.Birthday = applicationUser.Birthday;
                    newApplicationUser.AddressStreet = applicationUser.AddressStreet;
                    newApplicationUser.AddressNumber = applicationUser.AddressNumber;
                    newApplicationUser.AddressPostalCode = applicationUser.AddressPostalCode;
                    newApplicationUser.AddressCity = applicationUser.AddressCity;
                    newApplicationUser.AddressCountry = applicationUser.AddressCountry;
                    newApplicationUser.Iban = applicationUser.Iban;
                    newApplicationUser.StudentNumber = applicationUser.StudentNumber;
                    newApplicationUser.StartdateStudy = applicationUser.StartdateStudy;
                    newApplicationUser.StudyType = applicationUser.StudyType;
                    newApplicationUser.IsActive = applicationUser.IsActive;
                    newApplicationUser.PhoneNumber = applicationUser.PhoneNumber;

                    _context.Update(newApplicationUser);
                    await _context.SaveChangesAsync();

                    string roleValue;

                    if (!string.IsNullOrEmpty(Request.Form["userrole"]))
                    {
                        roleValue = Request.Form["userrole"];

                        await _userManager.RemoveFromRoleAsync(newApplicationUser, _userManager.GetRolesAsync(newApplicationUser).Result[0]);
                        await _userManager.AddToRoleAsync(newApplicationUser, roleValue);

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
            return View(applicationUser);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
