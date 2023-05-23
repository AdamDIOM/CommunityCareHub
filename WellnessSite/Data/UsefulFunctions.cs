using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using WellnessSite.Models;
using System.Net;
using System.Xml.Linq;
using System.Net.Mail;
using Newtonsoft.Json.Linq;

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

                if (page.Request.Cookies[".guestTextSizeCookie"] == null)
                {
                    page.Response.Cookies.Append(".guestTextSizeCookie", "15", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    textSize = 15;
                }
                else
                {
                    if(!Int32.TryParse(page.Request.Cookies[".guestTextSizeCookie"], out textSize))
                    {
                        textSize = 15;
                        page.Response.Cookies.Append(".guestTextSizeCookie", "15", new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    }
                }
                p.FontSize = textSize;

                /*if (page.Request.Cookies["colour"] == null)
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
                }*/
            }
            return p;
        }

        public static CookiesOptions IsCookiesEnabled(Microsoft.AspNetCore.Mvc.RazorPages.PageModel page)
        {
            if (page.Request.Cookies[".cookieAcceptedStatusCookie"] == "enabled") return CookiesOptions.Enabled;
            else if (page.Request.Cookies[".cookieAcceptedStatusCookie"] == "disabled") return CookiesOptions.Disabled;
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

        public static string GetAccessibilityStylesheet(IHttpContextAccessor accessor)
        {
            switch (accessor.HttpContext.Request.Cookies[".colourSchemeCookie"])
            {
                case "greyscale":
                    return "greyscale";
                    break;
                case "invert":
                    return "invert";
                    break;
                case "contrast":
                    return "contrast";
                    break;
                default:
                    break;
            }
            return "";
        }

        public static void DisableCookies(Microsoft.AspNetCore.Mvc.RazorPages.PageModel page)
        {
			page.Response.Cookies.Append(".colourSchemeCookie", "standard", new CookieOptions { Expires = DateTime.Now });
			page.Response.Cookies.Append(".guestTextSizeCookie", "standard", new CookieOptions { Expires = DateTime.Now });
			page.Response.Cookies.Append(".cookieAcceptanceStatusCookie", "standard", new CookieOptions { Expires = DateTime.Now });
		}

        public static SmtpClient sc = new SmtpClient
        {
            Credentials = new NetworkCredential("CommunityCareHubIOM@gmail.com", "ydwazxgxyngmcrhw"),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            Host = "smtp.gmail.com",
            Port = 587
        };

        public static bool SendEmail(string title, string to, string message, string? cc = "")
        {
            if (to == null || to.Trim() == "" || !to.Contains('@')) return false;
			MailMessage m = new MailMessage();
			m.From = new MailAddress("CommunityCareHubIOM@gmail.com", "Community Care Hub");
			m.To.Add(new MailAddress(to));
            if (cc != null && cc.Trim() != "" && cc.Contains('@'))
            {
				m.CC.Add(new MailAddress(cc));
			}
			m.Body = message;
			m.Subject = title;
			sc.Send(m);
            return true;
		}
    }
}
