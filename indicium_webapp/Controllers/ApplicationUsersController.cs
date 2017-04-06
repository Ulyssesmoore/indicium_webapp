using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;

namespace indicium_webapp.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUser.ToListAsync());
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
        public async Task<IActionResult> Details(string id, [Bind("FirstName,LastName,Sex,Birthday,AddressStreet,AddressNumber,AddressPostalCode,AddressCity,AddressCountry,Iban,StudentNumber,StartdateStudy,StudyType,RegistrationDate,IsActive,Id,UserName,Email,ConcurrencyStamp,PhoneNumber")] ApplicationUser applicationUser)
        {
            _userManager Request.Form["UserName"]

            if (id != applicationUser.Id)
            {
                System.Diagnostics.Debug.WriteLine("This is that one error, to rule them all.");
                System.Diagnostics.Debug.WriteLine(id);
                System.Diagnostics.Debug.WriteLine(applicationUser.Id);
                return NotFound();
            }
                        
            try
            {
                System.Diagnostics.Debug.WriteLine("Updating IsApproved");
                applicationUser.IsApproved = 1;
                System.Diagnostics.Debug.WriteLine("Updated IsApproved");
                _context.Update(applicationUser);
                System.Diagnostics.Debug.WriteLine("Next step worked");
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("Final step worked");
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
