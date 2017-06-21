using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Data;
using indicium_webapp.Models;
using indicium_webapp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace indicium_webapp.Controllers
{
    [Route("commissies"), Authorize(Roles = "Bestuur, Secretaris")]
    public class CommissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommissionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: /commissies
        public async Task<IActionResult> Index()
        {
            var commissions = await _context.Commission.ToListAsync();
            IEnumerable<CommissionViewModel> model = commissions.Select(CreateCommissionViewModel);

            return View(model);
        }

        // GET: /commissies/details/{id}
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commissionResult = await _context.Commission.SingleOrDefaultAsync(commission => commission.CommissionID == id);
            
            if (commissionResult == null)
            {
                return NotFound();
            }

            CommissionViewModel model = CreateCommissionViewModel(commissionResult);

            var members = await _context.CommissionMember
                .Where(commissionMember => commissionMember.CommissionID == commissionResult.CommissionID && commissionMember.Status == CommisionMemberStatus.Lid)
                .ToListAsync();

            foreach (var member in members)
            {
                model.CommissionMembers.Add(member);
                model.Members
                    .Add(_context.ApplicationUser
                    .Find(member.ApplicationUserID));
            }

            return View(model);
        }

        // GET: /commissies/aanmaken
        [Route("aanmaken")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /commissies/aanmaken
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("aanmaken")]
        public async Task<IActionResult> Create(CommissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(CreateCommission(model));
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /commissies/bewerken/{id}
        [Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commissionResult = await _context.Commission.SingleOrDefaultAsync(commission => commission.CommissionID == id);
            
            if (commissionResult == null)
            {
                return NotFound();
            }

            return View(CreateCommissionViewModel(commissionResult));
        }

        // POST: /commissies/bewerken/{id}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Route("bewerken/{id}")]
        public async Task<IActionResult> Edit(int id, CommissionViewModel model)
        {
            if (id != model.CommissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CreateCommission(model));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommissionExists(model.CommissionID))
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

        // GET: /commissies/verwijderen/{id}
        [Route("verwijderen/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commissionResult = await _context.Commission.SingleOrDefaultAsync(commission =>commission.CommissionID == id);
            
            if (commissionResult == null)
            {
                return NotFound();
            }

            return View(CreateCommissionViewModel(commissionResult));
        }

        // POST: /commissies/verwijderen/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Route("verwijderen/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commissionResult = await _context.Commission.SingleOrDefaultAsync(commission => commission.CommissionID == id);
            
            _context.Commission.Remove(commissionResult);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: /commissies/aanvragen
        [Route("aanvragen")]
        public async Task<IActionResult> Approval()
        {
            var commissionsResult = await _context.Commission.ToListAsync();
            var commissionViewModels = new List<CommissionViewModel>();

            foreach (var commissionItem in commissionsResult)
            {
                CommissionViewModel model = CreateCommissionViewModel(commissionItem);

                var commissionMembersResult = await _context.CommissionMember
                    .Where(comissionMember => comissionMember.CommissionID == commissionItem.CommissionID && comissionMember.Status == CommisionMemberStatus.Interesse)
                    .ToListAsync();

                // Skip commissions who don't have any members to approve
                if (commissionMembersResult.Where(comissionMember => comissionMember.Status == CommisionMemberStatus.Interesse).Count() > 0)
                {
                    foreach (var member in commissionMembersResult)
                    {
                        ApplicationUser applicationUser = _context.ApplicationUser.Find(member.ApplicationUserID);

                        // Don't make it possible to approve members who haven't been approved yet as a user
                        if (applicationUser.Status == Status.Lid)
                            model.Members.Add(applicationUser);
                            model.CommissionMembers.Add(member);
                    }

                    commissionViewModels.Add(model);
                }              
            }

            return View(commissionViewModels);
        }

        // POST: /commissies/aanvraag-goedkeuren/{id}
        [Route("aanvraag-goedkeuren/{id}")]
        public async Task<IActionResult> ApproveMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommissionMember commissionMemberResult = await _context.CommissionMember.SingleOrDefaultAsync(comissionMember => comissionMember.CommissionMemberID == id);

            if (commissionMemberResult == null)
            {
                return NotFound();
            }

            commissionMemberResult.Status = CommisionMemberStatus.Lid;

            try
            {
                _context.Update(commissionMemberResult);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionMemberExists(commissionMemberResult.CommissionMemberID))
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

        // POST: /commissies/aanvraag-afkeuren/{id}
        [Route("aanvraag-afkeuren/{id}")]
        public async Task<IActionResult> DisapproveMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommissionMember commissionMemberResult = await _context.CommissionMember.SingleOrDefaultAsync(commissionMember => commissionMember.CommissionMemberID == id);

            if (commissionMemberResult == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(commissionMemberResult);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionMemberExists(commissionMemberResult.CommissionMemberID))
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

        private bool CommissionExists(int id)
        {
            return _context.Commission.Any(comission => comission.CommissionID == id);
        }

        private bool CommissionMemberExists(int id)
        {
            return _context.CommissionMember.Any(comissionMember => comissionMember.CommissionMemberID == id);
        }

        private Commission CreateCommission(CommissionViewModel model)
        {
            return new Commission
            {
                CommissionID = model.CommissionID,
                Name = model.Name,
                Description = model.Description,
            };
        }

        private CommissionViewModel CreateCommissionViewModel(Commission commission)
        {
            return new CommissionViewModel
            {
                CommissionID = commission.CommissionID,
                Name = commission.Name,
                Description = commission.Description,
            };
        }
    }
}
