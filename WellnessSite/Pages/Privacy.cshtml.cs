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
		public Preferences p;

		public PrivacyModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
		{
			_sim = sim;
			_um = um;
			_context = con;
		}

		public async Task OnGetAsync()
		{

			p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

		}
	}
}