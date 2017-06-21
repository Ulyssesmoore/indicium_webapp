using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using indicium_webapp.Models.ViewModels;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Identity;

namespace indicium_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
