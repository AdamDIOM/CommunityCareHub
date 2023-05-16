using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WellnessSite.Models;

namespace WellnessSite.Data
{
    public class UsefulFunctions
    {

        public enum CookiesOptions
        {
            Unknown,
            Disabled,
            Enabled
        }


        public static async Task<Preferences> GetPreferences(WellnessSiteContext _context, UserManager<ApplicationUser> _um, SignInManager<ApplicationUser> _sim, System.Security.Claims.ClaimsPrincipal? User, Microsoft.AspNetCore.Mvc.RazorPages.PageModel page)
        {
            IList<Preferences> prefs = default!;
            Preferences p = new Preferences();
            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
            }
            
            if (_sim.IsSignedIn(User))
            {
                ApplicationUser u = await _um.GetUserAsync(User);
                //p.UserID = u.Id;
                if (prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
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
            else if(IsCookiesEnabled(page) == CookiesOptions.Enabled)
            {
                p = new Preferences("u");

                int textSize;

                if (page.Request.Cookies["text"] == null)
                {
                    page.Response.Cookies.Append("text", "15", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    textSize = 15;
                }
                else
                {
                    if(!Int32.TryParse(page.Request.Cookies["text"], out textSize))
                    {
                        textSize = 15;
                        page.Response.Cookies.Append("text", "15", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    }
                }

                if (page.Request.Cookies["colour"] == null)
                {
                    page.Response.Cookies.Append("colour", "standard", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                }
                else
                {
                    switch (page.Request.Cookies["colour"])
                    {
                        case "greyscale":
                            p = new Preferences("x", textSize, AccessibilityOptions.Greyscale);
                            break;
                        case "invert":
                            p = new Preferences("x", textSize, AccessibilityOptions.Invert);
                            break;
                        case "contrast":
                            p = new Preferences("x", textSize, AccessibilityOptions.Contrast);
                            break;
                        default:
                            p = new Preferences("x");
                            p.FontSize = textSize;
                            break;
                    }
                }
            }
            return p;
        }

        public static CookiesOptions IsCookiesEnabled(Microsoft.AspNetCore.Mvc.RazorPages.PageModel page)
        {
            if (page.Request.Cookies["cookies"] == "enabled") return CookiesOptions.Enabled;
            else if (page.Request.Cookies["cookies"] == "disabled") return CookiesOptions.Disabled;
            else return CookiesOptions.Unknown;
        }

        public static async Task SetStandardAdmin(UserManager<ApplicationUser> _um, RoleManager<IdentityRole> _rm)
        {
			// sets up admin@wellness.im to always have admin access
			bool exists = await _rm.RoleExistsAsync("Admin");
            bool exists2 = await _rm.RoleExistsAsync("OrgAdmin");
			IdentityResult result = IdentityResult.Success;
            if (!exists)
            {
                var role = new IdentityRole { Name = "Admin" };
                result = await _rm.CreateAsync(role);
            }
            if (!exists2)
            {
                var role = new IdentityRole { Name = "OrgAdmin" };
                await _rm.CreateAsync(role);
            }
            if (exists || result.Succeeded)
			{
				var user = await _um.FindByEmailAsync("admin@wellness.im");
				if (user != null)
				{
					await _um.AddToRoleAsync(user, "Admin");
				}
			}
		}
    }
}
