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
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public byte[]? imgData;

        public EditModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (id == null || _context.Service == null)
            {
                return NotFound();
            }

            var service =  await _context.Service.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            Service = service;
            imgData = service.ImageData;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if(Service.ImageData != null) Service.ImageData = Service.ImageData.ToArray();

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


                if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(Service.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ServiceExists(int id)
        {
          return (_context.Service?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
