using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using indicium_webapp.Models;
using indicium_webapp.Models.ViewModels.AccountViewModels;
using indicium_webapp.Services;
using indicium_webapp.Data;
using indicium_webapp.Models.InterfaceItemModels;
using Microsoft.EntityFrameworkCore;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace indicium_webapp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/Login
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var applicationUser = _userManager.FindByEmailAsync(model.Email).Result;

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                
                if (result.Succeeded) {
                    if (applicationUser.Status == Status.Lid || applicationUser.Status == Status.Alumni) {
                        _logger.LogInformation(1, "User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    else if(applicationUser.Status == Status.Nieuw)
                    {
                        await _signInManager.SignOutAsync();
                        _logger.LogWarning(2, "User account not approved.");
                        ModelState.AddModelError(string.Empty, "Account niet goedgekeurd, dit kan echter nog even duren.");
                        return View(model);
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Ongeldige aanmeldpoging.");
                        return View(model);
                    }
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning(3, "User account locked out.");
                    ModelState.AddModelError(string.Empty, "Dit account is uitgeschakeld, probeer het later opnieuw.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ongeldige aanmeldpoging.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet, AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            RegisterViewModel model = new RegisterViewModel();

            var checkBoxListItems = new List<CheckBoxListItem>();
            foreach (Commission commission in _context.Commission.ToListAsync().Result)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = commission.CommissionID.ToString(),
                    Display = commission.Name + " - " + commission.Description,
                    IsChecked = false
                });
            }

            model.Commissions = checkBoxListItems;

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Register
        [ValidateRecaptcha, HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            Console.Write(true);
            bool isOld = !(Age(DateTime.ParseExact(model.Birthday, "dd-MM-yyyy", new CultureInfo("nl-NL"))) < 16);
            if (ModelState.IsValid && isOld)
            {
                var user = new ApplicationUser {
                    StudentNumber = Convert.ToInt32(model.StudentNumber),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Email = model.Email,
                    Sex = (Sex)Convert.ToInt32(model.Sex),
                    Birthday = DateTime.ParseExact(model.Birthday, "dd-MM-yyyy", new CultureInfo("nl-NL")),
                    AddressCity = model.AddressCity,
                    AddressStreet = model.AddressStreet,
                    AddressNumber = model.AddressNumber,
                    AddressPostalCode = model.AddressPostalCode,
                    AddressCountry = "Nederland",
                    StartdateStudy = Convert.ToInt32(model.StartdateStudy),
                    RegistrationDate = DateTime.Today,
                    StudyType = (StudyType)Convert.ToInt32(model.StudyType)
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Send an email with this link
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account",
                        new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "Bevestig E-mailadres",
                        $"Klik op de volgende link om jouw e-mailadres te bevestigen: <a href='{callbackUrl}'>link</a>" +
                        $"Groet,<br>" +
                        $"Indicium");

                    await _userManager.AddToRoleAsync(user, "Lid");
                    _logger.LogInformation(3, "Gebruiker heeft een nieuw account aangemaakt met wachtwoord en rol.");
                    ModelState.AddModelError(string.Empty, "Gefeliciteerd u bent geregistreerd. Goedkeuring door de secretaris kan echter nog even duren. Er is " +
                        "een E-mail verstuurd naar het opgegeven adres. Voor u kunt inloggen, moet u deze bevestigen.");

                    // Save the commission interests:
                    if (model.Commissions != null)
                    {
                        var selectedCommissions = model.Commissions.Where(x => x.IsChecked).Select(x => x.ID).ToList();
                        foreach (string commissionID in selectedCommissions)
                        {
                            _context.CommissionMember.Add(new CommissionMember()
                            {
                                // We have to fetch the ID, cause we don't have it yet because this user was just created
                                ApplicationUserID = _userManager.FindByEmailAsync(model.Email).Result.Id,
                                CommissionID = Int32.Parse(commissionID),
                                Status = CommisionMemberStatus.Interesse

                            });

                            await _context.SaveChangesAsync();
                        }
                    }                    
                    return View("login");
                }
                AddErrors(result);              
            }

            if (!isOld)
            {
                ModelState.AddModelError(string.Empty, "U bent niet oud genoeg om een account aan te maken");
                model.Commissions = createCommissionCheckBoxList(); // We need to recreate the list for whatever reason.
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: /Account/ConfirmEmail
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Wachtwoord Resetten",
                   $"Klik op de volgende link om jouw wachtwoord te resetten: <a href='{callbackUrl}'>link</a><br>" +
                   $"Groet,<br>" +
                   $"Indicium");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet, AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet, AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET /Account/AccessDenied
        [HttpGet, AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private static int Age(DateTime birthday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age;
        }

        private List<CheckBoxListItem> createCommissionCheckBoxList()
        {
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            foreach (Commission commission in _context.Commission.ToListAsync().Result)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = commission.CommissionID.ToString(),
                    Display = commission.Name + " - " + commission.Description,
                    IsChecked = false
                });
            }

            return checkBoxListItems;
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
