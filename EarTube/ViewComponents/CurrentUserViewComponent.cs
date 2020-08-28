using EarTube.Areas.Identity.Data;
using EarTube.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EarTube.ViewComponents
{
    public class CurrentUserViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CurrentUserViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userDb = await _db.Users.FirstOrDefaultAsync(u => u.Id == claims.Value);

            return View(userDb);
        }
    }
}
