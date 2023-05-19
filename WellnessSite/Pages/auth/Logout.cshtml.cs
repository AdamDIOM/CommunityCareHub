using WellnessSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WellnessSite.Pages.auth
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _um;
        private readonly WellnessSiteContext _context;
        public LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> um, WellnessSiteContext context)
        {
            _signInManager = signInManager;
            _um = um;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            if (UsefulFunctions.IsCookiesEnabled(this) == UsefulFunctions.CookiesOptions.Enabled)
            {
                UsefulFunctions.DisableCookies(this);
            }

            return RedirectToPage("/Index");
        }
    }
}
