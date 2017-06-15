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

namespace indicium_webapp.Controllers
{
    public class CommissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommissionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Commissions
        public async Task<IActionResult> Index()
        {
            var commissions = await _context.Commission.ToListAsync();

            IEnumerable<CommissionViewModel> commissionModels = commissions.Select(commission => new CommissionViewModel
            {
                CommissionID = commission.CommissionID,
                Name = commission.Name,
                Description = commission.Description
            });

            return View(commissionModels);
        }

        // GET: Commissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commission = await _context.Commission
                .SingleOrDefaultAsync(m => m.CommissionID == id);
            if (commission == null)
            {
                return NotFound();
            }

            CommissionViewModel commissionViewModel = createCommissionViewModel(commission);

            var members = await _context.CommissionMember.Where(c => c.CommissionID == commission.CommissionID && c.Status == CommisionMemberStatus.Lid)
                .ToListAsync();

            foreach (var member in members)
            {
                commissionViewModel.CommissionMembers.Add(member);
                commissionViewModel.Members.Add(_context.ApplicationUser.Find(member.ApplicationUserID));
            }

            return View(commissionViewModel);
        }

        // GET: Commissions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Commissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommissionViewModel commissionViewModel)
        {
            if (ModelState.IsValid)
            {
                var commission = createCommission(commissionViewModel);
                _context.Add(commission);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(commissionViewModel);
        }

        // GET: Commissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commission = await _context.Commission.SingleOrDefaultAsync(m => m.CommissionID == id);
            if (commission == null)
            {
                return NotFound();
            }
            return View(createCommissionViewModel(commission));
        }

        // POST: Commissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommissionViewModel commissionViewModel)
        {
            Commission commission = createCommission(commissionViewModel);

            if (id != commission.CommissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommissionExists(commission.CommissionID))
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
            return View(commission);
        }

        // GET: Commissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commission = await _context.Commission
                .SingleOrDefaultAsync(m => m.CommissionID == id);
            if (commission == null)
            {
                return NotFound();
            }

            return View(createCommissionViewModel(commission));
        }

        // POST: Commissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commission = await _context.Commission.SingleOrDefaultAsync(m => m.CommissionID == id);
            _context.Commission.Remove(commission);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Commissions/ApproveOverview
        public async Task<IActionResult> ApproveOverview()
        {
            var commissions = await _context.Commission.ToListAsync();
            var commissionViewModels = new List<CommissionViewModel>();

            foreach (var commission in commissions)
            {
                CommissionViewModel commissionViewModel = createCommissionViewModel(commission);

                var members = await _context.CommissionMember.Where(c => c.CommissionID == commission.CommissionID && c.Status == CommisionMemberStatus.Interesse)
                    .ToListAsync();

                // Skip commissions who don't have any members to approve
                if (members.Where(m => m.Status == CommisionMemberStatus.Interesse).Count() > 0)
                {
                    foreach (var member in members)
                    {
                        ApplicationUser applicationUser = _context.ApplicationUser.Find(member.ApplicationUserID);

                        // Don't make it possible to approve members who haven't been approved yet as a user
                        if (applicationUser.Status == Status.Lid)
                            commissionViewModel.Members.Add(applicationUser);
                            commissionViewModel.CommissionMembers.Add(member);
                    }

                    commissionViewModels.Add(commissionViewModel);
                }              
            }

            return View(commissionViewModels);
        }

        // POST: Commissions/ApproveMember
        public async Task<IActionResult> ApproveMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommissionMember commissionMember = await _context.CommissionMember.SingleOrDefaultAsync(m => m.CommissionMemberID == id);

            if (commissionMember == null)
            {
                return NotFound();
            }

            commissionMember.Status = CommisionMemberStatus.Lid;

            try
            {
                _context.Update(commissionMember);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionMemberExists(commissionMember.CommissionMemberID))
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

        // POST: Commissions/ApproveMember
        public async Task<IActionResult> DisapproveMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommissionMember commissionMember = await _context.CommissionMember.SingleOrDefaultAsync(m => m.CommissionMemberID == id);

            if (commissionMember == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(commissionMember);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionMemberExists(commissionMember.CommissionMemberID))
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
            return _context.Commission.Any(e => e.CommissionID == id);
        }

        private bool CommissionMemberExists(int id)
        {
            return _context.CommissionMember.Any(m => m.CommissionMemberID == id);
        }

        private CommissionViewModel createCommissionViewModel(Commission commission)
        {
            return new CommissionViewModel
            {
                CommissionID = commission.CommissionID,
                Name = commission.Name,
                Description = commission.Description,
            };
        }

        private Commission createCommission(CommissionViewModel commissionViewModel)
        {
            return new Commission
            {
                CommissionID = commissionViewModel.CommissionID,
                Name = commissionViewModel.Name,
                Description = commissionViewModel.Description,
            };
        }
    }
}
