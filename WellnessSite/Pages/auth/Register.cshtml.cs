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
        [BindProperty]
        public bool RequestAdmin { get; set; }

        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _rm;
        private readonly WellnessSiteContext _context;
        public Preferences p;

        public RegisterModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            WellnessSiteContext con)
        {
            _sim = signInManager;
            _um = userManager;
            _rm = roleManager;
            _context = con;
        }
        public async Task OnGetAsync(string email)
        {

            await UsefulFunctions.SetStandardAdmin(_um, _rm);

            if (email == null)
            {
                email = "";
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);
        }
        public async Task<IActionResult> OnPostAsync()
        {


            if (email == null)
            {
                email = "";
            }


            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (ModelState.IsValid)
            {
                // creates new user and redirects to homepage otherwise reloads page if there is an error
                var user = new ApplicationUser { UserName = Email, Email = Email, RequestedAdmin = RequestAdmin };
                var result = await _um.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    
					_context.Preferences.Add(new Preferences(user.Id));
					await _context.SaveChangesAsync();
					await _sim.SignInAsync(user, isPersistent: false);
					await UsefulFunctions.SetStandardAdmin(_um, _rm);
					return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
			}
			await UsefulFunctions.SetStandardAdmin(_um, _rm);
			return Page();
        }
    }
}
