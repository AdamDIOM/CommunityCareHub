using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using WellnessSite.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WellnessSite.Models;

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
        private IList<Preferences> prefs;
        public Preferences p;

        public LoginModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }
        public async Task OnGetAsync()
        {
            if (email == null) email = "";
            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
            }
            ApplicationUser u = await _um.GetUserAsync(User);

            if (_sim.IsSignedIn(User) && prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
            {
                p = prefs.FirstOrDefault(p => p.UserID == u.Id)!;
            }
            else
            {
                p = new Preferences("u");
                if (Request.Cookies["user"] == null)
                {
                    Response.Cookies.Append("user", _context.Preferences.Count().ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    _context.Preferences.Add(new Preferences("usr-" + _context.Preferences.Count().ToString()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string uID = "usr-" + Request.Cookies["user"]!;

                    p = prefs.FirstOrDefault(p => p.UserID == uID)!;

                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _sim.PasswordSignInAsync(Email, Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect("/Index");
                }
                ModelState.AddModelError(string.Empty, "Invalid Logon Attempt");
            }
            return Page();
        }
    }
}
