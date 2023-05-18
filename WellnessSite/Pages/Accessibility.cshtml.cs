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
			if (choice == "enabled") cookies = UsefulFunctions.CookiesOptions.Enabled;
			else cookies = UsefulFunctions.CookiesOptions.Disabled;
			Response.Cookies.Append("cookies", choice, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            return RedirectToPage();
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
                Response.Cookies.Append("text", size.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            }

			return Redirect("/Accessibility");
		}

        public async Task<IActionResult> OnPostSetPropertiesAsync(string reset, string theme)
        {
			if (_context.Preferences == null) return NotFound();

            Preferences pr = p;
			if (sim.IsSignedIn(User))
			{
				ApplicationUser u = await _um.GetUserAsync(User);
				p.UserID = u.Id;
                if (reset == "true") pr = new Preferences(p.UserID);
                else
                {
                    switch (theme)
                    {
                        case "greyscale":
                            //pr = new Preferences(p.UserID, p.FontSize, AccessibilityOptions.Greyscale);
                            u.CookieState = "greyscale";
                            break;
                        case "contrast":
                            //pr = new Preferences(p.UserID, p.FontSize, AccessibilityOptions.Contrast);
                            u.CookieState = "contrast";
                            break;
                        case "invert":
                            //pr = new Preferences(p.UserID, p.FontSize, AccessibilityOptions.Invert);
                            u.CookieState = "invert";
                            break;
                        default:
                            break;

                    }
                }
                _context.ChangeTracker.Clear();
                _context.Attach(u).State = EntityState.Modified;
                _context.Attach(pr).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
			if(UsefulFunctions.IsCookiesEnabled(this) == UsefulFunctions.CookiesOptions.Enabled)
			{
                if(reset == "true")
                {
                    theme = "standard";
                }
                if(theme != null)
                {
                    Response.Cookies.Append("colour", theme, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                }
                
            }

            return Redirect("/Accessibility");
		}
    }
}
