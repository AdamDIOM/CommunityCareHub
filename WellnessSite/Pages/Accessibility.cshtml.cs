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
        public List<(string name, string code, string css)> colourOptions = new List<(string, string, string)> {
            ("Text Colour", "Text", "--text"),
            ("Highlight Colour", "Highlight", "--highColour"),
            ("Background Colour", "Background", "--backColour"),
            ("Header Colour", "Header", "--headColour"),
            ("Header Text Colour", "HeaderText", "--headText"),
            ("Header Text Colour 2","HeaderTextalt", "--headTextAlt"),
            ("Footer Colour", "Footer", "--footColour"),
            ("Footer Text Colour", "FooterText", "--footText"),
            ("Footer Text Colour2 ", "FooterTextAlt", "--footTextAlt"),
            ("HyperLink Colour", "Link", "--linkColour")
        };

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
            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
			}
			if (_context.Preferences == null) return NotFound();
            if (_sim.IsSignedIn(User))
            {
				ApplicationUser u = await _um.GetUserAsync(User);
				p.UserID = u.Id;
                if(prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
                {
					p = prefs.FirstOrDefault(p => p.UserID == u.Id)!;
				}
                else
                {
                    p = new Preferences(u.Id);
					_context.Preferences.Add(p);
					await _context.SaveChangesAsync();
					prefs = await _context.Preferences.ToListAsync();
				}
            }
            else
            {
                p = new Preferences("usr-x");
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
