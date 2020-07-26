using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarTube.Areas.Identity.Data;
using EarTube.Data;
using EarTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EarTube.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> GetCommentById(int id)
        {
            
            var data = await _db.Comment.Where(c => c.SongId == id).ToListAsync();
            
            return View(data);
        }

        public IActionResult AddComment(int songId)
        {
            
            var model = new Comment();
            model.SongId = songId;
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {

                var userId = _userManager.GetUserId(this.HttpContext.User);
                comment.UserId = userId;
                await _db.Comment.AddAsync(comment);

                await _db.SaveChangesAsync();

                return View();
            }

            return View(comment);
        }

    }
}