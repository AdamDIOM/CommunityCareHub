using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WellnessSite.Data;
using WellnessSite.Migrations;
using WellnessSite.Models;

namespace WellnessSite.Pages.auth
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly WellnessSiteContext _context;
        private readonly SignInManager<ApplicationUser> _sim;
        private IList<Preferences> prefs;
        public IList<Bookmarks> bookmarks;
        public IList<Service> Services;
        [BindProperty]
        public Preferences p { get; set; }

        public string? name;

        [BindProperty]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        [Display(Name = "New Name")]
        public string? Name { get; set; }

        public ProfileModel(SignInManager<ApplicationUser> sim,
            UserManager<ApplicationUser> um, WellnessSiteContext context)
        {
            _um = um;
            _sim = sim;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            ApplicationUser u = await _um.GetUserAsync(User);
            if(u != null && u.Name != null)
            {
                name = u.Name;
            }

            bookmarks = await _context.Bookmarks.Where(b => b.UserID == u.Id).ToListAsync();

            Services = await _context.Service.ToListAsync();

        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            ApplicationUser u = await _um.GetUserAsync(User);
            if (u != null && u.Name != null)
            {
                name = u.Name;
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            bookmarks = await _context.Bookmarks.Where(b => b.UserID == u.Id).ToListAsync();

            Services = await _context.Service.ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            // if user enters correct current password, change their password to the new password
            var user = await _um.GetUserAsync(User);
            if (await _um.CheckPasswordAsync(user, CurrentPassword))
            {
                await _um.ChangePasswordAsync(user, CurrentPassword, Password);
                return Page();
            }
            else
            {
                return Page();
            }
        }


        public async Task<IActionResult> OnPostSetPropertiesAsync(string reset)
        {
            if (_context.Preferences == null) return NotFound();

            ApplicationUser u = await _um.GetUserAsync(User);
            if (u != null && u.Name != null)
            {
                name = u.Name;
            }

            bookmarks = await _context.Bookmarks.Where(b => b.UserID == u.Id).ToListAsync();

            Services = await _context.Service.ToListAsync();

            Preferences pr = p;
            if (_sim.IsSignedIn(User))
            {
                p.UserID = u.Id;
                if (reset == "true") pr = new Preferences(p.UserID);
                
                _context.ChangeTracker.Clear();
                _context.Attach(pr).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            if (UsefulFunctions.IsCookiesEnabled(this) == UsefulFunctions.CookiesOptions.Enabled)
            {
                if (reset == "true")
                {
                    Response.Cookies.Append("colour", "standard", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                }
            }

            return RedirectToPage("./Profile");
        }

    }
}
