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

namespace WellnessSite.Pages.Services
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        public Preferences p;
        private readonly WellnessSiteContext _context;
        [BindProperty(SupportsGet = true)]
        public string qry { get; set; }
        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public IList<Service> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Service != null)
            {
                Service = await _context.Service.Where(s => s.Accepted).ToListAsync();
                if (qry != null && qry.Trim() != "")
                {
                    Service = Service.Where(s =>

                        s.Name.ToLower().Contains(qry.ToLower()) ||
                        s.PhoneNum.ToLower().Contains(qry.ToLower()) ||
                        s.Email.ToLower().Contains(qry.ToLower()) ||
                        s.WebLink.ToLower().Contains(qry.ToLower()) ||
                        s.Address.ToLower().Contains(qry.ToLower()) ||
                        s.Tags.ToLower().Contains(qry.ToLower())
                    ).ToList();
                }
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            Service = Service.OrderBy(s => s.Name).ToList();
        }
    }
}
