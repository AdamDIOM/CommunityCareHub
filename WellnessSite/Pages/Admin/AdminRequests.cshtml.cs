using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Admin
{
    public class AdminRequestsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;

        public IList<ApplicationUser> users = default!;

        public AdminRequestsModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            users = await _context.Users.Where(u => u.RequestedAdmin).ToListAsync();

        }

        public async Task OnPostProcessAdminRequestAsync(string uid, string accept)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == uid);

            if(user != null)
            {
                user.RequestedAdmin = false;
                if(accept == "true") await _um.AddToRoleAsync(user, "OrgAdmin");
                await _context.SaveChangesAsync();
            }
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            users = await _context.Users.Where(u => u.RequestedAdmin).ToListAsync();
        }
    }
}
