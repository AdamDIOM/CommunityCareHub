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
    public class IndexModel : PageModel
    {
        private readonly WellnessSiteContext _context;

        public readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        public Preferences p;

        public IList<Service> Service { get;set; } = default!;
        public IList<AdminAccess> AA { get; set; } = default!;

        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }
        public async Task OnGetAsync()
        {
            if(_context.AdminAccess != null)
            {
                AA = await _context.AdminAccess.ToListAsync();
                ApplicationUser u = await _um.GetUserAsync(User);
                
                if (_context.Service != null)
                {
                    Service = await _context.Service.ToListAsync();
                    if (!await _um.IsInRoleAsync(await _um.GetUserAsync(User), "Admin"))
                    {
                        Service = Service.Where(s => s.Maintainer == u.Id).ToList();
                    }
                }
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
        }
    }
}
