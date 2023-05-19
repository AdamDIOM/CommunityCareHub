using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using WellnessSite.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WellnessSite.Models;
using WellnessSite.Migrations;

namespace WellnessSite.Pages.auth
{
    public class LoginModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string email { get; set; }
        [Required]
        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly WellnessSiteContext _context;
        private readonly RoleManager<IdentityRole> _rm;
        private IList<Preferences> prefs;
        public Preferences p;

        public LoginModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con, RoleManager<IdentityRole> rm)
        {
            _sim = sim;
            _um = um;
            _context = con;
            _rm = rm;
        }
        public async Task OnGetAsync()
        {
            ModelState.Clear();
			await UsefulFunctions.SetStandardAdmin(_um, _rm);
			if (email == null) email = "";
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (email == null) email = "";

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (ModelState.IsValid)
            {
                ApplicationUser u = await _um.FindByEmailAsync(Email);
                if(u != null && (await _um.IsInRoleAsync(u, "OrgAdmin") || await _um.IsInRoleAsync(u, "Admin")))
                {
                    var r = await _um.CheckPasswordAsync(u, Password);
                    if (r) return RedirectToPage("AdminLogin", new { UID = u.Id });
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Logon Attempt");
                        return Page();
                    }
                    // redirect to adminlogin for sec qs
                }
                // else do below stuff
                var result = await _sim.PasswordSignInAsync(Email, Password, false, false);
                
                if (result.Succeeded)
                {
                    if(u != null && u.CookieState != null)
                    {
                        Response.Cookies.Append(".colourSchemeCookie", u.CookieState, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    }
                    await UsefulFunctions.SetStandardAdmin(_um, _rm);
					return Redirect("../Index");
                }
                ModelState.AddModelError(string.Empty, "Invalid Logon Attempt");
			}
			await UsefulFunctions.SetStandardAdmin(_um, _rm);
			return Page();
        }
    }
}
