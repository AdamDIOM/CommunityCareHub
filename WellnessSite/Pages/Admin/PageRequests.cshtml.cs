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
        public readonly UserManager<ApplicationUser> um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<Service> requests = default!;

        public PageRequestsModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            this.um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

            if(_context.Service != null)
            {
                requests = await _context.Service.ToListAsync();
                requests = requests.Where(s => !s.Accepted).ToList();
            }
            else
            {
                requests = new List<Service>();
            }
        }

        public async Task OnPostProcessPageRequestAsync(string sid, string accept)
        {
            requests = await _context.Service.ToListAsync();
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

            p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

            requests = await _context.Service.Where(s => !s.Accepted).ToListAsync();
        }
    }
}
