using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarTube.Areas.Identity.Data;
using EarTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EarTube.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Song { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<UserSongLike> UserSongLike { get; set; }
        public DbSet<News> News { get; set; }
        //public DbSet<Likes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
