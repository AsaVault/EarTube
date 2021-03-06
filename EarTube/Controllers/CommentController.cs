﻿using System.Linq;
using System.Threading.Tasks;
using EarTube.Areas.Identity.Data;
using EarTube.Data;
using EarTube.Helpers;
using EarTube.Models;
using EarTube.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace EarTube.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommentRepository _comment;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, ICommentRepository comment, ILogger<CommentController> logger)
        {
            _db = db;
            _userManager = userManager;
            _comment = comment;
            _logger = logger;
        }
        public IActionResult GetCommentById(int id)
        {

            var data = _comment.CommentById(id);

            return View(data);
        }


        //Old logic
        public async Task<IActionResult> RepoGetCommentById(int id)
        {
            var data = await _db.Comment.Where(c => c.SongId == id).ToListAsync();

            return View(data);
        }

        [HttpGet]
        public IActionResult AddComment(int songId)
        {
            var model = new Comment
            {
                SongId = songId
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddComment(Comment comment, bool isSuccess = false)
        {
            try
            {
                var userId = _userManager.GetUserId(this.HttpContext.User);
                comment.UserId = userId;

                if (ModelState.IsValid)
                {
                    _comment.AddNewComment(comment);
                    _comment.Save();
                    //comment.Description = " ";
                    ViewBag.IsSuccess = isSuccess;
                    TempData["Alert"] = true;

                    //Adding Json here
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_AddComment", _db.Comment.ToListAsync()) });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            // JSON Return
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_AddComment", comment) });
        }

        //old logic
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RepoAddComment(Comment comment, bool isSuccess = false)
        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            comment.UserId = userId;

            if (ModelState.IsValid)
            {
                //var model = new Comment();
                //model.SongId = comment.SongId;

                await _db.Comment.AddAsync(comment);

                await _db.SaveChangesAsync();
                //comment.Description = " ";
                ViewBag.IsSuccess = isSuccess;
                TempData["Alert"] = true;

                //Adding Json here
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_AddComment", _db.Comment.ToListAsync()) });
            }
            // JSON Return
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_AddComment", comment) });
        }


        //Youtube Like song
        [Route("like-comment/{id}", Name = "likeComment")]

        public async Task<IActionResult> LikeComment(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var data = _comment.CommentById(id);
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool likeComment = await _comment.CommentLike(data, userId);
            //data.SongLike += 1;
            if (likeComment)
            {
                TempData["LikeAlert"] = true;
                TempData["AlreadyLikeAlert"] = false;
                return RedirectToAction("GetSong", "Song", new { id = data.SongId });
            }

            TempData["LikeAlert"] = false;
            TempData["AlreadyLikeAlert"] = true;
            return RedirectToAction("GetSong", "Song", new { id = data.SongId });
        }


        //Youtube Like song
        [Route("dislike-comment/{id}", Name = "dislikeComment")]

        public async Task<IActionResult> DisikeComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data =  _comment.CommentById(id);
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool dislikeComment = await _comment.CommentDislike(data, userId);
            //data.SongLike += 1;
            if (dislikeComment)
            {
                TempData["LikeAlert"] = true;
                TempData["AlreadyLikeAlert"] = false;
                return RedirectToAction("GetSong", "Song", new { id = data.SongId });
            }

            TempData["LikeAlert"] = false;
            TempData["AlreadyLikeAlert"] = true;
            return RedirectToAction("GetSong", "Song", new { id = data.SongId });
        }
    }
}