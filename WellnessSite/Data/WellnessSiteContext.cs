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
            modelBuilder.Entity<Bookmarks>().ToTable("Bookmarks");
            modelBuilder.Entity<Categories>().ToTable("Categories");
            modelBuilder.Entity<SecQues>().ToTable("SecQues");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }

        public DbSet<Service> Service { get; set; } = default!;
        public DbSet<Preferences> Preferences { get; set; } = default!;
        public DbSet<Bookmarks> Bookmarks { get; set; } = default!;
        public DbSet<Categories> Categories { get; set; } = default!;
        public DbSet<SecQues> SecQues { get; set; } = default!;
    }
}
