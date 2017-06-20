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
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Je wachtwoord is veranderd."
                : message == ManageMessageId.Error ? "Er heeft een fout plaatsgevonden."
                : message == ManageMessageId.ChangeEmailSuccess ? "Je e-mailadres is met succes gewijzigd."
                : message == ManageMessageId.ChangePersonalInformationSuccess ? "Je persoonlijke gegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeEducationalInformationSuccess ? "Je studiegegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeAddressInformationSuccess ? "Je adresgegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangePhoneNumberSuccess ? "Je telefoonnummer is met succes gewijzigd."
                : "";

            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                return View("Error");
            }

            var model = new IndexViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AddressCity = user.AddressCity,
                AddressPostalCode = user.AddressPostalCode,
                AddressNumber = user.AddressNumber,
                AddressStreet = user.AddressStreet,
                AddressCountry = user.AddressCountry,
                PhoneNumber = user.PhoneNumber,
                StartdateStudy = user.StartdateStudy,
                StudyType = user.StudyType,
                Sex = user.Sex,
                Birthday = user.Birthday.ToString("dd MMMM yyyy", new CultureInfo("nl-NL")),
                StudentNumber = user.StudentNumber.ToString()
            };
            
            var loggedInUser = await GetCurrentUserAsync();
            ViewBag.Roles = await _userManager.GetRolesAsync(loggedInUser);

            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost, ValidateAntiForgeryToken]
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
        // GET: /Manage/AbortMembership
        [HttpGet]
        public IActionResult AbortMembership()
        {
            return View();
        }

        //
        // POST: /Manage/AbortMembership
        [HttpPost, ValidateAntiForgeryToken]
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
        // GET: /Manage/ChangeEmail
        [HttpGet]
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
        // POST: /Manage/ChangeEmail
        [HttpPost, ValidateAntiForgeryToken]
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
                    var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    await _userManager.ChangeEmailAsync(user, model.Email, token);
                    user.UserName = model.Email;
                    await _userManager.UpdateNormalizedEmailAsync(user);
                    await _userManager.UpdateNormalizedUserNameAsync(user);

                    // Saves changes
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    // Relogs user to refresh the _LoginPatial.cshtml
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, false);

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
        // GET: /Manage/ChangePersonalInformation
        [HttpGet]
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
        // POST: /Manage/ChangePersonalInformation
        [HttpPost, ValidateAntiForgeryToken]
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
        // GET: /Manage/ChangeEducationalInformation
        [HttpGet]
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
        // POST: /Manage/ChangeEducationalInformation
        [HttpPost, ValidateAntiForgeryToken]
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
        // GET: /Manage/ChangeAddressInformation
        [HttpGet]
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
        // POST: /Manage/ChangeAddressInformation
        [HttpPost, ValidateAntiForgeryToken]
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
        // GET: /Manage/ChangePhoneNumber
        [HttpGet]
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
        // POST: /Manage/ChangePhoneNumber
        [HttpPost, ValidateAntiForgeryToken]
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