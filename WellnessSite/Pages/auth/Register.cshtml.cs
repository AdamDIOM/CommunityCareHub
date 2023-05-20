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
        [Display(Name = "Name")]
        public string Name { get; set; }

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

        [BindProperty]
        [Display(Name = "Security Question 1")]
        public string Q1 { get; set; }
        [BindProperty]
        [Display(Name = "Security Question 2")]
        public string Q2 { get; set; }
        [BindProperty]
        [Display(Name = "Answer 1")]
        public string A1 { get; set; }
        [BindProperty]
        [Display(Name = "Answer 2")]
        public string A2 { get; set; }

        public string? SQError { get; set; } = "";

        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _rm;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<SecQues> sq;

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

            if (_context.SecQues != null)
            {
                sq = await _context.SecQues.ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (email == null)
            {
                email = "";
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (_context.SecQues != null)
            {
                sq = await _context.SecQues.ToListAsync();
            }

            if (RequestAdmin && Q1 == Q2)
            {
                SQError = "The chosen security questions must be different.";
                return Page();
            }

            if(!RequestAdmin)
            {
                ModelState.Remove("A1");
                ModelState.Remove("A2");
            }

            if (ModelState.IsValid) //|| (!RequestAdmin && ModelState.Values))
            {
                // creates new user and redirects to homepage otherwise reloads page if there is an error
                ApplicationUser user;
                if (RequestAdmin)
                {
                    user = new ApplicationUser { UserName = Email, Email = Email, RequestedAdmin = RequestAdmin, Question1 = Q1, Answer1 = A1, Question2 = Q2, Answer2 = A2, Name = Name };
                }
                else
                {
                    user = new ApplicationUser { UserName = Email, Email = Email, RequestedAdmin = RequestAdmin, Name = Name };
                }
                
                var result = await _um.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    
					_context.Preferences.Add(new Preferences(user.Id));
					await _context.SaveChangesAsync();
					await _sim.SignInAsync(user, isPersistent: false);
					await UsefulFunctions.SetStandardAdmin(_um, _rm);
					return RedirectToPage("/Index");
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return Page();
			}
			await UsefulFunctions.SetStandardAdmin(_um, _rm);
			return Page();
        }
    }
}
