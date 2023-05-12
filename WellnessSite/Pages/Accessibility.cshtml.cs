using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WellnessSite.Migrations;

namespace WellnessSite.Pages
{
    public class AccessibilityModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        private IList<Preferences> prefs;
        [BindProperty]
        public Preferences p { get; set; }
        

        /*
        public int TextSize { get; set; }*/

        public AccessibilityModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
            p = new Preferences("usr-x");
            prefs = new List<Preferences>();
        }

        public async Task<IActionResult> OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            return Page();
        }

        public async Task<IActionResult> OnPostSetText(string reset, int size)
        {
			if (_context.Preferences != null)
			{
				prefs = await _context.Preferences.ToListAsync();
			}
			if (_context.Preferences == null) return NotFound();

            if(size > 0)
            {
                p.TextSize = size;
            }

			Preferences pr = p;
			if (_sim.IsSignedIn(User))
			{
				ApplicationUser u = await _um.GetUserAsync(User);
				p.UserID = u.Id;
			}
			else
			{
				p.UserID = "usr-" + Request.Cookies["user"]!;
			}

            if (reset == "true") pr.TextSize = new Preferences().TextSize;

			_context.ChangeTracker.Clear();
			_context.Attach(pr).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return Redirect("/Accessibility");
		}

        public async Task<IActionResult> OnPostSetPropertiesAsync(string reset, string theme)
        {
			if (_context.Preferences != null)
			{
				prefs = await _context.Preferences.ToListAsync();
			}
			if (_context.Preferences == null) return NotFound();

            Preferences pr = p;
			if (_sim.IsSignedIn(User))
			{
				ApplicationUser u = await _um.GetUserAsync(User);
				p.UserID = u.Id;
			}
			else
			{
                p.UserID = "usr-" + Request.Cookies["user"]!;
			}

			if (reset == "true") pr = new Preferences(p.UserID);
			else
			{
				switch (theme)
				{
					case "greyscale":
						pr = new Preferences(p.UserID, p.TextSize, AccessibilityOptions.Greyscale);
						break;
					case "contrast":
						pr = new Preferences(p.UserID, p.TextSize, AccessibilityOptions.Contrast);
						break;
					case "invert":
						pr = new Preferences(p.UserID, p.TextSize, AccessibilityOptions.Invert);
						break;
					default:
						break;

				}
			}

			_context.ChangeTracker.Clear();
			_context.Attach(pr).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Redirect("/Accessibility");
		}
    }
}
