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
using indicium_webapp.Models.ManageViewModels;
using indicium_webapp.Services;
using indicium_webapp.Data;
using Microsoft.EntityFrameworkCore;

namespace indicium_webapp.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          ApplicationDbContext context,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Je wachtwoord is veranderd."
                : message == ManageMessageId.SetPasswordSuccess ? "Je wachtwoord is gezet."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Je twee-factor authenticatie provider is gezet."
                : message == ManageMessageId.Error ? "Er heeft een fout plaatsgevonden."
                : message == ManageMessageId.AddPhoneSuccess ? "Je telefoonnummer is toegevoegd."
                : message == ManageMessageId.RemovePhoneSuccess ? "Je telefoonnummer is verwijderd"
                : message == ManageMessageId.ChangeEmailSuccess ? "Je email-adres is met succes gewijzigd."
                : message == ManageMessageId.ChangePersonalInformationSuccess ? "Je persoonlijke gegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeEducationalInformationSuccess ? "Je studiegegevens zijn met succes gewijzigd."
                : message == ManageMessageId.ChangeAddressInformationSuccess ? "Je adresgegevens zijn met succes gewijzigd."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new IndexViewModel
            {
                Name = user.FirstName + " " + user.LastName,
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
                Birthday = user.Birthday,
                Iban = user.Iban//,
                //BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Je veiligheidscode is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Het is niet gelukt je telefoonnummer te verifiëren.");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }
        
        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                Iban = user.Iban
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangePersonalInformation
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                user.Iban = model.Iban;
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
                StartdateStudy = user.StartdateStudy,
                StudyType = user.StudyType
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangeEducationalInformation
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "Externe login is verwijderd."
                : message == ManageMessageId.AddLoginSuccess ? "Externe login is toegevoegd."
                : message == ManageMessageId.Error ? "Er heeft een fout plaatsgevonden."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback), "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = ManageMessageId.Error;
            if (result.Succeeded)
            {
                message = ManageMessageId.AddLoginSuccess;
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            ChangeEmailSuccess,
            ChangeEducationalInformationSuccess,
            ChangePersonalInformationSuccess,
            ChangeAddressInformationSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
