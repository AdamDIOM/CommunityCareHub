using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Admin.Services
{
    public class CreateModel : PageModel
    {
        public readonly UserManager<ApplicationUser> um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<Categories> c;
        public string email;

        public CreateModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            this.um = um;
            _context = con;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

            if(_context.Categories != null)
            {
                c = await _context.Categories.ToListAsync();
            }

            email = (await um.GetUserAsync(User)).Email;

            return Page();

        }

        [BindProperty]
        public Service Service { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, um, _sim, User, this);

            if (_context.Categories != null)
            {
                c = await _context.Categories.ToListAsync();
            }

            email = (await um.GetUserAsync(User)).Email;

            if (!ModelState.IsValid || _context.Service == null || Service == null)
            {
                return Page();
            }

            if (Request.Form.Files.Count >= 1)
            {
                foreach (var file in Request.Form.Files)
                {
                    // copies file data into an array and then into the object
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    Service.ImageData = ms.ToArray();

                    ms.Close();
                    ms.Dispose();
                }
            }

            _context.Service.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
