using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Admin
{
    public class IndexModel : PageModel
    {
		public readonly UserManager<ApplicationUser> um;
		private readonly SignInManager<ApplicationUser> _sim;
		private readonly WellnessSiteContext _context;
		public Preferences p;
		public string welcomeMsg;
		public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
		{
			_sim = sim;
			this.um = um;
			_context = con;
		}

		public async Task OnGetAsync()
		{
			p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

			var u = await um.GetUserAsync(User);

            if (u != null && u.Name != null && u.Name.Trim() != "")
			{
				welcomeMsg = "Hi there, " + u.Name;
			}
			else
			{
				welcomeMsg = "Hi there";
			}
		}
	}
}
