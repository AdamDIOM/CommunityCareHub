using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WellnessSite.Data;
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
        public Preferences p;

        public string username;

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


        public ProfileModel(SignInManager<ApplicationUser> sim,
            UserManager<ApplicationUser> um, WellnessSiteContext context)
        {
            _um = um;
            _sim = sim;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            username = _um.GetUserName(User);

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            ApplicationUser u = await _um.GetUserAsync(User);

            bookmarks = await _context.Bookmarks.Where(b => b.UID == u.Id).ToListAsync();

            Services = await _context.Service.ToListAsync();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            username = _um.GetUserName(User);

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

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
    }
}
