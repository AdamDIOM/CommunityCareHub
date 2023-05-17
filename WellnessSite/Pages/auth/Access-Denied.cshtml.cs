using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.auth
{
    public class Access_DeniedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;

        public Access_DeniedModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

        }
    }
}
