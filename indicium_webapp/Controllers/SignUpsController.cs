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

namespace indicium_webapp.Controllers
{
    [Authorize]
    public class SignUpsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SignUps
        public async Task<IActionResult> Index()
        {
            return View(await _context.SignUp
                .Where(m => m.ApplicationUserID == GetCurrentUserAsync().Result.Id)
                .ToListAsync());
        }

        // GET: SignUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUp.SingleOrDefaultAsync(m => m.SignUpID == id);
            
            if (signUp == null)
            {
                return NotFound();
            }

            return View(signUp);
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
                signUp.ApplicationUserID = GetCurrentUserAsync().Result.Id;
                signUp.ActivityID = id;

                _context.Add(signUp);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("\n\nModelState Invalid\n\n");
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

            var signUpResult = await _context.SignUp.SingleOrDefaultAsync(m => m.ActivityID == id && m.ApplicationUserID == GetCurrentUserAsync().Result.Id);
            
            if (signUpResult == null)
            {
                return NotFound();
            }

            return View(signUpResult);
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signUp = await _context.SignUp.SingleOrDefaultAsync(m => m.ActivityID == id && m.ApplicationUserID == GetCurrentUserAsync().Result.Id);

            _context.SignUp.Remove(signUp);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool SignUpExists(int id)
        {
            return _context.SignUp.Any(e => e.SignUpID == id);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
