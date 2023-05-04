using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages
{
    public class PrivacyModel : PageModel
    {
		private readonly UserManager<ApplicationUser> _um;
		private readonly SignInManager<ApplicationUser> _sim;
		private readonly WellnessSiteContext _context;
		private IList<Preferences> prefs;
		public Preferences p;

		public PrivacyModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
		{
			_sim = sim;
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

			if (_sim.IsSignedIn(User) && prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
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
	}
}