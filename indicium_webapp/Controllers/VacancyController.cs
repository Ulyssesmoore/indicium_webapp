using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace indicium_webapp.Controllers
{
    public class VacancyController : Controller
    {

        [Route("/vacatures")]
        public IActionResult Index()
        {
            return View();
        }
    }
}