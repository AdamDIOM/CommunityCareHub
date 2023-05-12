using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;

namespace WellnessSite.Pages.Contact
{
    public class ConfirmModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        private IList<Preferences> prefs;
        public Preferences p;
        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }

        public ConfirmModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
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
