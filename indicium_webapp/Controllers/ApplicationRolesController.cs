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
    [Route("rollen"), Authorize(Roles = "Secretaris")]
    public class ApplicationRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRolesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: /rollen
        public async Task<IActionResult> Index()
        {
            var applicationRolesResult = await _context.ApplicationRole.ToListAsync();
            IEnumerable<ApplicationRoleViewModel> model = applicationRolesResult.Select(CreateApplicationRoleViewModel);

            return View(model);
        }

        // GET: /rollen/bewerken/{id}
        [Route("bewerken/{id}")]
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

        // POST: /rollen/bewerken/{id}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(string id, ApplicationRoleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CreateApplicationRole(model));
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

        private bool ApplicationRoleExists(string id)
        {
            return _context.ApplicationRole.Any(applicationRole => applicationRole.Id == id);
        }
        
        private ApplicationRole CreateApplicationRole(ApplicationRoleViewModel model)
        {
            ApplicationRole applicationRole = _context.ApplicationRole.Find(model.Id);
            
            applicationRole.Description = model.Description;

            return applicationRole;
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
