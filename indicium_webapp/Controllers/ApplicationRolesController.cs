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
using indicium_webapp.Models.ViewModels.AccountViewModels;

namespace indicium_webapp.Controllers
{
    [Authorize(Roles = "Secretaris")]
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
            var applicationRolesResult = await _context.ApplicationRole.ToListAsync();
            IEnumerable<ApplicationRoleViewModel> model = applicationRolesResult.Select(CreateApplicationRoleViewModel);

            return View(model);
        }

        // GET: ApplicationRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(CreateApplicationRole(model));
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRoleResult = await _context.ApplicationRole.SingleOrDefaultAsync(applicationRole => applicationRole.Id == id);

            if (applicationRoleResult == null)
            {
                return NotFound();
            }
            
            return View(CreateApplicationRoleViewModel(applicationRoleResult));
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationRoleViewModel model)
        {
            var applicationRole = _context.ApplicationRole.Find(id);

            if (id != model.Id || applicationRole == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationRole.Id = model.Id;
                    applicationRole.Name = model.Name;
                    applicationRole.Description = model.Description;
                    
                    _context.Update(applicationRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationRoleExists(model.Id))
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

        // GET: ApplicationRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRoleResult = await _context.ApplicationRole.SingleOrDefaultAsync(applicationRole => applicationRole.Id == id);
            
            if (applicationRoleResult == null)
            {
                return NotFound();
            }

            return View(CreateApplicationRoleViewModel(applicationRoleResult));
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationRoleResult = await _context.ApplicationRole.SingleOrDefaultAsync(applicationRole => applicationRole.Id == id);
            
            _context.ApplicationRole.Remove(applicationRoleResult);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.ApplicationRole.Any(applicationRole => applicationRole.Id == id);
        }

        private ApplicationRole CreateApplicationRole(ApplicationRoleViewModel model)
        {
            return new ApplicationRole {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }
        
        private ApplicationRoleViewModel CreateApplicationRoleViewModel(ApplicationRole applicationRole)
        {
            return new ApplicationRoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name,
                Description = applicationRole.Description
            };
        }
    }
}
