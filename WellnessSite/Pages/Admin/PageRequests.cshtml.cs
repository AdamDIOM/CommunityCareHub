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
                if(s.Maintainer != null && s.Maintainer.Trim() != "")
                {
                    UsefulFunctions.SendEmail(
                    title: "Community Care Hub Page Accepted",
                    to: s.Maintainer,
                    message: $"Hello there,\nThis message is to notify you as the maintainer of {s.Name}, the page has been accepted.\nMany Thanks,\nThe Community Care Hub Team"
                    );
                }
                
            }
            else
            {
                _context.Service.Remove(s);
                await _context.SaveChangesAsync();
				if (s.Maintainer != null && s.Maintainer.Trim() != "")
				{
                    string str = $"Hello there,\nThis message is to notify you as the maintainer of {s.Name}, the page has been declined.\nDetails:";

					if (s.Name != null) str += $"Name:\n{s.Name}\n";
                    if (s.Category != null) str += $"Category:\n{s.Category}\n";
                    if (s.Description != null) str += $"Description:\n{s.Description}\n";
                    if (s.PhoneNum != null) str += $"Phone Number:\n{s.PhoneNum}\n";
                    if (s.Email != null) str += $"Email:\n{s.Email}\n";
                    if (s.Address != null) str += $"Address:\n{s.Address}\n";
                    if (s.Town != null) str += $"Town:\n{s.Town}\n";
                    if (s.Postcode != null) str += $"Postcode:\n{s.Postcode}\n";
                    if (s.WebLink != null) str += $"WebLink:\n{s.WebLink}\n";
                    if (s.Other != null) str += $"Other:\n{s.Other}\n";
                    if (s.Tags != null) str += $"Tags:\n{s.Tags}\n";

                    str += $"\nKind Regards,\nThe Community Care Hub Team";

					UsefulFunctions.SendEmail(
					    title: "Community Care Hub Page Declined",
					    to: s.Maintainer,
					    message: str
					);
				}
			}

            p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

            requests = await _context.Service.Where(s => !s.Accepted).ToListAsync();
        }

    }
}
