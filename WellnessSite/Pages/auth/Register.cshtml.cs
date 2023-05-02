using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WellnessSite.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;

namespace WellnessSite.Pages.auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string email { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly WellnessSiteContext _context;
        private IList<Preferences> prefs;
        public Preferences p;

        public RegisterModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            WellnessSiteContext con)
        {
            _sim = signInManager;
            _um = userManager;
            _roleManager = roleManager;
            _context = con;
        }
        public async Task OnGetAsync()
        {
            /*
            // sets up admin@ltpr.it to always have admin access
            bool exists = await _roleManager.RoleExistsAsync("Admin");
            if(!exists)
            {
                var role = new IdentityRole { Name = "Admin" };
                var result = await _roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync("admin@ltpr.it");
                    if(user != null)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    
                }
            }*/
            if (email == null)
            {
                email = "";
            }

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
                // creates new user and redirects to homepage otherwise reloads page if there is an error
                var user = new ApplicationUser { UserName = Email, Email = Email };
                var result = await _um.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await _sim.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
