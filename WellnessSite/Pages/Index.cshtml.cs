using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<Service> Services { get; set; } = default!;
        public IList<Bookmarks> bookmarks;
        public string? name;
        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
            if (_sim.IsSignedIn(User))
            {
                ApplicationUser u = await _um.GetUserAsync(User);
                if (u != null && u.Name != null)
                {
                    name = u.Name;
                }

                bookmarks = await _context.Bookmarks.Where(b => b.UserID == u.Id).ToListAsync();

                Services = await _context.Service.ToListAsync();
            }
        }

    }
}