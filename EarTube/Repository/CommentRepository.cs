using EarTube.Data;
using EarTube.Models;
using EarTube.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Repository
{

    public class CommentRepository 
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async void AddNewComment(Comment comment)
        {
           await _db.Comment.AddAsync(comment);
        }

        public  Comment CommentById(int? id)
        {
            return  _db.Comment.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> CommentDislike(Comment model, string userId)
        {
            var dislikeComment = false;
            var userCommentDislike =  await _db.UserCommentDislike.AnyAsync(u => u.UserId == userId && u.CommentId == model.Id);
            if (!userCommentDislike)
            {
                var newComment = await _db.Comment.FirstOrDefaultAsync(x => x.Id == model.Id);
                var userCommentLike = await _db.UserCommentLike.AnyAsync(u => u.UserId == userId && u.CommentId == model.Id);
                if (!userCommentLike)
                {
                    newComment.CommentDisikes += 1;
                }
                else
                {
                    var likeData = await _db.UserCommentLike.FirstOrDefaultAsync(u => u.UserId == userId && u.CommentId == model.Id);
                    _db.UserCommentLike.Remove(likeData);
                    newComment.CommentLikes -= 1;
                    newComment.CommentDisikes += 1;
                }

                _db.Comment.Update(newComment);
               await _db.UserCommentDislike.AddAsync(new UserCommentDislike { UserId = userId, CommentId = model.Id });
                await  _db.SaveChangesAsync();

                dislikeComment = true;
            }
            return dislikeComment;
        }

        public  async Task<bool> CommentLike(Comment model, string userId)
        {
            var likeComment = false;
            var userCommentLike = await _db.UserCommentLike.AnyAsync(u => u.UserId == userId && u.CommentId == model.Id);
            if (!userCommentLike)
            {
                var newComment = await _db.Comment.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                var userCommentDislike = await _db.UserCommentDislike.AnyAsync(u => u.UserId == userId && u.CommentId == model.Id);
                if (!userCommentDislike)
                {
                    newComment.CommentLikes += 1;
                }
                else
                {
                    var dislikeData = await _db.UserCommentDislike.FirstOrDefaultAsync(u => u.UserId == userId && u.CommentId == model.Id);
                    _db.UserCommentDislike.Remove(dislikeData);
                    newComment.CommentDisikes -= 1;
                    newComment.CommentLikes += 1;
                }

                _db.Comment.Update(newComment);
                _db.UserCommentLike.Add(new UserCommentLike { UserId = userId, CommentId = model.Id });
                await _db.SaveChangesAsync();

                likeComment = true;
            }
            return likeComment;
        }

        public void Save()
        {
            _db.SaveChangesAsync();
        }
    }
}
