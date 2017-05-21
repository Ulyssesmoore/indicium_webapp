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

namespace indicium_webapp.Controllers
{
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
            return View(await _context.SignUp.ToListAsync());
        }

        // GET: SignUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUp
                .SingleOrDefaultAsync(m => m.SignUpID == id);
            if (signUp == null)
            {
                return NotFound();
            }

            return View(signUp);
        }

        // GET: SignUps/Create
        public IActionResult Create()
        {
            var ApplicationUserList = new List<SelectListItem>();
            var ApplicationUsers = from u in _context.ApplicationUser select u;

            ApplicationUserList.Add(new SelectListItem { Value = "", Text = "" });

            foreach (ApplicationUser user in ApplicationUsers)
            {
                ApplicationUserList.Add(new SelectListItem { Value = user.Id, Text = user.FirstName});
            }

            var ActivitiesList = new List<SelectListItem>();
            var Activities = from a in _context.Activity select a;

            ActivitiesList.Add(new SelectListItem { Value = "", Text = "" });

            foreach (Activity activity in Activities)
            {
                ActivitiesList.Add(new SelectListItem { Value = activity.ActivityID.ToString(), Text = activity.Name});
            }

            ViewBag.ApplicationUserID = ApplicationUserList;
            ViewBag.ActivityID = ActivitiesList;

            return View();
        }

        // POST: SignUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SignUpID,ActivityID")] SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();                
                signUp.ApplicationUserID = user.Id;
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

        // GET: SignUps/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: SignUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SignUpID,ActivityID,ApplicationUserID,Status")] SignUp signUp)
        {
            if (id != signUp.SignUpID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signUp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignUpExists(signUp.SignUpID))
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
            return View(signUp);
        }

        // GET: SignUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUp
                .SingleOrDefaultAsync(m => m.SignUpID == id);
            if (signUp == null)
            {
                return NotFound();
            }

            return View(signUp);
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signUp = await _context.SignUp.SingleOrDefaultAsync(m => m.SignUpID == id);
            _context.SignUp.Remove(signUp);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SignUpExists(int id)
        {
            return _context.SignUp.Any(e => e.SignUpID == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
