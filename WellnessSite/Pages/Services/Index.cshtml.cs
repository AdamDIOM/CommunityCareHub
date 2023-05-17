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
        public string Query { get; set; }
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
                if(Query != null && Query.Trim() != "" && Query.Length == 5 && Query.Substring(0,3) == "cat")
                {
                    Service = Service.Where(s => s.Name.ToUpper()[0] == Query[4]).ToList();
                }
                else if (Query != null && Query.Trim() != "")
                {
                    Service = Service.Where(s =>
					{
						if (s.WebLink == null) s.WebLink = "";
						if (s.Address == null) s.Address = "";
						if (s.Town == null) s.Town = "";
						if (s.Tags == null) s.Tags = "";

						return s.Name.ToLower().Contains(Query.ToLower()) ||
                        s.Category.ToLower().Contains(Query.ToLower()) ||
						s.Address.ToLower().Contains(Query.ToLower()) ||
						s.Town.ToLower().Contains(Query.ToLower()) ||
						s.Tags.ToLower().Contains(Query.ToLower());
                    }

                        
                    ).ToList();
                }
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            Service = Service.OrderBy(s => s.Name).ToList();
        }
    }
}
