using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Data;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace indicium_webapp.api
{
    [Route("api/v1/[controller]")]
    public class MembersController : Controller
    {
        private ApplicationDbContext _context;
        public MembersController(ApplicationDbContext context) {
            _context = context; 
        }

        // GET api/v1/members
        [HttpGet]
        public async Task<IEnumerable<ApplicationUser>> Get()
        {
            return await _context.ApplicationUser.ToListAsync();
        }

        // GET api/v1/members/5
        [HttpGet("{id}")]
        public async Task<ApplicationUser> Get(String id)
        {
            return await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}
