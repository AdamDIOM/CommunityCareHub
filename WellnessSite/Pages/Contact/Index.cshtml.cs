using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WellnessSite.Pages.Contact
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        public readonly SignInManager<ApplicationUser> sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;

        [BindProperty]
        [Required]
        public string Name { get; set; }
        [BindProperty]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        public string Subject { get; set; }
        [BindProperty]
        [Required]
        public string Message { get; set; }

        public string email;

        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            this.sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, sim, User, this);
            ApplicationUser u = await _um.GetUserAsync(User);
            email = (u != null) ? u.Email : "";
        }

        public async Task<IActionResult> OnPostAsync()
        {

            ApplicationUser u = await _um.GetUserAsync(User);
            email = (u != null) ? u.Email : "";
            p = await UsefulFunctions.GetPreferences(_context, _um, sim, User, this);


            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (!UsefulFunctions.SendEmail(
                    title: $"Wellness Form Enquiry - {Subject}",
                    to: "CommunityCareHubIOM@gmail.com",
                    message: $"Message from {Name}:\n\n" + $"{Message}",
                    cc: Email
                    )) throw new Exception();

                return RedirectToPage("./Confirm", new { Message = Message });
            }
            // if something goes wrong, the page is reloaded with an error message
            catch (Exception e)
            {
                ModelState.AddModelError("Customer Contact Field Error", "Invalid data in customer contact form" + e.Message);
                return Page();
            }

        }
    }
}
