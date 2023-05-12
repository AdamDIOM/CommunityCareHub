using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Admin
{
    public class PageRequestsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<Service> requests = default!;

        public PageRequestsModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            requests = await _context.Service.Where(s => !s.Accepted).ToListAsync();

        }

        public async Task OnPostProcessPageRequestAsync(string sid, string accept)
        {
            Service s = requests.FirstOrDefault(s => s.Id.ToString() == sid)!;
            if (accept == "true")
            {

                s.Accepted = true;
                _context.Attach(s).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Service.Remove(s);
                await _context.SaveChangesAsync();
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            requests = await _context.Service.Where(s => !s.Accepted).ToListAsync();
        }
    }
}
