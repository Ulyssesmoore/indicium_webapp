using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using indicium_webapp.Models;
using indicium_webapp.Models.ViewModels.ManageViewModels;
using indicium_webapp.Services;
using indicium_webapp.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace indicium_webapp.Controllers
{
    [Authorize]
    [Route("profiel")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IEmailSender emailSender, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeEmailSuccess,
            ChangeEducationalInformationSuccess,
            ChangePersonalInformationSuccess,
            ChangeAddressInformationSuccess,
            ChangePhoneNumberSuccess,
            Error
        }

        //
        // GET: /profiel
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Je wachtwoord is veranderd."
                : message == ManageMessageId.Error ? "Er heeft een fout plaatsgevonden."
                : message == ManageMessageId.ChangeEmailSuccess ? "Er is een e-mail gestuurd ter bevestiging."
                : message == ManageMessageId.ChangePersonalInformationSuccess ? "Je persoonlijke gegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeEducationalInformationSuccess ? "Je studiegegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeAddressInformationSuccess ? "Je adresgegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangePhoneNumberSuccess ? "Je telefoonnummer is met succes gewijzigd."
                : "";

            var applicationUserResult = await _context.ApplicationUser
                .Include(applicationUser => applicationUser.Commissions)
                    .ThenInclude(commission => commission.Commission)
                .SingleOrDefaultAsync(applicationUser => applicationUser.Id == GetCurrentUserAsync().Result.Id);

            if (applicationUserResult == null)
            {
                return View("Error");
            }

            var model = new IndexViewModel
            {
                FirstName = applicationUserResult.FirstName,
                LastName = applicationUserResult.LastName,
                Email = applicationUserResult.Email,
                AddressCity = applicationUserResult.AddressCity,
                AddressPostalCode = applicationUserResult.AddressPostalCode,
                AddressNumber = applicationUserResult.AddressNumber,
                AddressStreet = applicationUserResult.AddressStreet,
                AddressCountry = applicationUserResult.AddressCountry,
                PhoneNumber = applicationUserResult.PhoneNumber,
                StartdateStudy = applicationUserResult.StartdateStudy,
                StudyType = applicationUserResult.StudyType,
                Sex = applicationUserResult.Sex,
                Birthday = applicationUserResult.Birthday.ToString("dd MMMM yyyy", new CultureInfo("nl-NL")),
                StudentNumber = applicationUserResult.StudentNumber.ToString(),
                Commissions = applicationUserResult.Commissions
            };
            
            return View(model);
        }

        //
        // GET: /profiel/wijzig-wachtwoord
        [HttpGet, Route("wijzig-wachtwoord")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /profiel/wijzig-wachtwoord
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-wachtwoord")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /profiel/lidmaatschap-opzeggen
        [HttpGet, Route("lidmaatschap-opzeggen")]
        public IActionResult AbortMembership()
        {
            return View();
        }

        //
        // POST: /profiel/lidmaatschap-opzeggen
        [HttpPost, ActionName("AbortMembership"), ValidateAntiForgeryToken, Route("lidmaatschap-opzeggen")]
        public async Task<IActionResult> ConfirmedAbortMembership()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                // Sets user's status to "Uitgeschreven" and logs out the user
                user.Status = Status.Uitgeschreven;
                await _signInManager.SignOutAsync();

                // Saves changes
                _context.Update(user);
                await _context.SaveChangesAsync();

                // Send automatic email to the users emailaddress
                await _emailSender.SendEmailAsync(user.Email, "Bevestiging uitschrijving",
                    "Je bent uitgeschreven!");

                // User has no business in the application anymore so they will be redirected to the homepage. 
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /profiel/wijzig-email
        [HttpGet, Route("wijzig-email")]
        public async Task<IActionResult> ChangeEmail()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new ChangeEmailViewModel
            {
                Email = user.Email
            };
            return View(model);
        }

        //
        // POST: /profiel/wijzig-email
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-email")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                try
                {
                    // Generates a token, overwrites values in currently logged in user to match the new data
                    var email = model.Email.ToLower();
                    var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
                    user.NewEmail = email;

                    // Saves changes
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeEmailSuccess });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ApplicationUser.Any(e => e.Id == user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /profiel/wijzig-email
        [HttpGet, Route("bevestig-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            //if (userId == null || code == null)
            //{
            //    return View("Error");
            //}
            //var user = await _userManager.FindByIdAsync(userId);
            //var user = GetCurrentUserAsync();
            //if (user == null)
            //{
            //    return View("Error");
            //}
            //var result = await _userManager.ConfirmEmailAsync(user, code);


            //await _userManager.ChangeEmailAsync(user, model.Email, token);
            //user.UserName = model.Email;
            //await _userManager.UpdateNormalizedEmailAsync(user);
            //await _userManager.UpdateNormalizedUserNameAsync(user);

            //// Relogs user to refresh the _LoginPatial.cshtml
            //await _signInManager.SignOutAsync();
            //await _signInManager.SignInAsync(user, false);

            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
            throw new NotImplementedException();
        }

        //
        // GET: /profiel/wijzig-persoonlijke-gegevens
        [HttpGet, Route("wijzig-persoonlijke-gegevens")]
        public async Task<IActionResult> ChangePersonalInformation()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new ChangePersonalInformationViewModel
            {
                Birthday = user.Birthday,
                Sex = user.Sex,
            };
            return View(model);
        }

        //
        // POST: /profiel/wijzig-persoonlijke-gegevens
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-persoonlijke-gegevens")]
        public async Task<IActionResult> ChangePersonalInformation(ChangePersonalInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                // Overwrites values in currently logged in user to match the new data
                user.Sex = model.Sex;
                user.Birthday = model.Birthday;

                // Saves changes
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePersonalInformationSuccess });
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /profiel/wijzig-studie-gegevens
        [HttpGet, Route("wijzig-studie-gegevens")]
        public async Task<IActionResult> ChangeEducationalInformation()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new ChangeEducationalInformationViewModel
            {
                StudentNumber = user.StudentNumber.ToString(),
                StartdateStudy = user.StartdateStudy,
                StudyType = user.StudyType
            };
            return View(model);
        }

        //
        // POST: /profiel/wijzig-studie-gegevens
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-studie-gegevens")]
        public async Task<IActionResult> ChangeEducationalInformation(ChangeEducationalInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                // Overwrites values in currently logged in user to match the new data
                user.StartdateStudy = model.StartdateStudy;
                user.StudyType = model.StudyType;

                // Saves changes
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeEducationalInformationSuccess });
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        //
        // GET: /profiel/wijzig-adres-gegevens
        [HttpGet, Route("wijzig-adres-gegevens")]
        public async Task<IActionResult> ChangeAddressInformation()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new ChangeAddressInformationViewModel
            {
                AddressCountry = user.AddressCountry,
                AddressCity = user.AddressCity,
                AddressPostalCode = user.AddressPostalCode,
                AddressNumber = user.AddressNumber,
                AddressStreet = user.AddressStreet
            };
            return View(model);
        }

        //
        // POST: /profiel/wijzig-adres-gegevens
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-adres-gegevens")]
        public async Task<IActionResult> ChangeAddressInformation(ChangeAddressInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                // Overwrites values in currently logged in user to match the new data
                user.AddressCountry = model.AddressCountry;
                user.AddressCity = model.AddressCity;
                user.AddressPostalCode = model.AddressPostalCode;
                user.AddressNumber = model.AddressNumber;
                user.AddressStreet = model.AddressStreet;

                // Saves changes
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeAddressInformationSuccess });
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /profiel/wijzig-telefoonnummer
        [HttpGet, Route("wijzig-telefoonnummer")]
        public async Task<IActionResult> ChangePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new ChangePhoneNumberViewModel
            {
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }

        //
        // POST: /profiel/wijzig-telefoonnummer
        [HttpPost, ValidateAntiForgeryToken, Route("wijzig-telefoonnummer")]
        public async Task<IActionResult> ChangePhoneNumber(ChangePhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                // Overwrites values in currently logged in user to match the new data
                user.PhoneNumber = model.PhoneNumber;
                // Saves changes
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePhoneNumberSuccess });
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        #endregion
    }
}