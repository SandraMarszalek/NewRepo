using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Models;

namespace WebApplication8.Data
{
    public class WebAppContext : IdentityDbContext<ApplicationUser>
    {
        public WebAppContext(DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<RelativesApplicationUser>(e =>
            {
                e.HasKey(p => new { p.GroupId, p.UserId });
                e.HasOne(p => p.Relatives).WithMany(t =>
                t.RelativesApplicationUsers).HasForeignKey(p => p.GroupId);
                e.HasOne(p => p.ApplicationUser).WithMany(tg =>
                tg.RelativesApplicationUsers).HasForeignKey(p => p.UserId);
            });

            builder.Entity<UserMapLocation>(e =>
            {
                e.HasKey(p => new { p.MapId, p.UserId });
                e.HasOne(p => p.Map).WithMany(t =>
                t.UsersMapLocations).HasForeignKey(p => p.MapId);
                e.HasOne(p => p.ApplicationUser).WithMany(tg =>
                tg.UsersMapLocations).HasForeignKey(p => p.UserId);
            });

            
        }

        public DbSet<Map> Map { get; set; }

        public DbSet<UserTasks> UserTasks { get; set; }
        public DbSet<Relatives> Relatives { get; set; }
        public DbSet<RelativesApplicationUser> RelativesApplicationUsers { get; set; }
        public DbSet<UserMapLocation> UsersMapLocations { get; set; }

    }
}
