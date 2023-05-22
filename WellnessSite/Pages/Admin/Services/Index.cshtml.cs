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
        [BindProperty(SupportsGet = true)]
        public string qry { get; set; }

        public IList<Service> Service { get; set; } = default!;

        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }
        public async Task OnGetAsync()
        {

            ApplicationUser u = await _um.GetUserAsync(User);

            if (_context.Service != null)
            {
                Service = await _context.Service.ToListAsync();
                if (!await _um.IsInRoleAsync(await _um.GetUserAsync(User), "Admin"))
                {
                    Service = Service.Where(s => s.Maintainer == u.Email).ToList();
                }
            }
            if (qry != null && qry.Trim() != "")
            {
                Service = Service.Where(s =>
                {
                    if (s.WebLink == null) s.WebLink = "";
                    if (s.Address == null) s.Address = "";
                    if (s.Town == null) s.Town = "";
                    if (s.Tags == null) s.Tags = "";

                    return s.Name.ToLower().Contains(qry.ToLower()) ||
                    s.Category.ToLower().Contains(qry.ToLower()) ||
                    s.Address.ToLower().Contains(qry.ToLower()) ||
                    s.Town.ToLower().Contains(qry.ToLower()) ||
                    s.Tags.ToLower().Contains(qry.ToLower());
                }


                ).ToList();
            }


            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
        }
    }
}

   
