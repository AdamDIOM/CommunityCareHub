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
                Response.Cookies.Append("colour", "standard", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                ApplicationUser u = await _um.GetUserAsync(User);
                u.CookieState = "standard";
                _context.Attach(u).State = EntityState.Modified;
            }

            return RedirectToPage("/Index");
        }
    }
}
