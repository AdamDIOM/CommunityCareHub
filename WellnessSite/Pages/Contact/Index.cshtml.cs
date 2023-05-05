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
        private IList<Preferences> prefs;
        public Preferences p;

        [BindProperty]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        public string Name { get; set; }
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
            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
            }
            ApplicationUser u = await _um.GetUserAsync(User);
            email = (u != null) ? u.Email : "";

            if (sim.IsSignedIn(User) && prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
            {
                p = prefs.FirstOrDefault(p => p.UserID == u.Id)!;
            }
            else
            {
                p = new Preferences("u");
                if (Request.Cookies["user"] == null)
				{
					Response.Cookies.Append("user", _context.Preferences.Count().ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
					p = new Preferences("usr-" + _context.Preferences.Count().ToString());
					_context.Preferences.Add(p);
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

            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
            }
            ApplicationUser u = await _um.GetUserAsync(User);
            email = (u != null) ? u.Email : "";

            if (sim.IsSignedIn(User) && prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
            {
                p = prefs.FirstOrDefault(p => p.UserID == u.Id)!;
            }
            else
            {
                p = new Preferences("u");
                if (Request.Cookies["user"] == null)
                {
                    Response.Cookies.Append("user", _context.Preferences.Count().ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    p = new Preferences("usr-" + _context.Preferences.Count().ToString());
                    _context.Preferences.Add(p);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string uID = "usr-" + Request.Cookies["user"]!;

                    p = prefs.FirstOrDefault(p => p.UserID == uID)!;

                }
            }




            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // links to email client
                SmtpClient sc = new SmtpClient
                {
                    Credentials = new NetworkCredential("adam.drummond9@gmail.com", "xjiglfnicnibaffc"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587
                };


                MailMessage m = new MailMessage();
                // from gmail address
                m.From = new MailAddress("adam.drummond9@gmail.com", "Wellness Site Enquiry");
                // sent to Chester email address (emulating restaurant address)
                m.To.Add(new MailAddress("2126671@chester.ac.uk"));
                // CC in user's email
                m.CC.Add(new MailAddress(Email));
                // adds text and subject line then sends
                m.Body = $"{Name} said {Message}.";
                m.Subject = "Wellness Form Enquiry";
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
