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
    [Authorize(Roles = "Bestuur")]
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
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["FirstNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            var users = from u in _context.ApplicationUser select u;

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
        public async Task<IActionResult> Details(string id, [Bind("FirstName,LastName,Sex,Birthday,AddressStreet,AddressNumber,AddressPostalCode,AddressCity,AddressCountry,Iban,StudentNumber,StartdateStudy,StudyType,RegistrationDate,IsActive,IsApproved,Id,UserName,Email,ConcurrencyStamp,PhoneNumber")] ApplicationUser applicationUser)
        {
            /*
            db.
            _userManager.GetUserId Request.Form["UserName"];
            */
            if (id != applicationUser.Id)
            {
                System.Diagnostics.Debug.WriteLine("This is that one error, to rule them all.");
                System.Diagnostics.Debug.WriteLine(id);
                System.Diagnostics.Debug.WriteLine(applicationUser.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (applicationUser.IsApproved == 1)
                    {
                        _context.Update(applicationUser);
                        System.Diagnostics.Debug.WriteLine("Next step worked");
                        await _context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine("Final step worked");
                    } else if (applicationUser.IsApproved == 2 ){
                        _context.Remove(applicationUser);
                        _context.SaveChanges();
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
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Sex,Birthday,AddressStreet,AddressNumber,AddressPostalCode,AddressCity,AddressCountry,Iban,StudentNumber,StartdateStudy,StudyType,RegistrationDate,IsActive,Id,UserName,Email,ConcurrencyStamp,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();

                    string roleValue;

                    if (!string.IsNullOrEmpty(Request.Form["userrole"]))
                    {
                        roleValue = Request.Form["userrole"];

                        await _userManager.RemoveFromRoleAsync(applicationUser, _userManager.GetRolesAsync(applicationUser).Result[0]);
                        await _userManager.AddToRoleAsync(applicationUser, roleValue);
                        
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
