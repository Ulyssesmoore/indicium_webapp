using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using indicium_webapp.Models.ViewModels;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Identity;
using indicium_webapp.Services;
using indicium_webapp.Models.ViewModels.HomeViewModels;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace indicium_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        
        public HomeController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        
        public IActionResult Index()
        {
            //HttpContext.Session.SetInt32("amount", 0);
            return View();
        }

        [Route("/over")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/contact")]
        public async Task<IActionResult> Contact()
        {
            ViewBag.LoggedInUser = await GetCurrentUserAsync();

            return View();
        }

        [Route("/contact"), ValidateRecaptcha, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {                
                var emailaddress = _emailSender.Options.SmtpUsername;
                var staticEmailadress = "svindicium@gmail.com";
                await _emailSender.SendEmailAsync(staticEmailadress, model.Subject, "Van: " + model.Email + "<br/>Bericht: " + model.Message);

                ModelState.AddModelError(string.Empty, "Contactformulier is verstuurd.");
            }

            return View(model);
        }

        [Route("/statuten")]
        public IActionResult Statuten()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

    }       
}
