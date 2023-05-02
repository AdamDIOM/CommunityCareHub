using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WellnessSite.Data
{
    public class WellnessSiteContext : IdentityDbContext<ApplicationUser>
    {
        public WellnessSiteContext (DbContextOptions<WellnessSiteContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<Preferences>().ToTable("Preferences");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }

        public DbSet<WellnessSite.Models.Service> Service { get; set; } = default!;
        public DbSet<Preferences> Preferences { get; set; } = default!;
    }
}
