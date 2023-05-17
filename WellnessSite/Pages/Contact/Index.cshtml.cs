using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;

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
                // links to email client
                SmtpClient sc = new SmtpClient
                {
                    Credentials = new NetworkCredential("CommunityCareHubIOM@gmail.com", "ydwazxgxyngmcrhw"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587
                };


                MailMessage m = new MailMessage();
                // from gmail address
                m.From = new MailAddress("CommunityCareHubIOM@gmail.com", "Wellness Site Enquiry");
                // sent to Chester email address (emulating restaurant address)
                m.To.Add(new MailAddress("CommunityCareHubIOM@gmail.com"));
                // CC in user's email
                m.CC.Add(new MailAddress(Email));
                // adds text and subject line then sends
                m.Body = $"Message from {Name}:" +
                    $"{Message}.";
                m.Subject = $"Wellness Form Enquiry - {Subject}";
                sc.Send(m);
                // provided nothing failed, redirects to confirmation page to show user their message.
                return Redirect("/Contact/Confirm?Message=" + Message);
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
