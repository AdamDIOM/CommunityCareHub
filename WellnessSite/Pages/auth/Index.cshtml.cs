using WellnessSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Models;

namespace WellnessSite.Pages.auth
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        private IList<Preferences> prefs;
        public Preferences p;

        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Email == null || Email == "")
            {
                return RedirectToPage();
            }
            // attempts to find user in database, if user exists they are redirected to Login page, otherwise to the Register page. Both pass the entered email address as a parameter to the next page
            var user = await _um.FindByNameAsync(Email);
            if(user != null)
            {
                return RedirectToPage("Login", new { email = Email });
            }
            else
            {
                return RedirectToPage("Register", new { email = Email });
            }
        }
    }
}
