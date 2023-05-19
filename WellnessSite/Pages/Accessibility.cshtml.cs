using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WellnessSite.Migrations;
using System.Net;

namespace WellnessSite.Pages
{
    public class AccessibilityModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        public readonly SignInManager<ApplicationUser> sim;
        private readonly WellnessSiteContext _context;
        [BindProperty]
        public Preferences p { get; set; }

		public UsefulFunctions.CookiesOptions cookies = UsefulFunctions.CookiesOptions.Unknown;


		public AccessibilityModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            this.sim = sim;
            _um = um;
            _context = con;
            p = new Preferences("usr-x");
        }

        public async Task<IActionResult> OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, sim, User, this);

            return Page();
        }

		public async Task<IActionResult> OnPostCookiesAsync(string choice)
        {
			cookies = UsefulFunctions.IsCookiesEnabled(this);
			p = await UsefulFunctions.GetPreferences(_context, _um, sim, User, this);
            var u = await _um.GetUserAsync(User);
            if (choice == "enabled")
            {
                cookies = UsefulFunctions.CookiesOptions.Enabled;
				if (u != null && u.CookieState == null)
				{
					u.CookieState = "standard";
					_context.Attach(u).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
			}
            else cookies = UsefulFunctions.CookiesOptions.Disabled;
			Response.Cookies.Append(".cookieAcceptedStatusCookie", choice, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            if (choice == "disabled")
            {
                UsefulFunctions.DisableCookies(this);
                if (u != null)
                {
					u.CookieState = null;
					_context.Attach(u).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
            }
            return RedirectToPage("./Accessibility");
		}
		public async Task<IActionResult> OnPostSetText(string reset, int size)
        {
			if (_context.Preferences == null) return NotFound();

            if(size > 0)
            {
                p.FontSize = size;
            }


			if (sim.IsSignedIn(User))
			{
                Preferences pr = p;

                if (reset == "true") pr.FontSize = new Preferences().FontSize;

                ApplicationUser u = await _um.GetUserAsync(User);
				p.UserID = u.Id;
                _context.ChangeTracker.Clear();
                _context.Attach(pr).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else if(UsefulFunctions.IsCookiesEnabled(this) == UsefulFunctions.CookiesOptions.Enabled)

            {
                Response.Cookies.Append(".guestTextSizeCookie", size.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            }

			return RedirectToPage("./Accessibility");
		}

        public async Task<IActionResult> OnPostSetPropertiesAsync(string reset, string theme)
        {
            if (_context.Preferences == null) return NotFound();

            Preferences pr = p;
            if (sim.IsSignedIn(User)) //Currently users have to double click buttons to have effects happen. Page isn't resetting properly.
            {
                ApplicationUser u = await _um.GetUserAsync(User);
                p.UserID = u.Id;
                if (reset == "true")
                {
                    pr = new Preferences(p.UserID);
		    u.CookieState = "standard";
                }
                switch (theme)
                {
                    case "greyscale":
                        u.CookieState = "greyscale";
                        break;
                    case "contrast":
                        u.CookieState = "contrast";
                        break;
                    case "invert":
                        u.CookieState = "invert";
                        break;
                    default:
                        break;
                }
                _context.ChangeTracker.Clear();
                _context.Attach(u).State = EntityState.Modified;
                _context.Attach(pr).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            if (UsefulFunctions.IsCookiesEnabled(this) == UsefulFunctions.CookiesOptions.Enabled)
            {
                if (reset == "true")
                {
                    theme = "standard";
                }
                Response.Cookies.Append(".colourSchemeCookie", theme, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            }
            return RedirectToPage();
        }
    }
}
