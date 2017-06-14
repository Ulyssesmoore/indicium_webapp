using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Data;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace indicium_webapp.Controllers.api.v1
{
    [Route("api/v1/[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: api/v1/activities
        [HttpGet]
        public IEnumerable<Activity> Get()
        {
            return _context.Activity
                .Include(t => t.ActivityType)
                .Include(t => t.SignUps)
                .ToList();
        }

        // GET: api/v1/activities/{id}
        [HttpGet("{id}")]
        public Activity Get(int? id)
        {
            return _context.Activity
                .Include(t => t.ActivityType)
                .Include(t => t.SignUps)
                .SingleOrDefault(m => m.ActivityID == id);
        }
    }
}
