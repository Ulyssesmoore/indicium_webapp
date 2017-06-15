using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using indicium_webapp.Models.AccountViewModels;

namespace indicium_webapp.Controllers
{
    [Authorize(Roles = "Bestuur, Secretaris")]
    public class ApplicationRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRolesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ApplicationRoles
        public async Task<IActionResult> Index()
        {
            var applicationrole = await _context.ApplicationRole.ToListAsync();

            IEnumerable<ApplicationRoleViewModel> applicationroleviewmodel = applicationrole.Select(role => new ApplicationRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            });

            return View(applicationroleviewmodel);
        }

        // GET: ApplicationRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationrole = await _context.ApplicationRole
                .SingleOrDefaultAsync(m => m.Id == id);
            
            if (applicationrole == null)
            {
                return NotFound();
            }

            ApplicationRoleViewModel applicationroleviewmodel = new ApplicationRoleViewModel
            {
                Id = applicationrole.Id,
                Name = applicationrole.Name,
                Description = applicationrole.Description
            };

            return View(applicationroleviewmodel);
        }

        // GET: ApplicationRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Id,Name,NormalizedName,ConcurrencyStamp")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationRole);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationrole = await _context.ApplicationRole.SingleOrDefaultAsync(m => m.Id == id);

            if (applicationrole == null)
            {
                return NotFound();
            }
            
            ApplicationRoleViewModel applicationroleviewmodel = new ApplicationRoleViewModel
            {
                Id = applicationrole.Id,
                Name = applicationrole.Name,
                Description = applicationrole.Description
            };

            return View(applicationroleviewmodel);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationRoleViewModel applicationroleviewmodel)
        {
            var newApplicationRole = _context.ApplicationRole.Find(id);
            if (id != applicationroleviewmodel.Id || newApplicationRole == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    newApplicationRole.Id = applicationroleviewmodel.Id;
                    newApplicationRole.Name = applicationroleviewmodel.Name;
                    newApplicationRole.Description = applicationroleviewmodel.Description;
                    
                    _context.Update(newApplicationRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ApplicationRole.Any(e => e.Id == applicationroleviewmodel.Id))
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
            return View(applicationroleviewmodel);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationrole = await _context.ApplicationRole.SingleOrDefaultAsync(m => m.Id == id);
            
            if (applicationrole == null)
            {
                return NotFound();
            }

            ApplicationRoleViewModel applicationroleviewmodel = new ApplicationRoleViewModel
            {
                Id = applicationrole.Id,
                Name = applicationrole.Name,
                Description = applicationrole.Description
            };

            return View(applicationroleviewmodel);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationRole = await _context.ApplicationRole.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationRole.Remove(applicationRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.ApplicationRole.Any(e => e.Id == id);
        }

    }
}
