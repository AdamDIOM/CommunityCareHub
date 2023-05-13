using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Admin.Services
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;

        public DetailsModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }
      public Service Service { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (id == null || _context.Service == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            else 
            {
                Service = service;
            }
            return Page();
        }
    }
}
