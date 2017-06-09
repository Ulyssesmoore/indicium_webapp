using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Data;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<Activity>> Get()
        {
            return await _context.Activity.Include(t => t.ActivityType)
                .ToListAsync();
        }

        // GET: api/v1/activities/{id}
        [HttpGet("{id}")]
        public async Task<Activity> Get(int? id)
        {
            return await _context.Activity
                .SingleOrDefaultAsync(m => m.ActivityID == id);
        }
    }
}
