using EarTube.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Areas.Identity.Data
{
    public class DbInitializer : IDbInitializer
    {
        
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                
            }
            var emailAdmin = "admin@gmail.com";
            if (_db.Users.Any(r => r.Email == emailAdmin)) return;

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = emailAdmin,
                Email = emailAdmin,
                FirstName = "Samuel",
                LastName = "Olanrewaju",
                EmailConfirmed = true,
                PhoneNumber = "1112223333"
            }, "Admin123*").GetAwaiter().GetResult();

            //IdentityUser user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");

            //await _userManager.AddToRoleAsync(user, SD.ManagerUser);

        }

    }
}
   
